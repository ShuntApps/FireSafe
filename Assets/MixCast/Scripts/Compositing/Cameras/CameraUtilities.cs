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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueprintReality.MixCast
{
    public static class CameraUtilities
    {
        public static int CalculateCameraRenderWidth(MixCastData.CameraCalibrationData cameraConfig)
        {
            int val = GetConfiguredCameraRenderWidth(cameraConfig);
            if (FreeLicenseAffectsWidth(val))
                val = MixCastData.CameraCalibrationData.MAX_WIDTH_FREE;
            return val;
        }
        public static int GetConfiguredCameraRenderWidth(MixCastData.CameraCalibrationData cameraConfig)
        {
            switch (cameraConfig.outputResolution)
            {
                case MixCastData.OutputResolution.WindowSize:
                    return Screen.width;
                case MixCastData.OutputResolution.FullScreen:
                    return Screen.currentResolution.width;
                case MixCastData.OutputResolution.Preset:
                    return cameraConfig.outputWidth;
                case MixCastData.OutputResolution.Custom:
                    if (cameraConfig.outputWidth > 0)
                        return cameraConfig.outputWidth;
                    else if (cameraConfig.outputHeight > 0)
                        return (int)(((float)Screen.width * cameraConfig.outputHeight) / Screen.height);
                    else
                        return Screen.width;
            }
            return -1;
        }
        public static bool FreeLicenseAffectsWidth(int renderWidth)
        {
            if (MixCastSdkData.License.LicenseType == LicenseType.None || MixCastSdkData.License.LicenseType == LicenseType.Free)
                return MixCastData.CameraCalibrationData.MAX_WIDTH_FREE < renderWidth;
            else
                return false;
        }

        public static int CalculateCameraRenderHeight(MixCastData.CameraCalibrationData cameraConfig)
        {
            int val = GetConfiguredCameraRenderHeight(cameraConfig);
            if (FreeLicenseAffectsHeight(val))
                val = MixCastData.CameraCalibrationData.MAX_HEIGHT_FREE;
            return val;
        }
        public static int GetConfiguredCameraRenderHeight(MixCastData.CameraCalibrationData cameraConfig)
        {
            switch (cameraConfig.outputResolution)
            {
                case MixCastData.OutputResolution.WindowSize:
                    return Screen.height;
                case MixCastData.OutputResolution.FullScreen:
                    return Screen.currentResolution.height;
                case MixCastData.OutputResolution.Preset:
                    return cameraConfig.outputHeight;
                case MixCastData.OutputResolution.Custom:
                    if (cameraConfig.outputHeight > 0)
                        return cameraConfig.outputHeight;
                    else if (cameraConfig.outputWidth > 0)
                        return (int)(((float)Screen.height * cameraConfig.outputWidth) / Screen.width);
                    else
                        return Screen.height;
            }
            return -1;
        }
        public static bool FreeLicenseAffectsHeight(int renderHeight)
        {
            if (MixCastSdkData.License.LicenseType == LicenseType.None || MixCastSdkData.License.LicenseType == LicenseType.Free)
                return MixCastData.CameraCalibrationData.MAX_HEIGHT_FREE < renderHeight;
            else
                return false;
        }


        public static void GetCameraPose(MixCastData.CameraCalibrationData data, out Vector3 position, out Quaternion rotation)
        {
            if (data.wasTracked && GetCamerasTrackingObjectTransform(data, out position, out rotation))
            {
                position = position + rotation * data.trackedPosition;
                rotation = rotation * data.trackedRotation;
            }
            else
            {
                position = data.worldPosition;
                rotation = data.worldRotation;
            }
        }
        public static bool GetCamerasTrackingObjectTransform(MixCastData.CameraCalibrationData data, out Vector3 position, out Quaternion rotation)
        {
            if (!string.IsNullOrEmpty(data.trackedByDeviceId))
            {
                if (TrackedObjectManager.GetTransformById(data.trackedByDeviceId, out position, out rotation))
                {
                    return true;
                }
            }

            //Fall back to index if guid isn't found
            if (!string.IsNullOrEmpty(data.trackedByDeviceRole))
            {
                int roleIndex = 0;
                if (int.TryParse(data.trackedByDeviceRole, out roleIndex))
                {
                    Data.AssignedRole role = (Data.AssignedRole)roleIndex;
                    return TrackedObjectManager.GetTransformByRole(role, out position, out rotation);
                }
            }

            position = Vector3.zero;
            rotation = Quaternion.identity;
            return false;
        }

        public static bool IsRecordingOrStreaming(bool recording = true, bool streaming = true)
        {
            if (recording)
            {
                for (int i = 0; i < MixCastSdkData.Cameras.Count; i++)
                    if (MixCastSdkData.Cameras[i].VideoRecordingEnabled)
                        return true;
            }

            if (streaming)
            {
                for (int i = 0; i < MixCastSdkData.Cameras.Count; i++)
                    if (MixCastSdkData.Cameras[i].VideoStreamingEnabled)
                        return true;
            }
            //Debug.Log("is neither recording nor streaming");
            return false;
        }
    }
}
#endif
