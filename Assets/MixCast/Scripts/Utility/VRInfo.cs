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
using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;
#if UNITY_2017_2_OR_NEWER
using UnityEngine.XR;
#else
using UnityEngine.VR;
#endif
#if MIXCAST_STEAMVR
using Valve.VR;
#endif

namespace BlueprintReality.MixCast
{
    public class VRInfo : MarshalByRefObject
    {
        private class VRInfoBehaviour : MonoBehaviour
        {
            public static VRInfo instance = null;

#if !UNITY_PROTECTION_PROXY
            public static bool destroyed = false;
#endif

            private void Awake()
            {
                instance = new VRInfo();
#if !UNITY_PROTECTION_PROXY
                destroyed = false;
#endif
                instance.Initialize();
            }

            private void Update()
            {
#if UNITY_2017_2_OR_NEWER
                var newState = XRSettings.enabled;
#else
                var newState = VRSettings.enabled;
#endif
                if (newState != instance.vrEnableState)
                {
                    instance.Initialize();
                }
            }

            private void OnDestroy()
            {
#if UNITY_PROTECTION_PROXY
                instance = UnityProtectionProxy.Create<VRInfo>();
#else
                instance = null;
                destroyed = true;
#endif
            }
        }

        private static VRInfo instance
        {
            get
            {
#if UNITY_PROTECTION_PROXY
                if (VRInfoBehaviour.instance == null && UnityEngine.Application.isPlaying)
#else
                if (VRInfoBehaviour.instance == null && !VRInfoBehaviour.destroyed && UnityEngine.Application.isPlaying)
#endif
                {
                    var obj = new GameObject("VRInfo");
                    obj.AddComponent<VRInfoBehaviour>();
                }
                return VRInfoBehaviour.instance;
            }
        }

        public static bool Init() { return instance != null; } // to ensure Instance can be created in the main thread.  Easy access way to do that.

        private const string OPENVR_DEVICE_NAME = "OpenVR";
        private const string OCULUS_DEVICE_NAME = "Oculus";
        private const string VIVE_MODEL_NAME = "vive";
        private const string OCULUS_MODEL_NAME = "rift";

        public static readonly Vector3 VIVE_HMD_TO_HEAD = new Vector3(0, 0, -0.075f);
        public static readonly Vector3 OCULUS_RIFT_HMD_TO_HEAD = new Vector3(0, 0, -0.075f);

        public static string loadedDeviceNameOverride = "";

        public enum VRSystemType { Oculus, Lighthouse, Holographic, Unknown }

        private string loadedDeviceName = null;
        private string deviceModel = null;
        private string vrSystem = null;
        private VRSystemType vrSystemType = VRSystemType.Unknown;

        private bool vrEnableState = false;

