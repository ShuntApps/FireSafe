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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Intel.RealSense;

namespace BlueprintReality.MixCast.RealSense
{
    public class RealSenseInputFeed : InputFeed
    {
        public static readonly string[] DEVICE_NAMES = new string[]
        {
            "Intel(R) RealSense(TM) 415",
            "Intel(R) RealSense(TM) 435",
            "Intel(R) RealSense(TM) 435i"
        };

        public static bool IsVideoInputRealSense(string name)
        {
            for (int i = 0; i < DEVICE_NAMES.Length; i++)
                if (DEVICE_NAMES[i] == name)
                    return true;
            return false;
        }

        public const float DEFAULT_GAIN_VAL = 0.5f;
        public const float DEFAULT_EXPOSURE_VAL = 0.22702f;//0.0117f;
        public const float DEFAULT_INFRARED_EXPOSURE_VAL = 0.37106f;//0.05109f;
        public const float DEFAULT_WHITEBALANCE_VAL = 0.5f;

        public const int DEFAULT_DECIMATION_MAGNITUDE = 2;

        protected override float POLLING_MIN_CLEAR_WAIT { get { return 2f; } }

        public Texture RGBTexture { get; protected set; }
        public Texture DepthTexture { get; protected set; }

        public override Texture Texture { get { return RGBTexture; } }
        
        protected override bool SrcHasFlippedY
        {
            get
            {
                return true;
            }
        }

        public Sensor colorSensor { get; private set; }
        public Sensor depthSensor { get; private set; }
        public RsDevice device { get; private set; }

        public RsProcessingPipe pipe { get; private set; }

        private int lastWidth, lastHeight;
        private string lastSerial;

        private GameObject deviceObj;
        private RsStreamTextureRenderer colorTex;
        private RsStreamTextureRenderer depthTex;

        private int depthWidth;
        private int depthHeight;

        public Material DepthMaskMaterial { get; private set; }
        private Material BlurMaterial;

        private int decimationAmount
        {
            get
            {
                return (context.Data.deviceData.enableDecimationFilter) ? (context.Data.deviceData.decimationMagnitude) : (1);
            }
        }

        // Textures for rendering and blurring depth mask
        private int _maskDecimation = 2;
        private RenderTexture _depthmask = null;
        private RenderTexture DepthMask
        {
            get
            {                
                if(_depthmask == null)
                {
                    _maskDecimation = decimationAmount;
                    _depthmask = new RenderTexture(depthWidth / _maskDecimation / 2, depthHeight / _maskDecimation / 2, 0, RenderTextureFormat.Default);
                }
                else if(_maskDecimation != decimationAmount)
                {
                    //Still confirming...but releasing this at runtime could potentially trigger problems.
                    RenderTexture.active = null;
                    _depthmask.Release();
                    _depthmask = new RenderTexture(depthWidth / _maskDecimation / 2, depthHeight / _maskDecimation / 2, 0, RenderTextureFormat.Default);
                }
                return _depthmask;
            }
        }
        private RenderTexture DepthMaskBlur;

        private RsProcessingProfile rsProcessingProfile;
        private RsAlign rsAlign;
        public RsDecimationFilter rsDecimationFilter;
        public RsSpatialFilter rsSpatialFilter;
        public RsTemporalFilter rsTemporalFilter;

        private SharedTexture.SharedTextureSender depthTextureSender = new SharedTexture.SharedTextureSender();

        public override void StartRender()
        {
            isVideoFeedReady = true;
            base.StartRender();

            if (context.Data.subjectHasPixelDepth)
            {
                blitMaterial.SetTexture("_RsDepthMap", DepthTexture);
                blitMaterial.SetFloat("_RsDepthMultiplier", 100);
                blitMaterial.SetTexture("_BgStaticDepthMask", DepthMask);
            }

            if (FlipX)
                blitMaterial.EnableKeyword("PXL_FLIP_X");
            if (FlipY)
                blitMaterial.EnableKeyword("PXL_FLIP_Y");
        }
        public override void StopRender()
        {
            if (FlipX)
                blitMaterial.DisableKeyword("PXL_FLIP_X");
            if (FlipY)
                blitMaterial.DisableKeyword("PXL_FLIP_Y");

            depthTextureSender.UpdateFromTexture(context.Data.id + "_depth", DepthTexture);

            base.StopRender();
        }

        protected override void ProcessTexture(RenderTextureFormat format = RenderTextureFormat.ARGB32)
        {
            base.ProcessTexture(RenderTextureFormat.ARGB32);
        }

