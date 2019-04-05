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
using Intel.RealSense;
using UnityEngine;

namespace BlueprintReality.MixCast.RealSense
{
    [ProcessingBlockDataAttribute(typeof(DisparityTransform))]
    public class RsDisparityTransform : RsProcessingBlock
    {
        public enum DisparityMode
        {
            DisparityToDepth = 0,
            DepthToDisparity = 1,
        }

        DisparityTransform _pb;

        [Tooltip("Stereoscopic Transformation Mode")]
        public DisparityMode Mode = DisparityMode.DepthToDisparity;

        private DisparityMode currMode;
        private readonly object _lock = new object();

        void OnDisable()
        {
            lock (_lock)
            {
                if (_pb != null)
                {
                    _pb.Dispose();
                    _pb = null;
                }
            }
        }

        public void Init()
        {
            lock (_lock)
            {
                _pb = new DisparityTransform(Mode != 0);
                currMode = Mode;
            }
        }

        public override Frame Process(Frame frame, FrameSource frameSource)
        {
            lock (_lock)
            {
                if (currMode != Mode)
                {
                    if (_pb != null)
                    {
                        _pb.Dispose();
                        _pb = null;
                    }
                }

                if (_pb == null)
                    Init();

                return _pb.Process(frame);
            }
        }
    }
}
#endif
