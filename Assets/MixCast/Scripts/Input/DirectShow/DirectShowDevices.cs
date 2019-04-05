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
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using System.Threading;

namespace BlueprintReality.MixCast
{
	public class DirectShowDevices
	{
		#region Class Types
		[StructLayout(LayoutKind.Sequential)]
		public struct VideoStreamParamsEx
		{
			public uint width;
			public uint height;
			public uint fpsNum;
			public uint fpsDen;
			public uint bitrate;
			public uint numPlanes;
			public uint bitsPerPixel;
			public int pixelFormat;
			public FieldScan fieldScan;
			public ColorSpace colorSpace;
			public ConnectionType connectionType; // 0 = unset (default)
			public EDEVICETYPE deviceType; //DirectShow (default)
			public Boolean isDefaultStream;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct AudioStreamParams
		{
			public uint nChannels;       // number of channels (i.e. mono, stereo...)
			public uint nSamplesPerSec;  // sample rate 
			public uint nAvgBytesPerSec; // for buffer estimation 
			public uint nBlockAlign;     // block size of data 
			public uint wBitsPerSample;  // number of bits per sample of mono data 
		}

		public enum EDEVICETYPE
		{
			UnknownCapture = 0x0000,

			//video
			VideoCapture = 0x0001,          //VideoCapture uses standard video capture category 
			DeckLinkVideoCapture = 0x0002,  //DeckLinkVideoCapture uses DeckLink SDK and its devices
			ElGatoVideoCapture = 0x0004,    //ElGatoVideoCapture uses ElGato Guids, separate from other categories
			HappaugeVideoCapture = 0x0008, //TODO
			SensorsVideoCapture = 0x0010, //TODO
			FutureVideoCaptureA = 0x0020, //TODO
			FutureVideoCaptureB = 0x0040, //TOOD
			FutureVideoCaptureC = 0x0080, //TODO
			AllVideoCapture = 0x00FF,

			//audio
			AudioCapture = 0x0100, //AudioCapture uses the same
			AudioCaptureIMMDevice = 0x0200, //uses IMMDeviceEnumerator
			FutureAudioCaptureA = 0x0400, //TODO
			FutureAudioCaptureB = 0x0800, //TODO
			AllAudioCapture = 0x0F00,

			//flag to ignore streams info and to specify which name to capture
			CaptureNamesOnly = 0x1000, //captures both alternative name (hw name) and friendly name
			AllCaptureCategories = 0xF000,

			//All
			AllDevicesCapture = 0xFFFF,
		};

		public enum FieldScan
		{
			unset = -1,
			progressive = 0,
			interlacedTopFirst = 1,
			interlacedBottomFirst = 2,
		}
		public enum ConnectionType
		{
			UNSET = 0,
			SDI = 1,
			HDMI = 2,
			OPTICAL_SDI = 4,
			COMPONENT = 8,
			COMPOSITE = 16,
			SVIDEO = 32,
		}

		public enum ColorSpace
		{
			BT709 = 0, //default (more common)
			BT601 = 1,
			T871 = 2,
		}

		public static class DeviceEnumerator
		{
			public static IntPtr enumerator = IntPtr.Zero;
			private static int refCount = 0;
			private static int getRefCount { get { return refCount; } }
			private static int usingEnumerator = 0;
			private static System.Object mutex = new System.Object();

			public static void setEnumerator()
			{
				if (enumerator == IntPtr.Zero)
				{
					enumerator = DirectShowDevices.buildDshowEnumerator();
				}
			}
			public static IntPtr GetEnumerator()
			{
				//acquire lock
				lock (mutex)
				{
					setEnumerator();

					//Release the lock
					Interlocked.Exchange(ref usingEnumerator, 0);
					return enumerator;
				}
			}
			public static void AddRef()
			{
				int startValue = getRefCount;
				int newValue = Interlocked.Increment(ref refCount); //ref++;

				//from 0 top 1, we should build the enumerator, if it wasn't already built
				if (startValue == 0 && newValue == 1)
					setEnumerator();
			}
			public static void Release()
			{
				int newValue = 0;
				if (refCount != 0)
					newValue = Interlocked.Decrement(ref refCount); //ref--;

				if (newValue == 0 && enumerator != IntPtr.Zero)
				{
					DirectShowDevices.freeDshowEnumerator(enumerator);
					enumerator = IntPtr.Zero;
				}
			}
		}

