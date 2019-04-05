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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Intel.RealSense;
using UnityEngine;

namespace BlueprintReality.MixCast.RealSense
{
    [ProcessingBlockData(typeof(DepthCutoff))]
    public class DepthCutoff : RsProcessingBlock
    {
        public ushort Distance = 1000;

        ushort[] depthData;

        void OnDisable()
        {
            depthData = null;
        }

        Frame ApplyFilter(DepthFrame depth, FrameSource frameSource)
        {
            using (var p = depth.Profile)
            {
                var count = depth.Width * depth.Height;
                if (depthData == null || depthData.Length != count)
                    depthData = new ushort[count];

                depth.CopyTo(depthData);

                for (int i = 0; i < count; i++)
                {
                    if (depthData[i] > Distance)
                        depthData[i] = 0;
                }

                var v = frameSource.AllocateVideoFrame<DepthFrame>(p, depth, depth.BitsPerPixel, depth.Width, depth.Height, depth.Stride, Intel.RealSense.Extension.DepthFrame);
                v.CopyFrom(depthData);

                return v;
            }
        }

        public override Frame Process(Frame frame, FrameSource frameSource)
        {
            if (frame.IsComposite)
            {
                using (var fs = FrameSet.FromFrame(frame))
                using (var depth = fs.DepthFrame)
                {
                    var v = ApplyFilter(depth, frameSource);
                    // return v;

                    // find and remove the original depth frame
                    var frames = new List<Frame>();
                    foreach (var f in fs)
                    {
                        using (var p1 = f.Profile)
                            if (p1.Stream == Stream.Depth && p1.Format == Format.Z16)
                            {
                                f.Dispose();
                                continue;
                            }
                        frames.Add(f);
                    }
                    frames.Add(v);

                    var res = frameSource.AllocateCompositeFrame(frames);
                    frames.ForEach(f => f.Dispose());
                    using (res)
                        return res.AsFrame();
                }
            }

            if (frame is DepthFrame)
                return ApplyFilter(frame as DepthFrame, frameSource);

            return frame;
        }
    }
}
#endif
