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
using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BlueprintReality.MixCast
{
    public class MixCastRegistry
    {
        public const string REGISTRY_PATH = @"SOFTWARE\Blueprint Reality\MixCast VR";

        public const string SETTINGS_REGISTRY_KEY = "DATA";
        public const string SECURE_REGISTRY_KEY = "SETUP";

        public static bool shouldShowRegistryMismatchWarning = false;

        public static MixCastData ReadData()
        {
            using (RegistryKey reg = Registry.CurrentUser.CreateSubKey(REGISTRY_PATH))
            {
                Assert.IsNotNull(reg);
                string dataStr = reg.GetValue(SETTINGS_REGISTRY_KEY, null) as string;

                if (string.IsNullOrEmpty(dataStr))
                    return null;

                MixCastData data = JsonUtility.FromJson<MixCastData>(dataStr);

                if (!string.IsNullOrEmpty(data.sourceVersion) && data.sourceVersion != MixCastSdk.VERSION_STRING)
                {
                    UpdateForBackwardCompatibility(data, dataStr);
                }

                return data;
            }
        }

        public static void WriteData(MixCastData data)
        {
            string dataStr = JsonUtility.ToJson(data);
            using (RegistryKey reg = Registry.CurrentUser.CreateSubKey(REGISTRY_PATH))
                reg.SetValue(MixCastRegistry.SETTINGS_REGISTRY_KEY, dataStr);
        }

        public static void ClearData()
        {
            using (RegistryKey reg = Registry.CurrentUser.CreateSubKey(REGISTRY_PATH))
                reg.DeleteValue(MixCastRegistry.SETTINGS_REGISTRY_KEY, false);
        }


        static void UpdateForBackwardCompatibility(MixCastData data, string dataStr)
        {
            AddMissingCameraInfo(data);
        }

        /// <summary>
        /// Fills in camera device fields that were added in new versions of MixCast.
        /// </summary>
        static void AddMissingCameraInfo(MixCastData data)
        {
            foreach (var camera in data.cameras)
            {
                var device = FeedDeviceManager.FindDeviceFromName(camera.deviceName);

                if (device == null)
                {
                    continue;
                }

                AddDeviceInfo(camera, device);
            }
        }

        /// <summary>
        /// Adds device info to the camera data.
        /// </summary>
        static void AddDeviceInfo(MixCastData.CameraCalibrationData camera, FeedDeviceManager.DeviceInfo device)
        {
            if (string.IsNullOrEmpty(camera.deviceAltName))
            {
                camera.deviceAltName = device.altname;
            }

            if (camera.deviceFramerateNum == 0 || camera.deviceFramerateDen == 0)
            {
                camera.deviceFramerateNum = device.outputs
                    .Select(output => output.fpsNum)
                    .OrderBy(fpsNum => fpsNum)
                    .FirstOrDefault();

                camera.deviceFramerateDen = device.outputs
                    .Select(output => output.fpsDen)
                    .OrderBy(fpsDen => fpsDen)
                    .FirstOrDefault();
            }

            if (string.IsNullOrEmpty(camera.devicePixelFormat))
            {
                camera.devicePixelFormat = device.outputs
                    .Select(output => output.pixelFormat)
                    .FirstOrDefault();
            }
        }
    }
}
#endif
