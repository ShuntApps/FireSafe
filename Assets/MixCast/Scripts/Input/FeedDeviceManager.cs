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
using System.Linq;
using System.Text;
using System;
using UnityEngine;
using BlueprintReality.MixCast.RealSense;

namespace BlueprintReality.MixCast
{
	public static class FeedDeviceManager
	{

		[System.Serializable]
		public class OutputInfo
		{
			public uint width = 0;
			public uint height = 0;
			public uint fpsNum = 0;
			public uint fpsDen = 0;
			public uint bitrate = 0;
			public uint numPlanes = 0;
			public uint bitsPerPixel = 0;
			public string pixelFormat = "";
			public int pixelFormatInt = -1;
			public DirectShowDevices.FieldScan fieldScan = DirectShowDevices.FieldScan.progressive;
			public DirectShowDevices.ColorSpace colorSpace = DirectShowDevices.ColorSpace.BT709;
			public DirectShowDevices.ConnectionType connectionType = DirectShowDevices.ConnectionType.UNSET; // 0 = unset (default)
			public DirectShowDevices.EDEVICETYPE deviceType = DirectShowDevices.EDEVICETYPE.UnknownCapture; //DirectShow (default)
			public Boolean isDefaultStream;

			public bool MatchesAspectRatio(float aspectRatio)
			{
				var outputAspectRatio = (float)width / height;
				return Mathf.Approximately(outputAspectRatio, aspectRatio);
			}
			
		}
		[System.Serializable]
		public class DeviceInfo
		{
			public string name = "";
			public string altname = "";
			public DirectShowDevices.EDEVICETYPE deviceType = DirectShowDevices.EDEVICETYPE.UnknownCapture;
			public int deviceIndex = 0;
			public List<OutputInfo> outputs = new List<OutputInfo>();
		}

		public static List<DeviceInfo> devices = new List<DeviceInfo>();
		public static DeviceInfo FindDeviceFromAltName(string altName)
		{
			foreach (var device in devices)
			{
				if (device.altname == altName)
				{
					return device;
				}
			}
			return null;
		}
		public static DeviceInfo FindDeviceFromName(string name)
		{
			foreach (var device in devices)
			{
				if (device.name == name)
				{
					return device;
				}
			}
			return null;
		}
		public static List<OutputInfo> realSenseDepthOutputs = new List<OutputInfo>();

		static FeedDeviceManager()
		{
			BuildDeviceList();
		}
	
