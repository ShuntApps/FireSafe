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

namespace BlueprintReality.MixCast
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class SetCameraParametersFromMainCamera : MonoBehaviour
    {
        public bool clearSettings = true;
        public bool cullingMask = true;
        public bool clippingPlanes = true;
        public bool hdr = true;

        private Camera mainCam;
        private Camera cam;

        private void Awake()
        {
            cam = GetComponent<Camera>();
            LateUpdate();
        }
        private void LateUpdate()
        {
            if( mainCam == null || !mainCam.isActiveAndEnabled )
                mainCam = VRInfo.FindHMDCamera();

            if (mainCam == null)
                return;

            if (clearSettings)
            {
                cam.clearFlags = mainCam.clearFlags;
                cam.backgroundColor = mainCam.backgroundColor;
            }
            if( cullingMask)
            {
                cam.cullingMask = mainCam.cullingMask;
            }
            if (clippingPlanes)
            {
                //cam.nearClipPlane = mainCam.nearClipPlane; // this is causing flickering in SDK integrations, as it takes a very low value from the SteamVR CameraRig settings
                cam.farClipPlane = mainCam.farClipPlane;
            }
            if( hdr )
            {
#if UNITY_5_6_OR_NEWER
                cam.allowHDR = mainCam.allowHDR;
#else
                cam.hdr = mainCam.hdr;
#endif
            }
        }
    }
}
#endif
