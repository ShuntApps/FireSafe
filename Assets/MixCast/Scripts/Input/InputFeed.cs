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
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace BlueprintReality.MixCast
{
    public class InputFeed : CameraComponent
    {
        public class FramePlayerData
        {
            public float playerDist;

            public Vector3 playerHeadPos;
            public Vector3 playerBasePos;
            public Vector3 playerLeftHandPos;
            public Vector3 playerRightHandPos;
        }

        private const string KEYWORD_CROP_PLAYER = "CROP_PLAYER";
        private const string KEYWORD_FLIP_X = "PXL_FLIP_X";
        private const string KEYWORD_FLIP_Y = "PXL_FLIP_Y";

        protected virtual float POLLING_DELAY { get { return 0.5f; } }
        protected virtual float POLLING_MIN_CLEAR_WAIT { get { return 1f; } }

        public static List<InputFeed> ActiveFeeds = new List<InputFeed>();
        public static InputFeed FindInputFeed(CameraConfigContext context)
        {
            for (int i = 0; i < ActiveFeeds.Count; i++)
            {
                var feed = ActiveFeeds[i];
                if (feed.context.Data == context.Data)
                {
                    return feed;
                }
            }
            return null;
        }
        public static event Action<InputFeed> OnProcessInputStart;
        public static event Action<InputFeed> OnProcessInputEnd;

        public virtual bool ShouldRender { get { return wasCameraConnected; } }

        public Material blitMaterial;

        public virtual Texture Texture
        {
            get
            {
                return null;
            }
        }

        public bool FlipX
        {
            get
            {
                return context.Data.deviceData.flipX;
            }
        }
        public bool FlipY
        {
            get
            {
                return SrcHasFlippedY != context.Data.deviceData.flipY;
            }
        }
        protected virtual bool SrcHasFlippedY
        {
            get
            {
                return false;
            }
        }

        private Material processTextureMat;
        public Material ProcessTextureMaterial
        {
            get
            {
                return processTextureMat;
            }
        }


        private RenderTexture processedTexture;
        public RenderTexture ProcessedTexture
        {
            get
            {
                return processedTexture;
            }
        }

        public static event Action<InputFeed> OnBeforeProcessTexture;
        public static event Action<InputFeed> OnAfterProcessTexture;

        public event Action OnRenderStarted;
        public event Action OnRenderEnded;

        private Material setMaterial;
        protected FrameDelayQueue<FramePlayerData> frames;

        protected SharedTexture.SharedTextureSender rgbTextureSender = new SharedTexture.SharedTextureSender();


        protected float timeSinceLastDevicePoll = 0f;
        protected bool wasCameraConnected = true;
        protected bool isResettingCamera = false;
        protected MixCastData.OutputMode lastValidMode = MixCastData.OutputMode.Buffered;

        protected bool isVideoFeedReady = false;

        protected virtual void Awake()
        {
			processTextureMat = new Material(Shader.Find("Hidden/MixCast/Process Input"));
            frames = null;
        }

        protected override void OnEnable()
        {
			DirectShowDevices.DeviceEnumerator.AddRef();
			frames = new FrameDelayQueue<FramePlayerData>();

            ActiveFeeds.Add(this);

            base.OnEnable();
            Invoke("HandleDataChanged", 0.01f);
        }

        protected override void OnDisable()
        {
            ActiveFeeds.Remove(this);

            rgbTextureSender.Clear();

            frames = null;

            ClearTexture();
            base.OnDisable();
			DirectShowDevices.DeviceEnumerator.Release();
		}

        private void OnApplicationQuit()
        {
            frames = null;
            ClearTexture();
        }

        protected virtual void LateUpdate()
        {
            if (context == null || context.Data == null) 
                return;
			
            timeSinceLastDevicePoll += Time.deltaTime;
            if( timeSinceLastDevicePoll >= POLLING_DELAY + (isResettingCamera ? POLLING_MIN_CLEAR_WAIT : 0f) )
			{
				bool isConnected = FeedDeviceManager.IsVideoDeviceConnected(context.Data.deviceAltName, context.Data.deviceType);

                if( wasCameraConnected && !isConnected )
				{
                    isResettingCamera = true;
                    ClearTexture();
                    context.Data.unplugged = true;
                }
                else if( !wasCameraConnected && isConnected )
				{
					timeSinceLastDevicePoll = 0f;
                    FeedDeviceManager.BuildDeviceList(context.Data.deviceType);
                    SetTexture();
                    context.Data.unplugged = false;
                    isResettingCamera = false;
                } else {
                    timeSinceLastDevicePoll = 0f;
                }
                wasCameraConnected = isConnected;
            }
        }

        public virtual void StartRender()
        {
            if( context.Data == null || frames == null || isVideoFeedReady == false)
				return;
		
            frames.delayDuration = context.Data.bufferTime;
            frames.Update();

            ProcessTexture();

            if (blitMaterial != null && ProcessedTexture != null)
            {
                blitMaterial.mainTexture = ProcessedTexture;

                if (context.Data.croppingData.active)
                    blitMaterial.EnableKeyword(KEYWORD_CROP_PLAYER);

                FramePlayerData oldFrameData = frames.OldestFrameData;
                if (oldFrameData != null)
                {
                    blitMaterial.SetFloat("_PlayerDist", oldFrameData.playerDist);
                    blitMaterial.SetVector("_PlayerHeadPos", oldFrameData.playerHeadPos);
                    blitMaterial.SetVector("_PlayerLeftHandPos", oldFrameData.playerLeftHandPos);
                    blitMaterial.SetVector("_PlayerRightHandPos", oldFrameData.playerRightHandPos);
                    blitMaterial.SetVector("_PlayerBasePos", oldFrameData.playerBasePos);
                }

                float scale = MixCastRoomBehaviour.GetAverageScale();
                blitMaterial.SetFloat("_PlayerScale", scale);

                blitMaterial.SetFloat("_PlayerHeadCropRadius", context.Data.croppingData.headRadius);
                blitMaterial.SetFloat("_PlayerHandCropRadius", context.Data.croppingData.handRadius);
                blitMaterial.SetFloat("_PlayerFootCropRadius", context.Data.croppingData.baseRadius);

                //update the player's depth for the material
                FrameDelayQueue<FramePlayerData>.Frame<FramePlayerData> nextFrame = frames.GetNewFrame();
                if (nextFrame.data == null)
                    nextFrame.data = new FramePlayerData();

                FillTrackingData(nextFrame);
            }

            if (OnRenderStarted != null)
                OnRenderStarted();
        }

        public virtual void StopRender()
        {
            if (context.Data.croppingData.active)
                blitMaterial.DisableKeyword(KEYWORD_CROP_PLAYER);

            if (ProcessedTexture != null)
                rgbTextureSender.UpdateFromTexture(context.Data.id + "_rgb", ProcessedTexture);
            else
                rgbTextureSender.Clear();

            if (OnRenderEnded != null)
                OnRenderEnded();
        }

		protected virtual void SetTexture() { }
		
		protected virtual void ClearTexture() { }

        protected virtual void ProcessTexture( RenderTextureFormat format = RenderTextureFormat.ARGB32 )
        {
            if (processedTexture != null && Texture != null)
            {
                if (processedTexture.width != Texture.width || processedTexture.height != Texture.height)
                {
                    processedTexture.Release();
                    processedTexture = null;
                }
            }
            if (processedTexture == null && Texture != null)
            {
                processedTexture = new RenderTexture(Texture.width, Texture.height, 0, format, RenderTextureReadWrite.sRGB)
                {
                    useMipMap = false,
#if UNITY_5_5_OR_NEWER
                    autoGenerateMips = false,
#else
                    generateMips = false,
#endif
                };
                processedTexture.wrapMode = TextureWrapMode.Clamp;
            }

            ProcessTextureMaterial.mainTexture = Texture;

            PrepareProcessMaterial(ProcessTextureMaterial);

            if (OnBeforeProcessTexture != null)
                OnBeforeProcessTexture(this);
            if (OnProcessInputStart != null)
                OnProcessInputStart(this);

            Graphics.SetRenderTarget(ProcessedTexture);
            GL.Clear(true, true, Color.clear);

            bool oldSRGB = GL.sRGBWrite;
            GL.sRGBWrite = false;
            Graphics.Blit(Texture, processedTexture, ProcessTextureMaterial);
            GL.sRGBWrite = oldSRGB;

            Graphics.SetRenderTarget(null);

            if (OnProcessInputEnd != null)
                OnProcessInputEnd(this);
            if (OnAfterProcessTexture != null)
                OnAfterProcessTexture(this);

            CleanupProcessMaterial(ProcessTextureMaterial);
        }

        protected virtual void PrepareProcessMaterial(Material mat)
        {
            FramePlayerData oldFrameData = frames.OldestFrameData;
            if (oldFrameData != null)
                mat.SetFloat("_PlayerDist", oldFrameData.playerDist);

            if (FlipX)
                mat.EnableKeyword(KEYWORD_FLIP_X);
            if (FlipY)
                mat.EnableKeyword(KEYWORD_FLIP_Y);
        }

        protected virtual void CleanupProcessMaterial(Material mat)
        {
            if( context.Data.croppingData.active )
                processTextureMat.DisableKeyword(KEYWORD_CROP_PLAYER);

            if (FlipX)
                mat.DisableKeyword(KEYWORD_FLIP_X);
            if (FlipY)
                mat.DisableKeyword(KEYWORD_FLIP_Y);
        }

        void FillTrackingData(FrameDelayQueue<FramePlayerData>.Frame<FramePlayerData> frame)
        {
            MixCastCamera cam = MixCastCamera.FindCamera(context);
            if (cam != null && cam.gameCamera != null)
                frame.data.playerDist = cam.gameCamera.transform.TransformVector(Vector3.forward).magnitude * cam.CalculatePlayerDistance(); //Scale distance by camera scale

            frame.data.playerHeadPos = GetTrackingPosition( Data.AssignedRole.HEAD );
            frame.data.playerBasePos = new Vector3(frame.data.playerHeadPos.x, 0, frame.data.playerHeadPos.z);
            frame.data.playerLeftHandPos = GetTrackingPosition( Data.AssignedRole.LEFT_HAND);
            frame.data.playerRightHandPos = GetTrackingPosition( Data.AssignedRole.RIGHT_HAND);

            if (MixCastCameras.Instance != null)
            {
                Transform roomTransform = MixCastCameras.Instance.transform;
                frame.data.playerHeadPos = roomTransform.TransformPoint(frame.data.playerHeadPos);
                frame.data.playerBasePos = roomTransform.TransformPoint(frame.data.playerBasePos);
                frame.data.playerLeftHandPos = roomTransform.TransformPoint(frame.data.playerLeftHandPos);
                frame.data.playerRightHandPos = roomTransform.TransformPoint(frame.data.playerRightHandPos);
            }
        }
        Vector3 GetTrackingPosition( Data.AssignedRole role )
        {
            Data.TrackedObject trackedObj = TrackedObjectManager.GetTrackedObjectByRole(role);
            if (trackedObj == null)
                trackedObj = TrackedObjectManager.GetTrackedObjectByRole(Data.AssignedRole.HEAD);

            if( trackedObj != null )
            {
                Vector3 pos = trackedObj.Position.unity;
                if( trackedObj.AssignedRole == Data.AssignedRole.HEAD )
                {
                    if (trackedObj.Source == Data.TrackingSource.OPENVR)
                        pos += trackedObj.Rotation.unity * VRInfo.VIVE_HMD_TO_HEAD;
                    else if (trackedObj.Source == Data.TrackingSource.OCULUS)
                        pos += trackedObj.Rotation.unity * VRInfo.OCULUS_RIFT_HMD_TO_HEAD;
                }
                return pos;
            }
            
            Camera cam = VRInfo.FindHMDCamera();
            if (cam != null)
            {
                if (MixCastRoomBehaviour.ActiveRoomBehaviours.Count > 0)
                    return MixCastRoomBehaviour.ActiveRoomBehaviours[0].transform.InverseTransformPoint(cam.transform.position);
                else
                    return cam.transform.position;
            }
            return Vector3.zero;
        }


        void UpdateSharedTexture()
        {

        }
    }
}
#endif
