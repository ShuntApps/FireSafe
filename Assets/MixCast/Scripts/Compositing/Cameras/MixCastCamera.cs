/**********************************************************************************
* Blueprint Reality Inc. CONFIDENTIAL
* 2019 Blueprint Reality Inc.
* All Rights Reserved.
*
* NOTICE:  All information contained herein is, and remains, the property of
* Blueprint Reality Inc. and its suppliers, if any.  The intellectual and
* technical concepts contained herein are proprietary to Blueprint Reality Inc.
* and its suppliers and may be covered by Patents, pending patents, and are
* protected by trade secret or copyright law.
*
* Dissemination of this information or reproduction of this material is strictly
* forbidden unless prior written permission is obtained from Blueprint Reality Inc.
***********************************************************************************/

#if UNITY_STANDALONE_WIN
using BlueprintReality.MixCast.Data;
using System.Collections.Generic;
using UnityEngine;

namespace BlueprintReality.MixCast
{
    public class MixCastCamera : CameraComponent
    {
        public static List<MixCastCamera> ActiveCameras { get; protected set; }
        public static MixCastCamera Current { get; protected set; } //Assigned to the MixCastCamera that is being processed between FrameStarted and FrameEnded

        public static MixCastCamera FindCamera(CameraConfigContext context)
        {
            foreach(var cam in ActiveCameras)
            {
                if(cam.context.Data == context.Data)
                {
                    return cam;
                }
            }
            return null;
        }

        private static BlitTexture noCamBlit;

        static MixCastCamera()
        {
            ActiveCameras = new List<MixCastCamera>();
        }

        public static event System.Action<MixCastCamera> FrameStarted;
        public static event System.Action<MixCastCamera> FrameEnded;

        public static event System.Action<MixCastCamera> GameRenderStarted;
        public static event System.Action<MixCastCamera> GameRenderEnded;

        public Transform displayTransform;
        public Camera gameCamera;

        public Texture Output { get; protected set; }

        private bool IsInUseElsewhere
        {
            get { return context != null && context.Data != null && context.Data.isInUseElsewhere; }
        }

        private bool IsPluggedIn
        {
            get { return context != null && context.Data != null && !context.Data.unplugged; }
        }

        public bool IsRunningInRealtime
        {
            get
            {
                if (context.Data == null || context.Data.unplugged)
                    return false;

                return IsBeingDisplayedOnDesktop || IsVideoRecording || IsVideoStreaming;
            }
        }

        public bool IsBeingDisplayedOnDesktop
        {
            get
            {
                if (context.Data == null)
                    return false;

                for (int i = 0; i < MixCastSdkData.Desktop.DisplayingCameraIds.Count; i++)
                    if (MixCastSdkData.Desktop.DisplayingCameraIds[i] == context.Data.id)
                        return true;

                return false;
            }
        }

        public bool IsVideoRecording
        {
            get
            {
                if (context.Data == null)
                    return false;

                for (int i = 0; i < MixCastSdkData.Cameras.Count; i++)
                    if (MixCastSdkData.Cameras[i].Identifier == context.Data.id)
                        return MixCastSdkData.Cameras[i].VideoRecordingEnabled;

                return false;
            }
        }

        public bool IsVideoStreaming
        {
            get
            {
                if (context.Data == null)
                    return false;

                for (int i = 0; i < MixCastSdkData.Cameras.Count; i++)
                    if (MixCastSdkData.Cameras[i].Identifier == context.Data.id)
                        return MixCastSdkData.Cameras[i].VideoStreamingEnabled;

                return false;
            }
        }

        private int width;
        private int height;

        protected override void OnEnable()
        {
			DirectShowDevices.DeviceEnumerator.AddRef();
			if (gameCamera != null)
            {
                gameCamera.stereoTargetEye = StereoTargetEyeMask.None;
                gameCamera.enabled = false;
            }

            base.OnEnable();

            if( context.Data != null )
            {
                if (string.IsNullOrEmpty(context.Data.deviceName))
                {
                    context.Data.deviceUseAutoFoV = false; // resolve edge case where someone with old 1.5.2 data with autofov = true and no cam input, would see auto fov enabled by default on first virtual camera created in 2.0
                }
                BuildOutput();
            }
                
            HandleDataChanged();

            ActiveCameras.Add(this);

            if(context.Data != null) {
                context.Data.unplugged = !FeedDeviceManager.IsVideoDeviceConnected( context.Data.deviceAltName, context.Data.deviceType );
            }
        }
        protected override void OnDisable()
        {
            ReleaseOutput();

            ActiveCameras.Remove(this);

            base.OnDisable();

            if (gameCamera != null)
                gameCamera.enabled = true;
			
			DirectShowDevices.DeviceEnumerator.Release();
		}

        protected override void HandleDataChanged()
        {
            base.HandleDataChanged();

            if (context.Data == null)
                return;

            if (context.Data.deviceFoV > 0 && gameCamera != null)
                gameCamera.fieldOfView = context.Data.deviceFoV;

            UpdateOutputDimensions();

            if (!IsVideoRecording && !IsVideoStreaming)
            {
                if (width != Output.width || height != Output.height)
                {
                    ReleaseOutput();
                    BuildOutput();
                }
            }
        }

