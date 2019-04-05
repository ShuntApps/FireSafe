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
    [ProcessingBlockDataAttribute(typeof(PointCloud))]
    public class RsPointCloud : RsProcessingBlock
    {
        public enum OcclusionRemoval
        {
            Off = 0,
            Heuristic = 1,
            Exhaustive = 2
        }

        public Stream TextureStream = Stream.Color;
        public Format TextureFormat = Format.Any;
        public OcclusionRemoval _occlusionRemoval = OcclusionRemoval.Off;

        PointCloud _pb;
        private IOption filterMag;
        private IOption streamFilter;
        private IOption formatFilter;

        private readonly object _lock = new object();

        public void Init()
        {
            lock (_lock)
            {
                _pb = new PointCloud();
                filterMag = _pb.Options[Option.FilterMagnitude];
                streamFilter = _pb.Options[Option.StreamFilter];
                formatFilter = _pb.Options[Option.StreamFormatFilter];
            }
        }

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

        public override Frame Process(Frame frame, FrameSource frameSource)
        {
            lock (_lock)
            {
                if (_pb == null)
                {
                    Init();
                }
            }

            UpdateOptions(frame.IsComposite);

            return _pb.Process(frame);
        }

        private void UpdateOptions(bool isComposite)
        {
            filterMag.Value = (float)_occlusionRemoval;
            if (isComposite)
            {
                streamFilter.Value = (float)TextureStream;
                formatFilter.Value = (float)TextureFormat;
            }
        }
    }
}
#endif
