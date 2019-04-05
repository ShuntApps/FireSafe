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
using System.IO;
using UnityEngine;

namespace BlueprintReality.MixCast
{
    [Serializable]
    public class MixCastData
    {
        public enum OutputMode
        {
            Immediate, Buffered, Quadrant
        }

        public enum IsolationMode
        {
            None, Chromakey, StaticSubtraction, StaticDepth, EdgeCropping, DepthCutoff
        }

        public enum StreamingService
        {
            None = -1,
            Custom = 0,
            Twitch,
            YouTube,
            Mixer,
            Facebook,
            Twitter
        }

        public enum OutputResolution
        {
            Preset,
            FullScreen,
            WindowSize,
            Custom
        }

        public enum LicenseType
        {
            Free, Arcade, Creator, Indie, Educator, Streamer, eSports, Film, Pro, Enterprise
        }

        static public event EventHandler OnWriteData;

        static public void WriteData()
        {
            if (OnWriteData != null)
            {
                OnWriteData(null, null);
            }
        }

        [Serializable]
        public class GlobalData
        {
            public int targetFramerate = 30;
            public int framesPerSpareRender = 2;

            public string rootOutputPath = "";
            public bool startAutomatically = true;

            //Video streaming
            public StreamingService defaultStreamService = StreamingService.None;
            public string defaultStreamUrl = "";
            public string defaultStreamKey = "";
            public int defaultStreamBitrate = RecordingData.DEFAULT_STREAM_BITRATE;

            public Vector3 standingPosToRaw = Vector3.zero;
            public Quaternion standingRotToRaw = Quaternion.identity;
        }

        [Serializable]
        public class SecureData
        {
            public string machineId;
            public string email;
            public LicenseType licenseType = LicenseType.Free; // legacy implementation; required for backwards compatability with old SDKs
        }

        [Serializable]
        public class FileBackedTexture
        {
            private const string FOLDER_NAME = "Blueprint Reality/MixCast";

            public FileBackedTexture(string filePurpose, string cameraId)
            {
                FilePurpose = filePurpose;
                CameraId = cameraId;
            }

            public string CameraId { get; protected set; }

            public string FilePurpose { get; protected set; }

            [SerializeField]
            private string filePath;

            private string GetFilePath()
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    var programDataFolder = new DirectoryInfo(Application.persistentDataPath).Parent.Parent.FullName;
                    var folderPath = Path.Combine(programDataFolder, FOLDER_NAME);
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    filePath = Path.Combine(folderPath, FilePurpose + "_" + CameraId);
                }
                return filePath;
            }

            [SerializeField]
            private bool valueSet;
            [SerializeField]
            private int width, height;
            [SerializeField]
            private string format;

            private void WriteTexture()
            {
                try
                {
                    if (tex != null)
                    {
                        byte[] texData = tex.GetRawTextureData();

                        //FileStream stream = File.OpenWrite(filePath);
                        //stream.BeginWrite(texData, 0, texData.Length, (IAsyncResult res) =>
                        //{
                        //    stream.EndWrite(res);
                        //    stream.Close();
                        //}, this);
                        File.WriteAllBytes(GetFilePath(), texData);

                        width = tex.width;
                        height = tex.height;
                        format = tex.format.ToString();
                    }
                    else if (valueSet)
                    {
                        if (File.Exists(GetFilePath()))
                            File.Delete(GetFilePath());
                        valueSet = false;
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogException(ex);
                }
            }

            private Texture2D tex;
            public Texture2D Tex
            {
                //Load the texture from the saved file if available
                get
                {
                    if (tex == null && valueSet)
                    {
                        if (!File.Exists(GetFilePath()))
                        {
                            valueSet = false;
                            return null;
                        }

                        TextureFormat formatEnum;
                        if (string.IsNullOrEmpty(format))
                            formatEnum = TextureFormat.RGB24;
                        else
                            formatEnum = (TextureFormat)System.Enum.Parse(typeof(TextureFormat), format);
                        tex = new Texture2D(width, height, formatEnum, false, QualitySettings.activeColorSpace.Equals(ColorSpace.Linear));
                        byte[] mapBytes = File.ReadAllBytes(GetFilePath());
                        tex.LoadRawTextureData(mapBytes);
                        tex.wrapMode = TextureWrapMode.Clamp;
                        tex.filterMode = FilterMode.Point;
                        tex.Apply();
                    }
                    return tex;
                }
                set
                {
                    tex = value;

                    WriteTexture();

                    valueSet = tex != null;
                }
            }

            public void ReadPixels(Rect sourceRect, int destX, int destY)
            {
                Tex.ReadPixels(sourceRect, destX, destY);
            }

            public void Apply()
            {
                Tex.Apply();
                WriteTexture();
            }
        }