        protected override void PrepareProcessMaterial(Material mat)
        {
            mat.EnableKeyword("DEPTH_REALSENSE");
            mat.SetTexture("_RsDepthMap", DepthTexture);
            mat.SetFloat("_RsDepthMultiplier", 100);

            mat.SetTexture("_BgStaticDepthMask", DepthMask);

            base.PrepareProcessMaterial(mat);
        }
        protected override void CleanupProcessMaterial(Material mat)
        {
            mat.DisableKeyword("DEPTH_REALSENSE");

            base.CleanupProcessMaterial(mat);
        }

        private string cachedAltName = null;
        private string cachedSerialNumber = null;
        private string targetSerialNumber
        {
            get
            {
                if (cachedSerialNumber == null || cachedAltName == null || !cachedAltName.Equals(context.Data.deviceAltName))
                {
                    var rs_device_serial = RealSenseUtility.GetDeviceSerialFromAltName(context.Data.deviceAltName);

                    if (!string.IsNullOrEmpty(rs_device_serial))
                    {
                        cachedAltName = context.Data.deviceAltName;
                        cachedSerialNumber = rs_device_serial;
                    }
                }
                return cachedSerialNumber;
            }
        }

        protected override void SetTexture()
        {
            ApplyDefaultDeviceParams();

            if (deviceObj != null && device != null)
            {
                return; // avoid the constant creation of devices that throw exceptions when the RS cam isn't plugged in.
            }

            if (!IsVideoInputRealSense(context.Data.deviceName))
                return;

            int width = System.Convert.ToInt32(context.Data.deviceFeedWidth);
            int height = System.Convert.ToInt32(context.Data.deviceFeedHeight);
            int framerate = 30;

            FeedDeviceManager.OutputInfo depthOutput = FindBestDepthOutput(width, height);
            if (depthOutput == null)
            {
                return; // not supported
            }
            depthWidth = (int)depthOutput.width;
            depthHeight = (int)depthOutput.height;

            deviceObj = new GameObject("Device");
            deviceObj.transform.SetParent(transform);
            deviceObj.SetActive(false);

            device = deviceObj.AddComponent<RsDevice>();

            device.DeviceConfiguration.mode = RsConfiguration.Mode.Live;

            if (!string.IsNullOrEmpty(targetSerialNumber))
            {
                device.DeviceConfiguration.RequestedSerialNumber = targetSerialNumber;
                lastSerial = targetSerialNumber;
            }
            else
            {
                Debug.LogError("Can't find appropriate real sense!");
                return;
            }

            device.DeviceConfiguration.Profiles = new RsVideoStreamRequest[2];

            device.DeviceConfiguration.Profiles[0].Stream = Stream.Color;
            device.DeviceConfiguration.Profiles[0].StreamIndex = 0;
            device.DeviceConfiguration.Profiles[0].Width = width;
            device.DeviceConfiguration.Profiles[0].Height = height;
            device.DeviceConfiguration.Profiles[0].Framerate = framerate;
            device.DeviceConfiguration.Profiles[0].Format = Format.Rgb8;

            device.DeviceConfiguration.Profiles[1].Stream = Stream.Depth;
            device.DeviceConfiguration.Profiles[1].StreamIndex = 0;
            device.DeviceConfiguration.Profiles[1].Width = depthWidth;
            device.DeviceConfiguration.Profiles[1].Height = depthHeight;
            device.DeviceConfiguration.Profiles[1].Framerate = framerate;
            device.DeviceConfiguration.Profiles[1].Format = Format.Z16;

            DepthMaskMaterial = new Material(Shader.Find("Hidden/MixCast/Depth Mask"));
            BlurMaterial = new Material(Shader.Find("Hidden/MixCast/Separable Blur"));
            
            _maskDecimation = decimationAmount;
            _depthmask = new RenderTexture(depthWidth / _maskDecimation / 2, depthHeight / _maskDecimation / 2, 0, RenderTextureFormat.Default); ;

            device.processMode = RsDevice.ProcessMode.Multithread;//.UnityThread;

            pipe = deviceObj.AddComponent<RsProcessingPipe>();
            pipe.Source = device;

            rsAlign = ScriptableObject.CreateInstance<RsAlign>();
            rsAlign.enabled = true;
            rsAlign._alignTo = Stream.Color;

            rsDecimationFilter = ScriptableObject.CreateInstance<RsDecimationFilter>();
            rsDecimationFilter.enabled = context.Data.deviceData.enableDecimationFilter;
            rsDecimationFilter._streamFilter = Stream.Depth;
            rsDecimationFilter._formatFilter = Format.Z16;
            rsDecimationFilter._filterMagnitude = context.Data.deviceData.decimationMagnitude;

            rsSpatialFilter = ScriptableObject.CreateInstance<RsSpatialFilter>();
            rsSpatialFilter.enabled = context.Data.deviceData.enableSpatialFilter;
            rsSpatialFilter._filterMagnitude = context.Data.deviceData.spatialMagnitude;
            rsSpatialFilter._filterSmoothAlpha = context.Data.deviceData.spatialSmoothAlpha;
            rsSpatialFilter._filterSmoothDelta = context.Data.deviceData.spatialSmoothDelta;
            rsSpatialFilter._holeFillingMode = context.Data.deviceData.spatialHoleFillingMode;

            rsTemporalFilter = ScriptableObject.CreateInstance<RsTemporalFilter>();
            rsTemporalFilter.enabled = context.Data.deviceData.enableTemporalFilter;
            rsTemporalFilter._filterSmoothAlpha = context.Data.deviceData.temporalSmoothAlpha;
            rsTemporalFilter._filterSmoothDelta = context.Data.deviceData.temporalSmoothDelta;
            rsTemporalFilter._temporalPersistence = context.Data.deviceData.temporalPersistence;

            rsProcessingProfile = ScriptableObject.CreateInstance<RsProcessingProfile>();
            rsProcessingProfile._processingBlocks = new List<RsProcessingBlock>();
            rsProcessingProfile._processingBlocks.Add(rsAlign);
            rsProcessingProfile._processingBlocks.Add(rsDecimationFilter);
            rsProcessingProfile._processingBlocks.Add(rsSpatialFilter);
            rsProcessingProfile._processingBlocks.Add(rsTemporalFilter);
            pipe.profile = rsProcessingProfile;

            colorTex = deviceObj.AddComponent<RsStreamTextureRenderer>();
            colorTex.Source = pipe;
            colorTex._stream = Stream.Color;
            colorTex._format = Format.Rgb8;
            colorTex._streamIndex = 0;
            colorTex.textureBinding = new RsStreamTextureRenderer.TextureEvent();
            colorTex.textureBinding.AddListener(HandleColorFrameUpdated);

            depthTex = deviceObj.AddComponent<RsStreamTextureRenderer>();
            depthTex.Source = pipe;
            depthTex._stream = Stream.Depth;
            depthTex._format = Format.Z16;
            depthTex._streamIndex = 0;
            depthTex.textureBinding = new RsStreamTextureRenderer.TextureEvent();
            depthTex.textureBinding.AddListener(HandleDepthFrameUpdated);

            var initSettingsComponent = deviceObj.AddComponent<InitRealSenseSettings>();
            initSettingsComponent.feed = this;

            deviceObj.SetActive(true);

            lastWidth = width;
            lastHeight = height;

            base.SetTexture();

            OnProcessInputStart += ProcessDepthMask;

            ReinitializeLibAVSettings();
            context.Data.isInUseElsewhere = false;
        }