		private static void EnumerateDeviceList(IntPtr devCtx)
		{
			if (devCtx == IntPtr.Zero)
				return;

			//get number of devices in the list that was built in memory
			int numDevices = DirectShowDevices.getNumberDevices(devCtx);
			if (numDevices == 0)
				return;

			List<OutputInfo> checkDuplicates = new List<OutputInfo>();
			List<OutputInfo> removeTheseDuplicates = new List<OutputInfo>();
			DirectShowDevices.VideoStreamParamsEx tmpParams = new DirectShowDevices.VideoStreamParamsEx();

			//iterate through the devices
			for (int i = 0; i < numDevices; i++)
			{
				DeviceInfo devInfo = new DeviceInfo();

				//save the friendly name
				DirectShowDevices.getDeviceNameFromStr(devCtx, out devInfo.name, i);
                int terminateIndex = devInfo.name.IndexOf("\0");
                if (terminateIndex > -1)
                    devInfo.name = devInfo.name.Substring(0, terminateIndex);

				//Filter out StarGazer's other sensors "Intel(R) RealSense(TM) Camera SR300" other than the RGB 
				if (devInfo.name.ToLower().Contains("sr300") && !devInfo.name.ToLower().Contains("rgb"))
					continue;

				//save the alternative name
				DirectShowDevices.getDeviceAltNameFromStr(devCtx, out devInfo.altname, i);

				DirectShowDevices.EDEVICETYPE devType = DirectShowDevices.getDeviceTypeFromIndex(devCtx, i);
				devInfo.deviceType = devType;

				//set the device index from built list
				devInfo.deviceIndex = i;
				devInfo.outputs.Clear();

				int numStreams = DirectShowDevices.getNumStreamParamsInDeviceFromIndex(devCtx, i);
				checkDuplicates.Clear();
				removeTheseDuplicates.Clear();

				//iterate through the streams per device
				for (int j = 0; j < numStreams; j++)
				{
					OutputInfo outputInfo = new OutputInfo();
					DirectShowDevices.getVideoStreamParamsInDeviceFromIndex(devCtx, out tmpParams, i, j);
					outputInfo.width = tmpParams.width;
					outputInfo.height = tmpParams.height;
					outputInfo.fpsNum = tmpParams.fpsNum;
					outputInfo.fpsDen = tmpParams.fpsDen;
					outputInfo.bitrate = tmpParams.bitrate;
					outputInfo.numPlanes = tmpParams.numPlanes;
					outputInfo.bitsPerPixel = tmpParams.bitsPerPixel;
					outputInfo.pixelFormatInt = tmpParams.pixelFormat; //pixelFormat is an int, from VideoStreamParamsEx
					outputInfo.fieldScan = tmpParams.fieldScan;
					outputInfo.colorSpace = tmpParams.colorSpace;
					outputInfo.connectionType = tmpParams.connectionType;
					outputInfo.deviceType = tmpParams.deviceType;
					outputInfo.isDefaultStream = tmpParams.isDefaultStream;

					DirectShowDevices.avPixFmtToStringFromStr(tmpParams.pixelFormat, out outputInfo.pixelFormat);
                    terminateIndex = outputInfo.pixelFormat.IndexOf("\0");
                    if (terminateIndex > -1)
                        outputInfo.pixelFormat = outputInfo.pixelFormat.Substring(0, terminateIndex);

					//Debug.Log("\t[" + j + "] pixelFormat: " + outputInfo.pixelFormat);

					if (outputInfo.width == 0 || outputInfo.height == 0)
						continue;

					//check duplicates
					bool found = false;

					//find duplicate resolutions, and remove the lower framerate ones, first by producing a list of them
					if (outputInfo.deviceType != DirectShowDevices.EDEVICETYPE.DeckLinkVideoCapture)
					{
						foreach (var a in checkDuplicates)
						{
							if (a.width == outputInfo.width && a.height == outputInfo.height && a.pixelFormat == outputInfo.pixelFormat)
							{
								found = true;
								float aFps = (float)a.fpsNum / a.fpsDen;
								float outFps = (float)outputInfo.fpsNum / outputInfo.fpsDen;

								//if the stored info has higher framerate
								if (aFps > outFps)
								{
									//keep the stored one and remove the one we just used to compare, outputInfo
									removeTheseDuplicates.Add(outputInfo);
								}
								else if (aFps < outFps)
								{
									//keep the new one and remove the old one
									removeTheseDuplicates.Add(a);
									checkDuplicates.Remove(a);
									checkDuplicates.Add(outputInfo);
								}

								//found first occurence, we can get out of here
								break;
							}
						}

						if (found == false)
							checkDuplicates.Add(outputInfo);
					}

					AddOutputEntry(devInfo, outputInfo);
				}//for (Streams)

				//remove the lower framerates of duplicate resolutions and pixel format
				if (devInfo.outputs.Count > 0)
				{
					foreach (var a in removeTheseDuplicates)
						devInfo.outputs.Remove(a);
				}

				//add this device to the list along with its stream info
				if (devInfo.outputs.Count > 0)
					devices.Add(devInfo);

				removeTheseDuplicates.Clear();
				checkDuplicates.Clear();
				devInfo = null;
			}//for (Devices)

			removeTheseDuplicates = null;
			checkDuplicates = null;
			return;
		}

		public static void BuildDeviceList(DirectShowDevices.EDEVICETYPE deviceType = DirectShowDevices.EDEVICETYPE.AllVideoCapture)
		{
			devices.Clear();

			//this builds the device list in memory
			IntPtr devCtx = DirectShowDevices.buildDeviceList(deviceType, DirectShowDevices.DeviceEnumerator.GetEnumerator());
			if (devCtx != IntPtr.Zero)
			{
				EnumerateDeviceList(devCtx);
				DirectShowDevices.freeVideoDeviceList(devCtx);
				devCtx = IntPtr.Zero;
			}

			ProcessRealSenseInput();
		}

		public static bool HasDevice(string altName)
		{
			return devices.Any(device => device.altname == altName);
		}