        [Serializable]
        public class AudioCalibrationData
        {
            //Audio device data
            public string audioDeviceName;
            public string audioDeviceAltName;
            public uint channel;
            public uint samplingRate;
            public uint blockAlign;
            public uint bitsPerSample;
            public uint avgBytesPerSec;
            public uint desktopDelayMs;

            public AudioInputDeviceData defaultSettings = new AudioInputDeviceData();
        }

        [Serializable]
        public class AudioInputDeviceData
        {
            public MixCastAV.AUDIOCONFIG audioConfig
            {
                get { return MixCastAV.GetConfigurationFromBools(useAudioInput, useDesktopAudio); }
                set
                {
                    useAudioInput = ((MixCastAV.AUDIOCONFIG.MICROPHONE_ONLY & value) > 0);
                    useDesktopAudio = ((MixCastAV.AUDIOCONFIG.DESKTOP_ONLY & value) > 0);
                }
            }

            public string audioAltName;
            public string audioName;

            public bool useDesktopAudio = true;  //MXC-463
            public bool useAudioInput = false;
            public bool monitorOutput = false; //future

            public static readonly float DEFAULT_MIC_VOLUME = 0.5f;
            public static readonly float DEFAULT_DESKTOP_VOLUME = 1.0f;

            public float volume = DEFAULT_MIC_VOLUME;
            public int quality = -1;
            public float desktopVolume = DEFAULT_DESKTOP_VOLUME;
            public int delayMs = 0;
        }

        [Serializable]
        public class CameraCalibrationData
        {
            public CameraCalibrationData()
            {
                id = System.Guid.NewGuid().ToString();
                staticSubtractionData = new StaticKeyCalibrationData(id);
                staticDepthData = new StaticDepthCalibrationData(id);
            }

            public CameraCalibrationData(string identifier)
            {
                id = identifier;
                staticSubtractionData = new StaticKeyCalibrationData(id);
                staticDepthData = new StaticDepthCalibrationData(id);
            }

            //Camera parameters
            public string displayName;
            public const float DEFAULT_DEVICE_FOV = 45;
            public float deviceFoV = DEFAULT_DEVICE_FOV;
            public float storedFoV = DEFAULT_DEVICE_FOV;

            //Imaging device data
            public string deviceName; //friendlyName
            public string deviceAltName; //alternativeName
            public uint deviceFeedWidth, deviceFeedHeight;
            public string devicePixelFormat;
            public int devicePixelFormatInt = -1;
            public uint deviceFramerate;
            public uint deviceFramerateNum;
            public uint deviceFramerateDen;
            public DirectShowDevices.EDEVICETYPE deviceType;
            public DirectShowDevices.FieldScan deviceFieldScan;
            public DirectShowDevices.ColorSpace deviceColorSpace;
            public DirectShowDevices.ConnectionType deviceConnectionType;
            public DirectShowDevices.ColorSpace outputColorSpace;
            public int outputFramerate;
            public uint outputFramerateNum;
            public uint outputFramerateDen;
            public bool unplugged;

            public InputDeviceData deviceData = new InputDeviceData();
            //Placement data
            public Vector3 worldPosition = Vector3.zero;
            public Quaternion worldRotation = Quaternion.identity;

            public bool wasTracked;
            public string trackedByDevice;
            public string trackedByDeviceId;
            public string trackedByDeviceRole;
            public Vector3 trackedPosition = Vector3.zero;
            public Quaternion trackedRotation = Quaternion.identity;

            //Actor tracking data
            public bool actorWasTracked;
            public string actorTrackingDeviceId;

            //Motion data
            public float positionSmoothTime;
            public float rotationSmoothTime;

