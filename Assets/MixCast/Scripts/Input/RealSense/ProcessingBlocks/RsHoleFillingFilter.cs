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
    [ProcessingBlockDataAttribute(typeof(HoleFillingFilter))]
    [HelpURL("https://github.com/IntelRealSense/librealsense/blob/master/doc/post-processing-filters.md#hole-filling-filter")]
    public class RsHoleFillingFilter : RsProcessingBlock
    {
        /// <summary>	
        /// Control the data that will be used to fill the invalid pixels	    public enum HoleFillingMode
        /// [0-2] enumerated:	
        /// fill_from_left - Use the value from the left neighbor pixel to fill the hole	
        /// farest_from_around - Use the value from the neighboring pixel which is furthest away from the sensor	
        /// nearest_from_around - - Use the value from the neighboring pixel closest to the sensor	
        /// </summary>
        public enum HoleFillingMode
        {
            FillFromLeft = 0,
            FarestFromAround = 1,
            NearestFromAround = 2,
        }

        public HoleFillingMode _holesFill = HoleFillingMode.FillFromLeft;

        private HoleFillingFilter _pb;
        private IOption holeFillOption;

        public void Init()
        {
            _pb = new HoleFillingFilter();
            holeFillOption = _pb.Options[Option.HolesFill];
        }

        void OnDisable()
        {
            if (_pb != null)
                _pb.Dispose();
        }

        public override Frame Process(Frame frame, FrameSource frameSource)
        {
            if (_pb == null)
            {
                Init();
            }

            UpdateOptions();

            return _pb.Process(frame);
        }

        public void SetHoleFill(float val)
        {
            _holesFill = (HoleFillingMode)val;
        }

        private void UpdateOptions()
        {
            holeFillOption.Value = (float)_holesFill;
        }
    }
}
#endif
