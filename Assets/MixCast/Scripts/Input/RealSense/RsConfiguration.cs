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
using Intel.RealSense;

namespace BlueprintReality.MixCast.RealSense
{    
    [Serializable]
    public struct RsConfiguration
    {
        public enum Mode
        {
            Live, Playback, Record
        }

        public Mode mode;
        public RsVideoStreamRequest[] Profiles;
        public string RequestedSerialNumber;
        public string PlaybackFile;
        public string RecordPath;


        public Config ToPipelineConfig()
        {
            Config cfg = new Config();

            switch (mode)
            {
                case Mode.Live:
                    cfg.EnableDevice(RequestedSerialNumber);
                    foreach (var p in Profiles)
                        cfg.EnableStream(p.Stream, p.StreamIndex, p.Width, p.Height, p.Format, p.Framerate);
                    break;

                case Mode.Playback:
                    if (String.IsNullOrEmpty(PlaybackFile))
                    {
                        mode = Mode.Live;
                    }
                    else
                    {
                        cfg.EnableDeviceFromFile(PlaybackFile);
                    }
                    break;

                case Mode.Record:
                    foreach (var p in Profiles)
                        cfg.EnableStream(p.Stream, p.StreamIndex, p.Width, p.Height, p.Format, p.Framerate);
                    if (!String.IsNullOrEmpty(RecordPath))
                        cfg.EnableRecordToFile(RecordPath);
                    break;

            }

            return cfg;
        }
    }
}
#endif