            // max width and height currently set to 4k, need to change code for warning text if these are adjusted
            public const int MAX_WIDTH = 3840;
            public const int MAX_HEIGHT = 2160;

            public const int MIN_WIDTH = 100;
            public const int MIN_HEIGHT = 100;

            public const int MAX_WIDTH_FREE = 1280;
            public const int MAX_HEIGHT_FREE = 720;

            public const int REALSENSE_DEPTH_WIDTH = 1280;
            public const int REALSENSE_DEPTH_HEIGHT = 720;

            //Output data
            public int outputWidth, outputHeight;
            public OutputMode outputMode = OutputMode.Buffered;
            public OutputResolution outputResolution = OutputResolution.WindowSize;

            //For buffered camera mode
            public const float DEFAULT_BUFFER_DELAY = 150f; //milliseconds
            public float bufferTime = DEFAULT_BUFFER_DELAY;
            public float maxBufferTime = 5000f;

            public bool isTransformConfigured = false;

            //Background removal data
            public SubjectBoxingData croppingData = new SubjectBoxingData();
            public EdgeCroppingData edgeCroppingData = new EdgeCroppingData();

            public IsolationMode isolationMode = IsolationMode.None;
            public ChromakeyCalibrationData chromakeying = new ChromakeyCalibrationData();
            public StaticKeyCalibrationData staticSubtractionData;
            public StaticDepthCalibrationData staticDepthData;
            public DepthCutOffData depthCutoffData = new DepthCutOffData();

            public const float DEFAULT_BG_REMOVAL_TOLERANCE = 1.0f;
            public float backgroundRemovalTolerance = DEFAULT_BG_REMOVAL_TOLERANCE;

            public const float DEFAULT_BG_REMOVAL_FADERANGE = 0.0f;
            public float backgroundRemovalFadeRange = DEFAULT_BG_REMOVAL_FADERANGE;

            public const float DEFAULT_BG_REMOVAL_FALLBACKALPHA = 1f;
            public float backgroundRemovalFallbackAlpha = DEFAULT_BG_REMOVAL_FALLBACKALPHA;

            public const bool DEFAULT_BG_REMOVAL_AA_ENABLED = true;
            public bool antiAliasSegmentationResults = DEFAULT_BG_REMOVAL_AA_ENABLED;

            public bool subjectHasPixelDepth = false;

            //Projection parameters
            public ProjectionData projectionData = new ProjectionData();

            //InScene display data
            public SceneDisplayData displayData = new SceneDisplayData();

            public LightingData lightingData = new LightingData();

            public RecordingData recordingData = new RecordingData();

            public AudioInputDeviceData audioData = new AudioInputDeviceData();

            public void StoreIsolationModeByWeight()
            {
                if (staticSubtractionData.active && staticSubtractionData.calibrated)
                    isolationMode = IsolationMode.StaticSubtraction;
                else if (chromakeying.active && chromakeying.calibrated)
                    isolationMode = IsolationMode.Chromakey;
                else
                    isolationMode = IsolationMode.None;
            }

            public bool isInUseElsewhere { get; set; }

            public string id;
            public bool deviceUseAutoFoV = false;
        }

        [Serializable]
        public class InputDeviceData
        {
            //Not all fields guaranteed to work as expected
            public float gain = -1;
            public float exposure = -1;
            public float infraredExposure = -1;
            public float whiteBalance = -1;

            //realsense post processing
            public bool enableDecimationFilter = true;
            public int decimationMagnitude = 2;
            public bool enableSpatialFilter = true;
            public int spatialMagnitude = 3;
            public float spatialSmoothAlpha = 0.5f;
            public int spatialSmoothDelta = 18;
            public RealSense.RsSpatialFilter.HoleFillingMode spatialHoleFillingMode = RealSense.RsSpatialFilter.HoleFillingMode.HoleFill2PixelRadius;
            public bool enableTemporalFilter = true;
            public float temporalSmoothAlpha = 0.6f;
            public int temporalSmoothDelta = 30;
            public int temporalPersistence = 3;

            public bool flipX = false;
            public bool flipY = false;
        }

        [Serializable]
        public class ChromakeyCalibrationData
        {
            public bool active = false;
            public bool calibrated = false;

