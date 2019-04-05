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
    [ProcessingBlockDataAttribute(typeof(SpatialFilter))]
    [HelpURL("https://github.com/IntelRealSense/librealsense/blob/master/doc/post-processing-filters.md#spatial-filter")]
    public class RsSpatialFilter : RsProcessingBlock
    {
        /// <summary>
        /// Number of filter iterations
        /// </summary>
        [Range(1, 5)]
        [Tooltip("Number of filter iterations")]
        public int _filterMagnitude = 2;

        /// <summary>
        /// The Alpha factor in an exponential moving average with Alpha=1 - no filter . Alpha = 0 - infinite filter
        /// </summary>
        [Range(0.25f, 1)]
        [Tooltip("The Alpha factor in an exponential moving average with Alpha=1 - no filter . Alpha = 0 - infinite filter")]
        public float _filterSmoothAlpha = 0.5f;

        /// <summary>
        /// Step-size boundary. Establishes the threshold used to preserve "edges"
        /// </summary>
        [Range(1, 50)]
        [Tooltip("Step-size boundary. Establishes the threshold used to preserve edges")]
        public int _filterSmoothDelta = 20;

        public enum HoleFillingMode
        {
            Disabled,
            HoleFill2PixelRadius,
            HoleFill4PixelRadius,
            HoleFill8PixelRadius,
            HoleFill16PixelRadius,
            Unlimited,
        }

        /// <summary>
        /// An in-place heuristic symmetric hole-filling mode applied horizontally during the filter passes.
        /// Intended to rectify minor artifacts with minimal performance impact
        /// </summary>
        public HoleFillingMode _holeFillingMode = 0;


        private SpatialFilter _pb;
        private IOption filterMag;
        private IOption filterAlpha;
        private IOption filterDelta;
        private IOption holeFill;

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
            _pb = new SpatialFilter();

            filterMag = _pb.Options[Option.FilterMagnitude];
            filterAlpha = _pb.Options[Option.FilterSmoothAlpha];
            filterDelta = _pb.Options[Option.FilterSmoothDelta];
            holeFill = _pb.Options[Option.HolesFill];
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

        public void SetSmoothAlpha(float val)
        {
            _filterSmoothAlpha = val;
        }

        public void SetSmoothDelta(float val)
        {
            _filterSmoothDelta = (int)val;
        }

        public void SetHolesFill(float val)
        {
            _holeFillingMode = (HoleFillingMode)val;
        }

        private void UpdateOptions()
        {
            filterMag.Value = _filterMagnitude;
            filterAlpha.Value = _filterSmoothAlpha;
            filterDelta.Value = _filterSmoothDelta;
            holeFill.Value = (float)_holeFillingMode;
        }
    }

}
#endif