        private void HandleColorFrameUpdated(Texture tex)
        {
            RGBTexture = tex;

            if (device == null || device.ActiveProfile == null)
            {
                return;
            }

            // Find colour sensor
            foreach (var sensor in device.ActiveProfile.Device.Sensors)
            {
                if (!sensor.Options[Option.DepthUnits].Supported)
                {
                    colorSensor = sensor;
                    UpdateColorSensorOptions();
                    break;
                }
            }
        }

        private void HandleDepthFrameUpdated(Texture tex)
        {
            DepthTexture = tex;

            if (device == null || device.ActiveProfile == null)
            {
                return;
            }

            // Find depth sensor
            foreach (var sensor in device.ActiveProfile.Device.Sensors)
            {
                if (sensor.Options[Option.DepthUnits].Supported)
                {
                    depthSensor = sensor;
                    UpdateDepthSensorOptions();
                }
            }
        }

        private void ProcessDepthMask(InputFeed feed)
        {
            if (context.Data == null)
            {
                return;
            }

            // Set up material                       
            DepthMaskMaterial.EnableKeyword("DEPTH_REALSENSE");
            DepthMaskMaterial.SetFloat("_RsDepthMultiplier", 100);
            DepthMaskMaterial.mainTexture = DepthTexture;

            if (context.Data.depthCutoffData.active)
            {
                DepthMaskMaterial.EnableKeyword("DEPTH_CUTOFF");
                DepthMaskMaterial.SetFloat("_BgStaticDepth_MaxDepth", context.Data.depthCutoffData.maxDepth);
            }
            else
            {
                DepthMaskMaterial.DisableKeyword("DEPTH_CUTOFF");
            }

            Graphics.Blit(DepthTexture, DepthMask, DepthMaskMaterial);

            if (context.Data.staticDepthData.active && context.Data.staticDepthData.blurEffect)
            {
                DepthMaskBlur = RenderTexture.GetTemporary(depthWidth / decimationAmount / 2, depthHeight / decimationAmount / 2, 0, RenderTextureFormat.Default);

                BlurMaterial.SetTexture("_ImageTex", RGBTexture);

                // Blur result
                BlurMaterial.EnableKeyword("BLUR_HORIZONTAL");
                Graphics.Blit(DepthMask, DepthMaskBlur, BlurMaterial);
                BlurMaterial.DisableKeyword("BLUR_HORIZONTAL");

                BlurMaterial.EnableKeyword("BLUR_VERTICAL");
                Graphics.Blit(DepthMaskBlur, DepthMask, BlurMaterial);
                BlurMaterial.DisableKeyword("BLUR_VERTICAL");

                RenderTexture.ReleaseTemporary(DepthMaskBlur);
            }
        }