            public Vector3 keyHsvMid = Vector3.one * 0.01f;
            public Vector3 keyHsvRange = Vector3.one * 0.01f;

            public const float DEFAULT_KEYHSVFEATHERING = 0.01f;
            public Vector3 keyHsvFeathering = Vector3.one * 0.2f;

            public const float DEFAULT_DESATURATION_BANDWIDTH = 0.0f;
            public float keyDesaturationBandWidth = DEFAULT_DESATURATION_BANDWIDTH;
            public const float DEFAULT_DESATURATION_FALLOFFWIDTH = 0.0f;
            public float keyDesaturationFalloffWidth = DEFAULT_DESATURATION_FALLOFFWIDTH;
        }

        [Serializable]
        public class SubjectBoxingData
        {
            public bool active = false;

            public const float DEFAULT_HEADRADIUS = 0.25f;
            public float headRadius = 0.25f;
            public const float DEFAULT_HANDRADIUS = 0.25f;
            public float handRadius = 0.25f;
            public const float DEFAULT_BASERADIUS = 0.25f;
            public float baseRadius = 0.25f;
        }
        [Serializable]
        public class EdgeCroppingData
        {
            public bool active = false;

            public float leftPercent = 0;
            public float rightPercent = 0;
            public float topPercent = 0;
            public float bottomPercent = 0;
        }

        [Serializable]
        public class StaticKeyCalibrationData
        {
            public StaticKeyCalibrationData(string cameraId)
            {
                midValueTexture = new FileBackedTexture("static_mid", cameraId);
                rangeValueTexture = new FileBackedTexture("static_range", cameraId);
                cielabTexture = new FileBackedTexture("static_cielab", cameraId);
            }

            public bool active = false;
            public bool calibrated = false;
            public bool hsvcalibrated = false;

            public FileBackedTexture midValueTexture;
            public FileBackedTexture rangeValueTexture;
            public FileBackedTexture cielabTexture;

            public const float DEFAULT_KEYHSVFEATHERING = 0.01f;
            public Vector3 keyHsvFeathering = Vector3.one * DEFAULT_KEYHSVFEATHERING;

            public const float DEFAULT_COLOR_TOLERANCE = 0.0f;
            public const float DEFAULT_DISTANCE_TOLERANCE = 0.125f;
            public Vector4 keyTolerance = new Vector4(DEFAULT_COLOR_TOLERANCE, DEFAULT_COLOR_TOLERANCE, DEFAULT_COLOR_TOLERANCE, DEFAULT_DISTANCE_TOLERANCE);
            public const float DEFAULT_RANGE = 1.0f;
            public float keyRange = DEFAULT_RANGE;
        }

        [Serializable]
        public class StaticDepthCalibrationData
        {
            public StaticDepthCalibrationData(string cameraId)
            {
                depthValueTexture = new FileBackedTexture("static_depth", cameraId);
            }

            public bool active = false;
            public bool calibrated = false;

            public const float DEFAULT_OUTLINE_SIZE = 1;
            public float outlineSize = DEFAULT_OUTLINE_SIZE;

            public const float DEFAULT_OUTLINE_FADE = 1;
            public float outlineFade = DEFAULT_OUTLINE_FADE;

            public float maxDepth = DepthCutOffData.DEFAULT_MAX_DEPTH;

            public bool blurEffect = true;

            public FileBackedTexture depthValueTexture;
        }

        [Serializable]
        public class DepthCutOffData
        {
            public bool active = false;

            public const float DEFAULT_MIN_DEPTH = 0;
            public float minDepth = DEFAULT_MIN_DEPTH;

            public const float DEFAULT_MAX_DEPTH = 10;
            public float maxDepth = DEFAULT_MAX_DEPTH;
        }

        [Serializable]
        public class ProjectionData
        {
            public bool displayToHeadset = false;
            public bool displayToOtherCams = false;
        }

        [Serializable]
        public class SceneDisplayData
        {
            public enum PlacementMode
            {
                Camera, World, Headset
            }
            public PlacementMode mode = PlacementMode.Camera;

            public Vector3 position;
            public Quaternion rotation;