		#endregion //Class Types

		#region Class Variabless
		public const System.UInt16 STRLEN_PIXELFORMAT = 64;
		public const System.UInt16 STRLEN_DEVICENAME = 256;
		public const System.UInt16 STRLEN_DEVICEALTNAME = 1024;
		public const System.UInt16 FRAMERATE_TO_FRACTIONAL_MULTIPLIER = 1000; 
		public const EDEVICETYPE DEF_AUDIO_DEVICE_TYPE = EDEVICETYPE.AudioCaptureIMMDevice;
		public const EDEVICETYPE DEF_VIDEO_DEVICE_TYPE = EDEVICETYPE.VideoCapture;
		public const EDEVICETYPE DEF_DECKLINK_DEVICE_TYPE = EDEVICETYPE.DeckLinkVideoCapture;
		public const EDEVICETYPE DEF_VIDEO_ENUMERATE_DEVICE_TYPE = EDEVICETYPE.AllVideoCapture;
		public const EDEVICETYPE DEF_AUDIO_ENUMERATE_DEVICE_TYPE = EDEVICETYPE.AudioCaptureIMMDevice;

		public static byte[] tmpByteDeviceName = new byte[STRLEN_DEVICENAME];
		public static byte[] tmpByteDeviceAltName = new byte[STRLEN_DEVICEALTNAME];
		public static byte[] tmpByteDevicePixelFormat = new byte[STRLEN_PIXELFORMAT];

		#endregion //Class Variables

		#region Class Functions
		[DllImport("DshowDevices", EntryPoint = "buildDshowEnumerator")]
		public static extern IntPtr buildDshowEnumerator();

		[DllImport("DshowDevices", EntryPoint = "freeDshowEnumerator")]
		public static extern int freeDshowEnumerator(IntPtr enumerator);

		[DllImport("DshowDevices", EntryPoint = "buildDeviceList")]
		public static extern IntPtr buildDeviceList(EDEVICETYPE type, IntPtr enumerator);

		[DllImport("DshowDevices", EntryPoint = "freeVideoDeviceList")]
		public static extern Boolean freeVideoDeviceList(IntPtr vidList);

		[DllImport("DshowDevices", EntryPoint = "freeAudioDeviceList")]
		public static extern Boolean freeAudioDeviceList(IntPtr audList);
		
		[DllImport("DshowDevices", EntryPoint = "getNumberDevices")]
		public static extern int getNumberDevices(IntPtr devList);
		
		[DllImport("DshowDevices", CharSet = CharSet.Ansi, EntryPoint = "getDeviceNameFromIndex")]
		public static extern void getDeviceNameFromIndex(IntPtr devList, byte[] str, int sizeString, int index);

		//Use this to call the above deviceName in order to get the correct ASCII conversion
		public static void getDeviceNameFromStr(IntPtr devList, out string str, int index)
		{
			//byte[] z = new byte[STRLEN_DEVICENAME];
			if (tmpByteDeviceName == null)
				tmpByteDeviceName = new byte[STRLEN_DEVICENAME];

			getDeviceNameFromIndex(devList, tmpByteDeviceName, STRLEN_DEVICENAME, index);
 			str = Encoding.ASCII.GetString(tmpByteDeviceName).Trim('\0');
			//z = null;
		}

		[DllImport("DshowDevices", EntryPoint = "getDeviceAltNameFromIndex")]
		public static extern void getDeviceAltNameFromIndex(IntPtr devList, byte[] str, int sizeString, int index);

        public static void getDeviceAltNameFromStr( IntPtr devList, out string str, int index )
		{
            //byte[] z = new byte[STRLEN_DEVICEALTNAME];
			if (tmpByteDeviceAltName == null)
				tmpByteDeviceAltName = new byte[STRLEN_DEVICEALTNAME];

			getDeviceAltNameFromIndex( devList, tmpByteDeviceAltName, STRLEN_DEVICEALTNAME, index );
            str = Encoding.ASCII.GetString(tmpByteDeviceAltName).Trim( '\0' );
            //z = null;
        }