        private void Initialize()
        {
#if UNITY_2017_2_OR_NEWER
            vrEnableState = XRSettings.enabled;
            loadedDeviceName = XRSettings.loadedDeviceName;
            deviceModel = XRDevice.model.ToLower();
#else
            vrEnableState = VRSettings.enabled;
            loadedDeviceName = VRSettings.loadedDeviceName;
            deviceModel = VRDevice.model.ToLower();
#endif

#if MIXCAST_STEAMVR
            if ((!string.IsNullOrEmpty(loadedDeviceNameOverride) && loadedDeviceNameOverride == OPENVR_DEVICE_NAME) || loadedDeviceName == OPENVR_DEVICE_NAME)
            {
                if (SteamVR.enabled)
                {
                    vrSystem = SteamVR.instance.hmd_TrackingSystemName;

                    if (!string.IsNullOrEmpty(vrSystem))
                    {
                        for (int i = 0; i < (int)VRSystemType.Unknown; i++)
                        {
                            VRSystemType sysType = (VRSystemType)i;
                            if (sysType.ToString().ToLower() == vrSystem.ToLower())
                            {
                                vrSystemType = sysType;
                                break;
                            }
                        }
                    }
                    else
                        vrSystemType = VRSystemType.Unknown;
                }
                else
                {
#if !UNITY_EDITOR
                    Debug.LogError("OpenVR init failed");   // SteamVR.instance is hardcoded to be null when in editor
#endif
                }
            }
#endif

            if (loadedDeviceName == OCULUS_DEVICE_NAME)
            {
                vrSystemType = VRSystemType.Oculus;
            }
        }
#if MIXCAST_STEAMVR
        private Dictionary<uint, uint> serialCapacity = new Dictionary<uint, uint>();
        private Dictionary<uint, StringBuilder> serialBuilder = new Dictionary<uint, StringBuilder>();
#endif
        public static string GetDeviceSerial(uint deviceIndex)
        {
#if MIXCAST_STEAMVR
            if (!IsDeviceOpenVR())
            {
                return deviceIndex.ToString();
            }

            var error = Valve.VR.ETrackedPropertyError.TrackedProp_Success;
            var capacity = Valve.VR.OpenVR.System.GetStringTrackedDeviceProperty(deviceIndex, Valve.VR.ETrackedDeviceProperty.Prop_SerialNumber_String, null, 0, ref error);
            StringBuilder result = null;
            if (capacity > 1)
            {
                if (instance.serialCapacity.ContainsKey(deviceIndex))
                {
                    if (instance.serialCapacity[deviceIndex] == capacity)
                    {
                        result = instance.serialBuilder[deviceIndex];
                    }
                    else
                    {
                        instance.serialCapacity.Remove(deviceIndex);
                        instance.serialBuilder.Remove(deviceIndex);

                        result = new StringBuilder((int)capacity);

                        instance.serialCapacity.Add(deviceIndex, capacity);
                        instance.serialBuilder.Add(deviceIndex, result);
                    }
                }
                else
                {
                    result = new StringBuilder((int)capacity);
                    instance.serialCapacity.Remove(deviceIndex); /// no harm if it isn't in there. Solves a should-not-happen-wtf issue of duplicate keys.
                    instance.serialBuilder.Remove(deviceIndex);

                    instance.serialCapacity.Add(deviceIndex, capacity);
                    instance.serialBuilder.Add(deviceIndex, result);
                }

                Valve.VR.OpenVR.System.GetStringTrackedDeviceProperty(deviceIndex, Valve.VR.ETrackedDeviceProperty.Prop_SerialNumber_String, result, capacity, ref error);
                return result.ToString();
            }
            return deviceIndex.ToString();
#else
            // TODO: Find a way how to get Device Serial Number from Oculus SDK
            return deviceIndex.ToString();
#endif
        }

        public static bool IsDeviceOculus()
        {
            if (!string.IsNullOrEmpty(loadedDeviceNameOverride))
                return loadedDeviceNameOverride == OCULUS_DEVICE_NAME;
            return instance.loadedDeviceName == OCULUS_DEVICE_NAME;
        }

        public static bool IsDeviceOpenVR()
        {
            if (!string.IsNullOrEmpty(loadedDeviceNameOverride))
                return loadedDeviceNameOverride == OPENVR_DEVICE_NAME;
            return instance.loadedDeviceName == OPENVR_DEVICE_NAME;
        }

        public static bool IsVRModelVive()
        {
            return instance.deviceModel != null ? instance.deviceModel.Contains(VIVE_MODEL_NAME) : false;
        }

        public static bool IsVRModelOculus()
        {
            return instance.deviceModel != null ? instance.deviceModel.Contains(OCULUS_MODEL_NAME) : false;
        }

        public static string GetVRSystem()
        {
            return instance.vrSystem;
        }

        public static bool IsVRSystemVive()
        {
            return instance.vrSystemType == VRSystemType.Lighthouse;
        }
        public static bool IsVRSystemOculus()
        {
            return instance.vrSystemType == VRSystemType.Oculus;
        }
        public static bool IsVRSystemWindowsMR()
        {
            return instance.vrSystemType == VRSystemType.Holographic;
        }

        public static VRSystemType GetVRSystemType()
        {
            return instance.vrSystemType;
        }

        public static Camera FindHMDCamera()
        {
            Camera[] allCams = Camera.allCameras;

#if MIXCAST_STEAMVR
            //Catches SteamVR 1.x.x scenario where template provides 2 cameras, both set to StereoTargetEyeMask.Both and tagged as MainCamera
            for (int i = 0; i < allCams.Length; i++)
            {
                if (allCams[i].GetComponent<SteamVR_Camera>() != null)
                    return allCams[i];
            }
#endif
            for (int i = 0; i < allCams.Length; i++)
            {
                if (!allCams[i].orthographic && allCams[i].stereoTargetEye != StereoTargetEyeMask.None)
                    return allCams[i];
            }

            if (Camera.main != null)
                return Camera.main;

            if (allCams.Length > 0)
                return allCams[0];

            return null;
        }
    }
}
#endif
