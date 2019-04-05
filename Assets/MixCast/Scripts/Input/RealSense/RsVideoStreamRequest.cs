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
using Intel.RealSense;

namespace BlueprintReality.MixCast.RealSense
{
    [Serializable]
    public struct RsVideoStreamRequest : IEquatable<RsVideoStreamRequest>
    {
        public Stream Stream;
        public Format Format;
        public int Framerate;
        public int StreamIndex;
        public int Width;
        public int Height;


        public RsVideoStreamRequest(Stream stream, Format format, int framerate, int streamIndex, int width, int height)
        {
            Stream = stream;
            Format = format;
            Framerate = framerate;
            StreamIndex = streamIndex;
            Width = width;
            Height = height;
        }

        public static RsVideoStreamRequest FromFrame(VideoFrame f)
        {
            using (var p = f.Profile)
                return new RsVideoStreamRequest(
                    p.Stream,
                    p.Format,
                    p.Framerate,
                    p.Index,
                    f.Width,
                    f.Height
                );
        }


        public static RsVideoStreamRequest FromProfile(StreamProfile p)
        {
            return new RsVideoStreamRequest(
                p.Stream,
                p.Format,
                p.Framerate,
                p.Index,
                p is VideoStreamProfile ? (p as VideoStreamProfile).Width : 0,
                p is VideoStreamProfile ? (p as VideoStreamProfile).Height : 0
            );
        }

        public override bool Equals(object other)
        {
            return (other is RsVideoStreamRequest) && Equals((RsVideoStreamRequest)other);
        }

        public bool Equals(RsVideoStreamRequest other)
        {
            return
                Stream == other.Stream &&
                Format == other.Format &&
                Framerate == other.Framerate &&
                StreamIndex == other.StreamIndex &&
                Width == other.Width &&
                Height == other.Height;
        }

        public bool HasConflict(VideoFrame f)
        {
            var vf = f as VideoFrame;
            using (var p = vf.Profile)
            {
                if (Stream != Stream.Any && Stream != p.Stream)
                    return true;
                if (Format != Format.Any && Format != p.Format)
                    return true;
                if (Width != 0 && Width != vf.Width)
                    return true;
                if (Height != 0 && Height != vf.Height)
                    return true;
                if (Framerate != 0 && Framerate != p.Framerate)
                    return true;
                if (StreamIndex != 0 && StreamIndex != p.Index)
                    return true;
                return false;
            }
        }

        public bool HasConflict(RsVideoStreamRequest other)
        {
            if (Stream != Stream.Any && Stream != other.Stream)
                return true;
            if (Format != Format.Any && Format != other.Format)
                return true;
            if (Width != 0 && Width != other.Width)
                return true;
            if (Height != 0 && Height != other.Height)
                return true;
            if (Framerate != 0 && Framerate != other.Framerate)
                return true;
            if (StreamIndex != 0 && StreamIndex != other.StreamIndex)
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            // https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
            return new { Stream, Format, Framerate, StreamIndex, Width, Height }.GetHashCode();
        }
    }
}
#endif
