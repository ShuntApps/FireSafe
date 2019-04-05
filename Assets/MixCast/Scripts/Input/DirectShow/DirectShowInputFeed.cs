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
using BlueprintReality.MixCast.RealSense;
using UnityEngine;
using System.Collections;

namespace BlueprintReality.MixCast
{
    public class DirectShowInputFeed : InputFeed
    {
        private string lastName = "";
        private uint lastWidth = 0, lastHeight = 0, lastFramerateNum = 0, lastFramerateDen = 0;
		private int lastPixelFormatInt = -1;
		private DirectShowDevices.EDEVICETYPE lastDeviceType = DirectShowDevices.EDEVICETYPE.UnknownCapture;
		private DirectShowDevices.ColorSpace lastColorSpace = DirectShowDevices.ColorSpace.BT709;
		private DirectShowDevices.FieldScan lastFieldScan = DirectShowDevices.FieldScan.unset;
		private DirectShowDevices.ConnectionType lastConnectionType = DirectShowDevices.ConnectionType.UNSET;


		private DirectShowInputFeedStream wTexture = null;
		
        public override Texture Texture
        {
            get
            {
                if (wTexture != null)
                    return wTexture.Texture;
                else
                    return null;
            }
        }

		protected void Update()
        {
			if (wTexture != null && wTexture.Texture != null)
				wTexture.RenderFrame();
        }

        protected override void LateUpdate()
        {
            if( UnityInfo.AreAllScenesLoaded() == false || 
				context == null || context.Data == null)
			{
                return;
            }

            // alternative way with no gc alloc
            if (RealSenseInputFeed.IsVideoInputRealSense(context.Data.deviceName))
                return;

            base.LateUpdate();

            if( isResettingCamera || !wasCameraConnected ) {
                return;
            }

			if (lastName != context.Data.deviceAltName || lastWidth != context.Data.deviceFeedWidth || lastHeight != context.Data.deviceFeedHeight ||
				lastFramerateNum != context.Data.deviceFramerateNum || lastFramerateDen != context.Data.deviceFramerateDen ||
				lastColorSpace != context.Data.deviceColorSpace || lastFieldScan != context.Data.deviceFieldScan || lastDeviceType != context.Data.deviceType ||
				lastConnectionType != context.Data.deviceConnectionType || lastPixelFormatInt != context.Data.devicePixelFormatInt)
            {
                context.Data.unplugged = false;
                ClearTexture();
				SetTexture();
            }
        }

		protected override void SetTexture()
        {
			if (context == null || context.Data == null)
				return;

			string devName = context.Data.deviceName;
            string devAltName = context.Data.deviceAltName;

            if (RealSenseInputFeed.IsVideoInputRealSense(devName))
                return;

            uint width = context.Data.deviceFeedWidth;
			uint height = context.Data.deviceFeedHeight;
			uint fpsNum = context.Data.deviceFramerateNum;
			uint fpsDen = context.Data.deviceFramerateDen;
			uint fpsOld = context.Data.deviceFramerate; //Previously, used only an uint to determine the framerate
			string pixfmt = context.Data.devicePixelFormat;
			int pixFmtInt = context.Data.devicePixelFormatInt;
			DirectShowDevices.EDEVICETYPE deviceType = context.Data.deviceType;
			DirectShowDevices.ColorSpace colorSpace = context.Data.deviceColorSpace;
			DirectShowDevices.FieldScan fieldScan = context.Data.deviceFieldScan;
			DirectShowDevices.ConnectionType connectionType = context.Data.deviceConnectionType;
			
			//we want to convert the framerate to the fractional frame rate numbers that we have
			if (fpsOld != 0 && (fpsNum == 0 || fpsDen == 0))
			{
				//Debug.Log("detected old framerate integer being used and converting it to newer fractional framerates.");
				fpsDen = DirectShowDevices.FRAMERATE_TO_FRACTIONAL_MULTIPLIER;
				fpsNum = fpsOld * DirectShowDevices.FRAMERATE_TO_FRACTIONAL_MULTIPLIER;
				fieldScan = DirectShowDevices.FieldScan.progressive;
				deviceType = DirectShowDevices.EDEVICETYPE.VideoCapture;
				colorSpace = DirectShowDevices.ColorSpace.BT709;
				connectionType = DirectShowDevices.ConnectionType.UNSET;
				pixFmtInt = DirectShowDevices.fmtStringToAvPixFmt(context.Data.devicePixelFormat);

				//resave the parameters
				context.Data.deviceFramerateNum = fpsNum;
				context.Data.deviceFramerateDen = fpsDen;
				context.Data.deviceFieldScan = fieldScan;
				context.Data.deviceType = deviceType;
				context.Data.deviceConnectionType = connectionType;
				context.Data.deviceColorSpace = colorSpace;
				context.Data.devicePixelFormatInt = pixFmtInt;
			}
			
			//Debug.Log("[type: " + deviceType + "] devname: " + devName + ", w: " + width + ", h: " + height + ",fps: " + fpsNum + "/" + fpsDen + ", pixFmt: " + pixfmt + ", fieldScan: " + fieldScan);

			if (devName == "" || string.IsNullOrEmpty(devAltName) || width == 0 || height == 0 || fpsNum == 0 || fpsDen == 0)
            {
				if (wTexture != null)
					wTexture.Stop();

                wTexture = null;
                return;
            }
            else if (wTexture == null)
            {
                Debug.Log(string.Format("Requesting Video Input with arguments: \nwidth: {1}, height: {2}\nfps: {3}\nPxlFmt: {4}\nFieldScan: {5}",
                    devName, width, height, (float)fpsNum / fpsDen, pixfmt, fieldScan));

                wTexture = new DirectShowInputFeedStream(devAltName, devName, deviceType, width, height, fpsNum, fpsDen, pixFmtInt, 
							fieldScan, connectionType, colorSpace);

				if (wTexture == null)
					return;
            }

            lastName = devAltName;
            lastWidth = width;
			lastHeight = height;
			lastFramerateNum = fpsNum;
			lastFramerateDen = fpsDen;
			lastPixelFormatInt = pixFmtInt;
			lastDeviceType = deviceType;
			lastColorSpace = colorSpace;
			lastFieldScan = fieldScan;
			lastConnectionType = connectionType;

			context.Data.isInUseElsewhere = false;

            if (wTexture != null)
            {
                if (wTexture.frameDataSize > 0)
                {
					isVideoFeedReady = true;
					wTexture.Play();
                }
                else
                {
					Debug.LogError("Error! Device could not be accessed. [" + devName + "] W x H: " + width + "x" + height + ", fps: " + fpsNum + "/" + fpsDen + ", pixFmt: " + pixfmt);
                    context.Data.isInUseElsewhere = true;
					if (wTexture != null)
						wTexture.Stop();
                    wTexture = null;
					isVideoFeedReady = false;
                }
            }
            else if (wTexture == null)
            {
                return;
            }
        }

        protected override void ClearTexture()
        {
			if (wTexture != null) //&& wTexture.Texture != null
			{
				wTexture.Stop();
				wTexture = null;
			}

            lastName = "";
            lastWidth = 0;
            lastHeight = 0;
            lastFramerateNum = 0;
			lastFramerateDen = 0;
			lastPixelFormatInt = -1;
			lastDeviceType = DirectShowDevices.EDEVICETYPE.VideoCapture;
			lastColorSpace = DirectShowDevices.ColorSpace.BT709;
			lastFieldScan = DirectShowDevices.FieldScan.unset;
			lastConnectionType = DirectShowDevices.ConnectionType.UNSET;
        }
    }
}
#endif