            public const float MAX_SCALE = 1.5f, MIN_SCALE = 0.25f;
            public float scale = 1f;
            public float alpha = 1f;
        }

        [Serializable]
        public class LightingData
        {
            public const float DEFAULT_EFFECT_AMOUNT = 0f;
            public const float DEFAULT_BASE_LIGHTING = 0.5f;
            public const float DEFAULT_POWER_MULTIPLIER = 1f;
            public const float DEFAULT_DIR_FACTOR = 1f;
            public const float DEFAULT_DIR_ADJUST = 0.5f;

            // Toggle to easily turn on/off ligting while preserving set values
            public bool isEnabled = DEFAULT_EFFECT_AMOUNT > 0;
            //Factor lerps from no lighting (0) to full lighting (1)
            public float effectAmount = DEFAULT_EFFECT_AMOUNT;
            //Adds a constant value to lighting to set a baseline amount of light
            public float baseLighting = DEFAULT_BASE_LIGHTING;
            //Multiplies the final lighting power
            public float powerMultiplier = DEFAULT_POWER_MULTIPLIER;

            //Determines how much directional attenuation applies
            public float lightingByDirection = DEFAULT_DIR_FACTOR;
            //Allows the user to customize at which slope lighting falls off
            public float dirLightingEnd = DEFAULT_DIR_ADJUST;
        }

        [Serializable]
        public class RecordingData
        {
            //Auto snapshots (times in seconds)
            public const int MIN_TIMELAPSE_EXP_START_BUFFER = 1;
            public const int MAX_TIMELAPSE_EXP_START_BUFFER = 600;
            public const int MIN_TIMELAPSE_INTERVAL = 1;
            public const int MAX_TIMELAPSE_INTERVAL = 7200;

            public bool autoStartTimelapse = false;
            public float timelapseExpStartBuffer = 120;
            public float timelapseInterval = 180;

            //Video recording
            public const int DEFAULT_RECORDING_BITRATE = 16000;
            public const int MAX_RECORDING_BITRATE = 128000;
            public const int MIN_RECORDING_BITRATE = 4000;
            public const int MIN_RECORDING_START_BUFFER = 1;
            public const int MAX_RECORDING_START_BUFFER = 600;

            public bool autoStartRecording = false; // this should be set to true for Arcade licenses by Service (through ServiceLicenseManager and MixCastDataUpgrader)
            public float recordingStartBuffer = 180;
            public float recordingInterval = 360;
            public int bitrateFileRecording = DEFAULT_RECORDING_BITRATE;

            //Video streaming
            public const int DEFAULT_STREAM_BITRATE = 2400;
            public const int MAX_STREAM_BITRATE = 32000;
            public const int MIN_STREAM_BITRATE = 800;
            public bool autoStartStreaming = false;
            public bool useSharedStreamingSettings = true;
            public int perCamStreamBitrate = DEFAULT_STREAM_BITRATE;
            public StreamingService perCamStreamService = StreamingService.None;
            public string perCamStreamUrl = "";
            public string perCamStreamKey = "";
        }

        [Serializable]
        public class SensorPose
        {
            public Vector3 position = Vector3.zero;
            public Quaternion rotation = Quaternion.identity;
        }

        public GlobalData global = new GlobalData();
        public List<CameraCalibrationData> cameras = new List<CameraCalibrationData>();
        public List<AudioCalibrationData> audioDevices = new List<AudioCalibrationData>();
        public AudioInputDeviceData desktopAudioDevice; // "what you hear" - not enumerated.
        public SceneDisplayData sceneDisplay = new SceneDisplayData();

        public string sensorId;
        public SensorPose sensorPose = new SensorPose();

        public Vector3 cameraStartPosition;
        public Quaternion cameraStartRotation;

        public string sourceVersion = "";

        public string language = "";
        public string persistentDataPath;

        public void PrepareDataToSave()
        {
            foreach (var cam in MixCast.Settings.cameras)
            {
                cam.StoreIsolationModeByWeight();
            }
        }

        public CameraCalibrationData GetCameraByID(string id)
        {
            for (int i = 0; i < cameras.Count; i++)
                if (cameras[i] != null && cameras[i].id == id)
                    return cameras[i];
            return null;
        }
    }
}
#endif
