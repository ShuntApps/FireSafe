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
    [ProcessingBlockDataAttribute(typeof(DecimationFilter))]
    [HelpURL("https://github.com/IntelRealSense/librealsense/blob/master/doc/post-processing-filters.md#decimation-filter")]
    public class RsDecimationFilter : RsProcessingBlock
    {
        public Stream _streamFilter = Stream.Depth;
        public Format _formatFilter = Format.Z16;

        /// <summary>
        /// Number of filter iterations
        /// </summary>
        [Range(2, 8)]
        [Tooltip("Number of filter iterations")]
        public int _filterMagnitude = 2;


        private DecimationFilter _pb;
        private IOption filterMag;
        private IOption streamFilter;
        private IOption formatFilter;

        public override Frame Process(Frame frame, FrameSource frameSource)
        {
            if (_pb == null)
            {
                Init();
            }

            UpdateOptions();

            return _pb.Process(frame);
        }

        public void Init()
        {
            _pb = new DecimationFilter();
            filterMag = _pb.Options[Option.FilterMagnitude];
            streamFilter = _pb.Options[Option.StreamFilter];
            formatFilter = _pb.Options[Option.StreamFormatFilter];
        }

        void OnDisable()
        {
            if (_pb != null)
            {
                _pb.Dispose();
                _pb = null;
            }
        }

        public void SetMagnitude(float val)
        {
            _filterMagnitude = (int)val;
        }

        private void UpdateOptions()
        {
            filterMag.Value = _filterMagnitude;
            streamFilter.Value = (float)_streamFilter;
            formatFilter.Value = (float)_formatFilter;
        }
    }
}
#endif
