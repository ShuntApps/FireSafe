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
using UnityEngine;
using System;

namespace BlueprintReality.MixCast
{
    public class DirectShowInputFeedStream
    {
		private IntPtr vidDec = IntPtr.Zero; //decoder codec pointer
		private IntPtr cfgVidDec = IntPtr.Zero; //cfg for video pointer
		private IntPtr vidTxfDec = IntPtr.Zero; //video transform pointer
		private IntPtr vidDeinterlacer = IntPtr.Zero; //deinterlacer pointer
		private volatile int decoderInterface = -1;

        public virtual int width { get; set; }
        public virtual int height { get; set; }
        private ulong udatasize = 0;
        public ulong frameDataSize { get { return udatasize; } }
        public Texture Texture { get; protected set; }

        private volatile bool startRun = false;
        public bool getStartRun {  get { return startRun; } }


		/// <summary>
		///     WebcamFeedLibAv(StringBuilder deviceName, int w, int h, int fps, StringBuilder pixFmtStr, bool forceRGB = true)
		///     Build the decoder with the info supplied in the parameters. This is non-threaded.
		/// </summary>
		/// <param name="deviceName">The device name which supports alternative hardware name</param>
		/// <param name="friendlyName">The friendly name of the device</param>
		/// <param name="type">The device type, to differentiate with directShow devices and capture cards</param>
		/// <param name="w">The requested width of the device and also the width of the texture</param>
		/// <param name="h">The requested height of the device and also the height of the texture</param>
		/// <param name="fpsNum">The requested framerate numerator of device</param>
		/// <param name="fpsDen">The requested framerate denominator of device</param>
		/// <param name="pixFmtStr">The requested pixel format of the supported device. ie; "bgr24"</param>
		/// <param name="fieldScan">The requested fieldscan type of the stream, only used in capture cards, set to progressive otherwise</param>
		/// <param name="connectionType">The requested connectionType of the device, only useful for capture cards.</param>
		/// <param name="colorSpace">The requested colorSpace of the device, only useful for capture cards.</param>
		/// <param name="unityTextureFormat">Change the pixel format of unity's texture, don't change this unless you know what you're doing</param>
		public DirectShowInputFeedStream(string deviceName, string friendlyName, DirectShowDevices.EDEVICETYPE type,
			uint w, uint h, uint fpsNum, uint fpsDen, int pixFmt, DirectShowDevices.FieldScan fieldScan,
			DirectShowDevices.ConnectionType connectionType, DirectShowDevices.ColorSpace colorSpace, 
			string unityTextureFormat = MixCastAV.texturePixelFormat)
        {
			if (cfgVidDec != IntPtr.Zero || vidDec != IntPtr.Zero || vidTxfDec != IntPtr.Zero || w == 0 || h == 0 || fpsNum == 0 || fpsDen == 0)
				return;

			//for the texture which requires int, instead of uint
			width = System.Convert.ToInt32(w);
            height = System.Convert.ToInt32(h);
			
			//Debug.Log("getVideoDecodeCfg(): pixFmt: " + pixFmt + ", fieldScan: " + fieldScan + ", connection: " + connectionType + ", colorSpace: " + colorSpace);

			//the output parameters are mirroring the input so it doesn't do any scaling, etc.
			cfgVidDec = MixCastAV.getVideoDecodeCfg(deviceName, friendlyName, (uint)type,
			w, h, fpsNum, fpsDen, pixFmt, (uint)fieldScan, (uint)connectionType, (uint)colorSpace,
			MixCastAV.FLIP_DEVICE_IMAGE_VERTICAL);

			//cfgVidDec = MixCastAV.getVideoDecodeCfg(deviceName, friendlyName, (uint)type,
			//w, h, 30000, 1000, (int)0x42475241, 1, 2, 0,
			//MixCastAV.FLIP_DEVICE_IMAGE_VERTICAL);

			if (cfgVidDec == IntPtr.Zero)
				return;

			if (vidDec == IntPtr.Zero)
				vidDec = MixCastAV.getVideoDecodeContext(cfgVidDec);
			
			if (vidDec == IntPtr.Zero)
			{
				Texture = null;
				return;
			}
			
			vidTxfDec = MixCastAV.getVideoTransformContext(cfgVidDec);

			if (vidTxfDec != IntPtr.Zero && vidDec != IntPtr.Zero && cfgVidDec != IntPtr.Zero)
				Debug.Log("Started Device Feed");
			else
			{
				Texture = null;
				return;
			}

			//vidDeinterlacer = MixCastAV.getVideoDeinterlacer(MixCastAV.FilterMode.YADIF_3, cfgVidDec);
			//if (vidDeinterlacer == IntPtr.Zero)
			//	Debug.Log("DEBUG - the deinterlacer is not set for this format");
			
			udatasize = MixCastAV.getCfgOutputDataSize(cfgVidDec);

			//initialize the texture format
			Texture = new Texture2D(width, height, TextureFormat.RGBA32, false, true);
            Texture.wrapMode = TextureWrapMode.Clamp;

			// Initialize decoder camera thread
			decoderInterface = MixCastAV.CreateDecodeInterface(vidDec, cfgVidDec, vidTxfDec, vidDeinterlacer);
			MixCastAV.SetDecodeInterfaceTexture(decoderInterface, Texture.GetNativeTexturePtr());
        }

