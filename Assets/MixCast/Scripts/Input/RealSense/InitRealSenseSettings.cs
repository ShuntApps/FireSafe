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
using UnityEngine;
using Intel.RealSense;

namespace BlueprintReality.MixCast.RealSense
{
    public class InitRealSenseSettings : MonoBehaviour
    {
        [SerializeField] CameraConfigContext context;

        public RealSenseInputFeed feed { private get; set; }
        float exposure;
        float whiteBalance;
        float gain;
        float infraredExposure;

        void OnEnable()
        {
            if (context == null)
            {
                context = GetComponentInParent<CameraConfigContext>();
            }

            exposure = context.Data.deviceData.exposure;
            whiteBalance = context.Data.deviceData.whiteBalance;
            gain = context.Data.deviceData.gain;
            infraredExposure = context.Data.deviceData.infraredExposure;

            StartCoroutine(DelayedInit());
        }

        IEnumerator DelayedInit()
        {
            yield return new WaitUntil(() => (feed.device != null && feed.colorSensor != null && feed.depthSensor != null && feed.device.Streaming));

            if (context == null)
            {
                yield break;
            }
            SetOptions();
        }

        void SetOptions()
        {
            RealSenseUtility.SetOption(feed.colorSensor, Option.Exposure, exposure);
            RealSenseUtility.SetOption(feed.colorSensor, Option.Gain, gain);
            RealSenseUtility.SetOption(feed.colorSensor, Option.WhiteBalance, whiteBalance);
            RealSenseUtility.SetOption(feed.depthSensor, Option.Exposure, infraredExposure);
        }
    }
}
#endif