		[DllImport("DshowDevices", EntryPoint = "getDeviceTypeFromName")]
		public static extern EDEVICETYPE getDeviceTypeFromName(IntPtr devList, string altName);

		[DllImport("DshowDevices", EntryPoint = "getDeviceTypeFromIndex")]
		public static extern EDEVICETYPE getDeviceTypeFromIndex(IntPtr devList, int deviceIndex);


		[DllImport("DshowDevices", EntryPoint = "fieldScanToString")]
		public static extern string fieldScanToString(FieldScan fieldScan);

		[DllImport("DshowDevices", EntryPoint = "colorSpaceToString")]
		public static extern string colorSpaceToString(ColorSpace colorSpace);

		[DllImport("DshowDevices", EntryPoint = "connectionTypeToString")]
		public static extern string connectionTypeToString(ConnectionType connectionType);

		[DllImport("DshowDevices", EntryPoint = "getNumStreamParamsInDeviceFromIndex")]
		public static extern int getNumStreamParamsInDeviceFromIndex(IntPtr devList, int devIndex);

		[DllImport("DshowDevices", CallingConvention = CallingConvention.Cdecl, EntryPoint = "getAudioStreamParamsInDeviceFromIndex")]
		public static extern void getAudioStreamParamsInDeviceFromIndex(IntPtr devList, out AudioStreamParams sParams, int devIndex, int streamIndex);

		[DllImport("DshowDevices", CallingConvention = CallingConvention.Cdecl, EntryPoint = "getVideoStreamParamsInDeviceFromIndex")]
		public static extern void getVideoStreamParamsInDeviceFromIndex(IntPtr devList, out VideoStreamParamsEx sParams, int devIndex, int streamIndex);

		[DllImport("DshowDevices", EntryPoint = "avPixFmtToString")]
		public static extern void avPixFmtToString(int AvPixelFormat, byte[] pixFmtString, int sizeString);

		public static void avPixFmtToStringFromStr(int AvPixelFormat, out string str)
		{
			//byte[] z = new byte[STRLEN_PIXELFORMAT];
			if (tmpByteDevicePixelFormat == null)
				tmpByteDevicePixelFormat = new byte[STRLEN_PIXELFORMAT];

			avPixFmtToString(AvPixelFormat, tmpByteDevicePixelFormat, STRLEN_PIXELFORMAT);
			str = Encoding.ASCII.GetString(tmpByteDevicePixelFormat).Trim('\0');
			//z = null;
		}

		[DllImport("DshowDevices", EntryPoint = "fmtStringToAvPixFmt")]
		public static extern int fmtStringToAvPixFmt(string formatString);


		///@brief Checks if the device altname is within the list of devices on the system based on deviceType.
		/// example: int ret = buildDeviceNamesFind(DEVICETYPE.VideoCaptureDevice, 
		///						new StringBuilder("@device_pnp_\\\\?\\root#media#0001#{65e8773d-8f56-11d0-a3b9-00a0c9223196}\\global"));
		///						
		///@return Returns 0 upon success, -1 if it could not find the device
		///@note Only these enums work, AudioCaptureIMMDevice, VideoCapture, AudioCapture, and DeckLinkVideoCapture
		[DllImport("DshowDevices", EntryPoint = "buildDeviceNamesFind", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl )]
		public static extern System.Boolean buildDeviceNamesFind(EDEVICETYPE type, IntPtr enumerator, [MarshalAs( UnmanagedType.LPStr )]string altName);


		//print functions for debug
		[DllImport("DshowDevices", EntryPoint = "printVideoDeviceList")]
		public static extern void printVideoDeviceList(IntPtr vidList);

		[DllImport("DshowDevices", EntryPoint = "printAudioDeviceList")]
		public static extern void printAudioDeviceList(IntPtr audList);

		#endregion //Class Functions
	}
}
#endif