        protected override void ClearTexture()
        {
            RGBTexture = null;
            DepthTexture = null;

            if (colorTex != null && colorTex.textureBinding != null)
                colorTex.textureBinding.RemoveListener(HandleColorFrameUpdated);
            if (depthTex != null && depthTex.textureBinding != null)
                depthTex.textureBinding.RemoveListener(HandleDepthFrameUpdated);
            DestroyImmediate(deviceObj);
            deviceObj = null;
            DestroyImmediate(device);
            device = null;

            base.ClearTexture();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

            if (isResettingCamera || !wasCameraConnected)
            {
                return;
            }

            if (context.Data == null || !IsVideoInputRealSense(context.Data.deviceName))
                return;

            ApplyDefaultDeviceParams();

            if (RGBTexture != null && (lastWidth != context.Data.deviceFeedWidth || lastHeight != context.Data.deviceFeedHeight || lastSerial != targetSerialNumber))
            {
                ClearTexture();
                return; //Give system a frame to shut down
            }
            if (RGBTexture == null && context.Data.deviceFeedHeight != 0 && context.Data.deviceFeedWidth != 0)
            {
                SetTexture();
            }
        }

        private void UpdateColorSensorOptions()
        {
            try
            {
                if (colorSensor != null)
                {
                    colorSensor.Options[Option.EnableAutoExposure].Value = 0;
                    colorSensor.Options[Option.EnableAutoWhiteBalance].Value = 0;
                }
            }
            catch { }
        }

        private void UpdateDepthSensorOptions()
        {
            try
            {
                if (depthSensor != null)
                {
                    depthSensor.Options[Option.EnableAutoExposure].Value = 1;
                }
            }
            catch { }
        }
        
        void ApplyDefaultDeviceParams()
        {
            if (context.Data == null)
                return;

            if (context.Data.deviceData.exposure < 0)
                context.Data.deviceData.exposure = DEFAULT_EXPOSURE_VAL;
            if (context.Data.deviceData.gain < 0)
                context.Data.deviceData.gain = DEFAULT_GAIN_VAL;
            if (context.Data.deviceData.whiteBalance < 0)
                context.Data.deviceData.whiteBalance = DEFAULT_WHITEBALANCE_VAL;
            if (context.Data.deviceData.infraredExposure < 0)
                context.Data.deviceData.infraredExposure = DEFAULT_INFRARED_EXPOSURE_VAL;
        }

        IEnumerator ReinitLibAVCoroutine()
        {
            yield return new WaitForEndOfFrame();

            if (context == null || context.Data == null || context.Data.deviceData == null)
            {
                yield break;
            }

            RealSenseUtility.SetOption(colorSensor, Option.Exposure, context.Data.deviceData.exposure);
            RealSenseUtility.SetOption(colorSensor, Option.Gain, context.Data.deviceData.gain);
            RealSenseUtility.SetOption(colorSensor, Option.WhiteBalance, context.Data.deviceData.whiteBalance);
            RealSenseUtility.SetOption(depthSensor, Option.Exposure, context.Data.deviceData.infraredExposure);
        }

        void ReinitializeLibAVSettings()
        {
            StartCoroutine(ReinitLibAVCoroutine());
        }

        /// <summary>
        /// Chooses the most appropriate depth resolution for the given RGB camera resolution.
        /// Some resolutions supported by the RGB camera, e.g. 960x540, are unsupported by the stereo module,
        /// in which case we need to choose the next best option.
        /// </summary>
        static FeedDeviceManager.OutputInfo FindBestDepthOutput(int width, int height)
        {
            if (FeedDeviceManager.realSenseDepthOutputs.Count == 0)
            {
                return null;
            }

            var aspectRatio = (float)width / height;

            try
            {
                FeedDeviceManager.OutputInfo info = FeedDeviceManager.realSenseDepthOutputs.Find(output =>
                    output.width <= width &&
                    output.height <= height &&
                    output.MatchesAspectRatio(aspectRatio)
                );
                if (info == null)
                    throw new KeyNotFoundException("Couldn't find a matching depth output for the RGB output");
                return info;
            }
            catch
            {
                return null;
            }
        }
    }
}
#endif