		static void ProcessRealSenseInput()
		{
			List<DeviceInfo> realSenseDevices = devices.FindAll(d =>
			{
				string sanitized = d.name.ToLower().Trim();
				return sanitized.Contains("intel") && sanitized.Contains("realsense") &&
                    (sanitized.Contains("435") || sanitized.Contains("430") || sanitized.Contains("415"));
			});

			List<DeviceInfo> rgbModules = realSenseDevices.FindAll(d =>
			{
                string sanitized = d.name.ToLower().Trim().Replace("depth camera", "");
                return !sanitized.Contains("depth");
			});

			foreach (var module in rgbModules)
			{
				string modelNumber = "";
                if (module.name.Contains("435i"))
                    modelNumber = "435i";
				else if (module.name.Contains("430") || module.name.Contains("435"))
					modelNumber = "435";
				else if (module.name.Contains("415"))
					modelNumber = "415";
				else
					continue;

				devices.Add(new DeviceInfo()
				{
					name = "Intel(R) RealSense(TM) " + modelNumber,
					altname = module.altname,
					outputs = module.outputs.ToList(),
				});
			}

			BuildRealSenseDepthOutputList(realSenseDevices);

			foreach (DeviceInfo inf in realSenseDevices)
				devices.Remove(inf);
		}

		static void BuildRealSenseDepthOutputList(List<DeviceInfo> realSenseDevices)
		{
			realSenseDepthOutputs = realSenseDevices
				.Where(device => device.name.ToLower().Replace("depth camera","").Contains("depth"))
				.SelectMany(module => module.outputs)
				.OrderByDescending(output => output.width * output.height)
				// Remove duplicates.
				.GroupBy(module => module.width + "x" + module.height)
				.Select(group => group.First())
				.ToList();
		}

		static string GetValue(string line, string key)
		{
			int valStartIndex = line.IndexOf(key + "=");
			valStartIndex += key.Length;
			valStartIndex++;   //for = char
			string fromValStart = line.Substring(valStartIndex);
			int valEndIndex = fromValStart.IndexOf(" ");
			if (valEndIndex == -1)
				valEndIndex = fromValStart.Length;
			valEndIndex += valStartIndex;
			return line.Substring(valStartIndex, valEndIndex - valStartIndex);
		}

		static void AddOutputEntry(DeviceInfo devInfo, OutputInfo outputInfo)
		{
			bool found = false;

			//add but not duplicate entries
			foreach (var o in devInfo.outputs)
			{
				if (o.width == outputInfo.width && o.height == outputInfo.height && 
					o.pixelFormat == outputInfo.pixelFormat && 
					o.fpsNum == outputInfo.fpsNum && 
					o.fpsDen == outputInfo.fpsDen && 
					o.fieldScan == outputInfo.fieldScan)
				{
					found = true;
				}
			}
			if (!found && ShouldAddOutputEntry(devInfo, outputInfo))
			{
				devInfo.outputs.Add(outputInfo);
			}
		}


		static int SortEntries(OutputInfo x, OutputInfo y)
		{
			int widthComp = x.width.CompareTo(y.width);
			if (widthComp != 0)
				return widthComp;

			return x.height.CompareTo(y.height);
		}

		static int SortEntriesDescendingWidth(OutputInfo x, OutputInfo y)
		{
			int compareHeight = -x.height.CompareTo(y.height);
			if (compareHeight != 0)
				return compareHeight;

			int compareWidth = -x.width.CompareTo(y.width);
			return compareWidth;
		}

		public static bool IsVideoDeviceConnected(string altname, DirectShowDevices.EDEVICETYPE deviceType)
		{
			if (string.IsNullOrEmpty(altname) == true || deviceType == DirectShowDevices.EDEVICETYPE.UnknownCapture)
			{
				return true; // NONE selected - don't show watermark in this case.
			}

			bool res = DirectShowDevices.buildDeviceNamesFind(deviceType, DirectShowDevices.DeviceEnumerator.GetEnumerator(), altname);
			return res;
		}

		public static bool ShouldAddOutputEntry(DeviceInfo devInfo, OutputInfo output)
		{
			bool valid = false;
			if (((int)output.width) % 32 == 0)
			{
				valid = true;
			}

			if (valid && RealSenseInputFeed.IsVideoInputRealSense(devInfo.name) && realSenseDepthOutputs.Count > 0)
			{
				// filter out resolutions that are below the depth outputs.
				int index = realSenseDepthOutputs.FindIndex(rso => rso.width <= output.width && rso.height <= output.height);
				if (index >= 0)
				{
					return true;
				}
				valid = false;
			}

			return valid;
		}
	}
}
#endif