		~DirectShowInputFeedStream()
		{
			this.Stop();
		}

		public void RenderFrame()
		{
			if (decoderInterface != -1)
			{
				GL.IssuePluginEvent(MixCastAV.GetDecodeInterfaceRenderCallback(), decoderInterface);
			}
        }
		
        public void Stop()
        {
			if (vidTxfDec != IntPtr.Zero || vidDec != IntPtr.Zero || cfgVidDec != IntPtr.Zero)
				_killDecoder();

			//throw away the texture
			if (Texture != null)
				UnityEngine.Object.DestroyImmediate(Texture);

            Texture = null;

            startRun = false;
        }

        public void Play()
        {
            startRun = true;
            MixCastAV.StartDecodeInterface(decoderInterface);
        }

        private void _killDecoder()
        {
            bool resFreeDec = false;
			bool resFreeCfg = false;
			bool resFreeTxf = false;
			
			MixCastAV.ReleaseDecodeInterface(decoderInterface);
			decoderInterface = -1;
			System.Threading.Thread.Sleep(32); //untested amount of sleep time in ms needed to avoid race condition


			//free the data config
			if (cfgVidDec != IntPtr.Zero)
				resFreeCfg = MixCastAV.freeVideoCfg(cfgVidDec) == 0 ? true : false;
			cfgVidDec = IntPtr.Zero;

			System.Threading.Thread.Sleep(2); //untested amount of sleep time in ms needed to avoid race condition
			
			//free the decoder
			if (vidDec != IntPtr.Zero)
				resFreeDec = MixCastAV.freeVideoDecodeContext(vidDec) == 0 ? true : false;
			vidDec = IntPtr.Zero;

			System.Threading.Thread.Sleep(2); //untested amount of sleep time in ms needed to avoid race condition

			//free the transformer
			if (vidTxfDec != IntPtr.Zero)
				resFreeTxf = MixCastAV.freeVideoTransform(vidTxfDec) == 0 ? true : false;
			vidTxfDec = IntPtr.Zero;

			if (resFreeDec == false || resFreeCfg == false || resFreeTxf == false)
			{
				string errorString = "Error freeing device ";
				int cnt = 0;
				if (!resFreeDec)
				{
					errorString += "decoder";
					cnt++;
				}

				if (!resFreeCfg)
				{
					if (!resFreeTxf)
						errorString += (cnt == 1) ? ", config" : "config";
					else
						errorString += (cnt == 1) ? " and config" : "config";
					cnt++;
				}

				if (!resFreeTxf)
				{
					if (cnt == 0)
						errorString += "transform";
					else if (cnt == 1)
						errorString += " and transform";
					else if (cnt == 2)
						errorString += ", and transform";
					cnt++;
				}
				errorString += (cnt >= 2) ? " objects." : " object.";
				Debug.LogError(errorString);
			}
			
		}

        protected void CompleteFrame()
        {
        }

    }// Public Class
}//namespace
#endif
