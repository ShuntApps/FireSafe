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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace BlueprintReality.MixCast.RealSense
{
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ProcessingBlockDataAttribute : System.Attribute
    {
        // See the attribute guidelines at
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        public readonly Type blockClass;

        public ProcessingBlockDataAttribute(Type blockClass)
        {
            this.blockClass = blockClass;
        }

    }


    [Serializable]
    public class RsProcessingPipe : RsFrameProvider
    {
        public RsFrameProvider Source;
        public RsProcessingProfile profile;
        public override event Action<PipelineProfile> OnStart;
        public override event Action OnStop;
        public override event Action<Frame> OnNewSample;
        private CustomProcessingBlock _block;

        void Awake()
        {
            Source.OnStart += OnSourceStart;
            Source.OnStop += OnSourceStop;

            _block = new CustomProcessingBlock(ProcessFrame);
            _block.Start(OnFrame);
        }

        private void OnSourceStart(PipelineProfile activeProfile)
        {
            Source.OnNewSample += _block.ProcessFrame;
            ActiveProfile = activeProfile;
            Streaming = true;
            var h = OnStart;
            if (h != null)
                h.Invoke(activeProfile);
        }

        private void OnSourceStop()
        {
            if (_block != null)
                Source.OnNewSample -= _block.ProcessFrame;
            Streaming = false;
            var h = OnStop;
            if (h != null)
                h();
        }

        private void OnFrame(Frame f)
        {
            var onNewSample = OnNewSample;
            if (onNewSample != null)
                onNewSample.Invoke(f);
        }

        private void OnDestroy()
        {
            if (_block != null)
            {
                _block.Dispose();
                _block = null;
            }
        }

        internal void ProcessFrame(Frame frame, FrameSource src)
        {
            try
            {
                Frame f = frame;

                if (profile != null)
                {
                    var filters = profile.ToArray();
                    // foreach (var pb in profile)
                    foreach (var pb in filters)
                    {
                        if (pb == null || !pb.Enabled)
                            continue;

                        var r = pb.Process(f, src);
                        if (r != f)
                        {
                            if (f != frame)
                            {
                                f.Dispose();
                            }
                            f = r;
                        }
                    }
                }

                src.FrameReady(f);

                if (f != frame)
                    f.Dispose();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
#endif