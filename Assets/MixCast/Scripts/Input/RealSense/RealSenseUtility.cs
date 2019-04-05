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
    public class RealSenseUtility
    {
        // It could be expensive, do not call it very often
        public static Device GetDeviceFromAltName(string requestAltName)
        {
            Context rs_context = new Context();
            DeviceList deviceList = rs_context.QueryDevices();

            for (int i = 0; i < deviceList.Count; i++)
            {
                var rs_device = deviceList[i];
                if( rs_device != null ) {
                    var device_port = rs_device.Info[CameraInfo.PhysicalPort];
                    if( !string.IsNullOrEmpty( device_port ) ) {
                        var unique_id = device_port.Split('&')[3];
                        if( requestAltName.Contains( unique_id ) ) {
                            return rs_device;
                        }
                    }
                }
            }

            return null;
        }

        // It could be expensive, do not call it very often
        public static string GetDeviceSerialFromAltName(string requestAltName)
        {
            var request_device = GetDeviceFromAltName(requestAltName);
            if (request_device != null)
            {
                return request_device.Info[CameraInfo.SerialNumber];
            }
            return null;
        }

        public static RsDevice GetRealSenseMixCastDeviceFromAltName(string requestAltName)
        {
            foreach (var feed in InputFeed.ActiveFeeds)
            {
                if (feed is RealSenseInputFeed)
                {
                    var rs_feed = feed as RealSenseInputFeed;
                    var rs_device = rs_feed.device;
                    if (rs_device != null)
                    {
                        var profile = rs_device.ActiveProfile;
                        if(profile != null) {
                            var deviceInfo = profile.Device.Info[CameraInfo.PhysicalPort];
                            if(!string.IsNullOrEmpty(deviceInfo)) {
                                var unique_id = deviceInfo.Split('&')[3];
                                if( requestAltName.Contains( unique_id ) ) {
                                    return rs_device;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        public static void SetOption(Sensor sensor, Option option, float normalizedValue)
        {
            if (normalizedValue < 0)
            {
                return;
            }

            try
            {
                // Convert Value to expand effective data range for small value
                if (option.Equals(Option.Exposure))
                {
                    normalizedValue = normalizedValue * normalizedValue * normalizedValue;
                }
                var cameraOption = sensor.Options[option];
                var value = normalizedValue * (cameraOption.Max - cameraOption.Min) + cameraOption.Min;
                sensor.Options[option].Value = value;
            }
            catch
            {
                // should we show a toast here?
                return;
            }
        }
    }
}
#endif
