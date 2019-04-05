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
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using System.Linq;
using Intel.RealSense;
using System.Collections;


namespace BlueprintReality.MixCast.RealSense
{
    /// <summary>
    /// Manages streaming using a RealSense Device
    /// </summary>
    [HelpURL("https://github.com/IntelRealSense/librealsense/tree/master/wrappers/unity")]
    public class RsDevice : RsFrameProvider
    {
        /// <summary>
        /// The parallelism mode of the module
        /// </summary>
        public enum ProcessMode
        {
            Multithread,
            UnityThread,
        }

        /// <summary>
        /// Threading mode of operation, Multithread or UnityThread
        /// </summary>
        [Tooltip("Threading mode of operation, Multithreads or Unitythread")]
        public ProcessMode processMode;

        /// <summary>
        /// Notifies upon streaming start
        /// </summary>
        public override event Action<PipelineProfile> OnStart;

        /// <summary>
        /// Notifies when streaming has stopped
        /// </summary>
        public override event Action OnStop;

        /// <summary>
        /// Fired when a new frame is available
        /// </summary>
        public override event Action<Frame> OnNewSample;

        /// <summary>
        /// User configuration
        /// </summary>
        public RsConfiguration DeviceConfiguration = new RsConfiguration
        {
            mode = RsConfiguration.Mode.Live,
            RequestedSerialNumber = string.Empty,
            Profiles = new RsVideoStreamRequest[]
            {
            new RsVideoStreamRequest {Stream = Stream.Depth, StreamIndex = -1, Width = 640, Height = 480, Format = Format.Z16 , Framerate = 30 },
            new RsVideoStreamRequest {Stream = Stream.Infrared, StreamIndex = -1, Width = 640, Height = 480, Format = Format.Y8 , Framerate = 30 },
            new RsVideoStreamRequest {Stream = Stream.Color, StreamIndex = -1, Width = 640, Height = 480, Format = Format.Rgb8 , Framerate = 30 }
            }
        };

        private Thread worker;
        private readonly AutoResetEvent stopEvent = new AutoResetEvent(false);
        private Intel.RealSense.Pipeline m_pipeline;
        
        void OnEnable()
        {
            m_pipeline = new Intel.RealSense.Pipeline();

            using (var cfg = DeviceConfiguration.ToPipelineConfig())
            {
                ActiveProfile = m_pipeline.Start(cfg);
            }

            using (var activeStreams = ActiveProfile.Streams)
            {
                List<RsVideoStreamRequest> activeStreamsList = new List<RsVideoStreamRequest>();
                foreach(var _profile in activeStreams)
                {
                    var _rsvs = RsVideoStreamRequest.FromProfile(_profile);
                    activeStreamsList.Add(_rsvs);
                }
                DeviceConfiguration.Profiles = activeStreamsList.ToArray();
            }

            if (processMode == ProcessMode.Multithread)
            {
                stopEvent.Reset();
                worker = new Thread(WaitForFrames);
                worker.IsBackground = true;
                worker.Start();
            }

            StartCoroutine(WaitAndStart());
        }

        IEnumerator WaitAndStart()
        {
            yield return new WaitForEndOfFrame();
            Streaming = true;
            if (OnStart != null)
                OnStart(ActiveProfile);
        }

        void OnDisable()
        {
            OnNewSample = null;
            // OnNewSampleSet = null;

            if (worker != null)
            {
                stopEvent.Set();
                worker.Join();
            }

            if (Streaming && OnStop != null)
                OnStop();

            if (m_pipeline != null)
            {
                if (Streaming)
                    m_pipeline.Stop();
                m_pipeline.Release();
                m_pipeline = null;
            }

            Streaming = false;

            if (ActiveProfile != null)
            {
                ActiveProfile.Dispose();
                ActiveProfile = null;
            }
        }

        void OnDestroy()
        {
            // OnStart = null;
            OnStop = null;

            if (m_pipeline != null)
                m_pipeline.Release();
            m_pipeline = null;

            // Instance = null;
        }

        private void RaiseSampleEvent(Frame frame)
        {
            var onNewSample = OnNewSample;
            if (onNewSample != null)
            {
                onNewSample(frame);
            }
        }

        /// <summary>
        /// Worker Thread for multithreaded operations
        /// </summary>
        private void WaitForFrames()
        {
            while (!stopEvent.WaitOne(0))
            {
                using (var frames = m_pipeline.WaitForFrames())
                using (var frame = frames.AsFrame())
                    RaiseSampleEvent(frame);
            }
        }

        void Update()
        {
            if (!Streaming)
                return;

            if (processMode != ProcessMode.UnityThread)
                return;

            FrameSet frames;
            if (m_pipeline.PollForFrames(out frames))
            {
                using (frames)
                using (var frame = frames.AsFrame())
                {
                    RaiseSampleEvent(frame);
                }
            }
        }
    }
}
#endif