        protected virtual void LateUpdate()
        {
            HandleDataChanged();
        }

        protected virtual void BuildOutput()
        {
            UpdateOutputDimensions();

            Output = new RenderTexture(width, height, 24, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear)
            {
                antiAliasing = CalculateAntiAliasingValue(),
                useMipMap = false,
#if UNITY_5_5_OR_NEWER
                autoGenerateMips = false,
#else
                generateMips = false,
#endif
            };

            if (gameCamera != null)
            {
                gameCamera.targetTexture = Output as RenderTexture;
                gameCamera.aspect = (float)width / height;
            }
        }

        protected void UpdateOutputDimensions()
        {
            if (context.Data == null)
                return;

            width = CameraUtilities.CalculateCameraRenderWidth(context.Data);
            height = CameraUtilities.CalculateCameraRenderHeight(context.Data);
        }

        protected virtual void ReleaseOutput()
        {
            if (Output != null)
            {
                (Output as RenderTexture).Release();
                Output = null;
                if (gameCamera != null)
                    gameCamera.targetTexture = null;
            }
        }

        protected int CalculateAntiAliasingValue()
        {
#if UNITY_5_6_OR_NEWER
            if (gameCamera != null && !gameCamera.allowMSAA)
                return 1;
#endif
            if (gameCamera.actualRenderingPath == RenderingPath.DeferredShading)
                return 1;

            if (MixCastSdkData.ProjectSettings.overrideQualitySettingsAA)
                return 1 << MixCastSdkData.ProjectSettings.overrideAntialiasingVal;    //saved as 2^x
            else
                return Mathf.Max(QualitySettings.antiAliasing, 1);  //Disabled can equal 0 rather than 1
        }

        public virtual void RenderScene()
        {

        }

        public float CalculatePlayerDistance()
        {
            int hmdCount = 0;
            Vector3 devicePosSum = Vector3.zero;
            for (int i = 0; i < MixCastSdkData.TrackedObjects.Count; i++)
            {
                var trackedObject = MixCastSdkData.TrackedObjects[i];

                if (!trackedObject.Connected || trackedObject.HideFromUser)
                    continue;

                Vector3 objectPosition;
                if (string.IsNullOrEmpty(context.Data.actorTrackingDeviceId))
                {
                    if (trackedObject.ObjectType != Data.ObjectType.HMD)
                        continue;

                    objectPosition = trackedObject.Position.unity;

                    if (trackedObject.Source == TrackingSource.OCULUS)
                        objectPosition += trackedObject.Rotation.unity * VRInfo.OCULUS_RIFT_HMD_TO_HEAD;
                    else if (trackedObject.Source == TrackingSource.OPENVR)
                        objectPosition += trackedObject.Rotation.unity * VRInfo.VIVE_HMD_TO_HEAD;
                }
                else
                {
                    if (trackedObject.Identifier != context.Data.actorTrackingDeviceId)
                        continue;

                    objectPosition = trackedObject.Position.unity;

                    if (trackedObject.ObjectType == ObjectType.HMD)
                    {
                        if (trackedObject.Source == TrackingSource.OCULUS)
                            objectPosition += trackedObject.Rotation.unity * VRInfo.OCULUS_RIFT_HMD_TO_HEAD;
                        else if (trackedObject.Source == TrackingSource.OPENVR)
                            objectPosition += trackedObject.Rotation.unity * VRInfo.VIVE_HMD_TO_HEAD;
                    }
                }

                devicePosSum += objectPosition;
                hmdCount++;
            }

            if (hmdCount == 0)
                return 0;

            devicePosSum /= hmdCount;

            Vector3 cameraPos;
            Quaternion cameraRot;
            CameraUtilities.GetCameraPose(context.Data, out cameraPos, out cameraRot);
            Matrix4x4 camMat = Matrix4x4.TRS(cameraPos, cameraRot, Vector3.one);

            Vector3 playerPosInCameraSpace = camMat.inverse.MultiplyPoint(devicePosSum);
            return playerPosInCameraSpace.z;
        }

        #region Event Firing
        protected void RenderGameCamera( Camera cam, RenderTexture target )
		{
			if (target.width == 0 || target.height == 0)
				return;

            RenderTexture oldTarget = cam.targetTexture;
            if( GameRenderStarted != null)
                GameRenderStarted( this );

            cam.targetTexture = target;
            cam.aspect = (float)target.width / target.height;
            cam.Render();

            if (GameRenderEnded != null)
                GameRenderEnded(this);

            cam.targetTexture = oldTarget;
            if (oldTarget != null)
                cam.aspect = (float)oldTarget.width / oldTarget.height;
            else
                cam.ResetAspect();
        }

		protected void StartFrame()
        {
            Current = this;
            if ( FrameStarted != null )
                FrameStarted(this);
        }

        protected void CompleteFrame()
        {
            Current = null;
            
            if (FrameEnded != null)
                FrameEnded(this);

            Graphics.SetRenderTarget(null);
        }
        #endregion
    }
}
#endif
