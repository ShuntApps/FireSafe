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

namespace BlueprintReality.MixCast.RealSense
{
    [ProcessingBlockDataAttribute(typeof(Align))]
    public class RsAlign : RsProcessingBlock
    {
        public Stream _alignTo = Stream.Depth;

        public bool AlignOtherToDepth { set { _alignTo = Stream.Depth; } }
        public bool AlignDepthToColor { set { _alignTo = Stream.Color; } }
        public bool AlignDepthToInfrared { set { _alignTo = Stream.Infrared; } }

        private Stream _currAlignTo;
        private Align _pb;

        public void Init()
        {
            if (_pb != null)
            {
                _pb.Dispose();
            }
            _pb = new Align(_alignTo);
            _currAlignTo = _alignTo;

        }

        void OnDisable()
        {
            if (_pb != null)
                _pb.Dispose();
            _pb = null;
        }

        public void AlignTo(Stream s)
        {
            _alignTo = s;
            Init();
        }

        public override Frame Process(Frame frame, FrameSource frameSource)
        {
            if (_pb == null || _alignTo != _currAlignTo)
                Init();

            return _pb.Process(frame);
        }
    }
}
#endif
