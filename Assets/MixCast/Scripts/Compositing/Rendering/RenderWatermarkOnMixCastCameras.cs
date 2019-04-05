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
using BlueprintReality.MixCast.Data;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BlueprintReality.MixCast
{
    public class RenderWatermarkOnMixCastCameras : MonoBehaviour
    {
        const string WATERMARK_SHADER = "Hidden/MixCast/Watermark";

        const string MC_LOGO_RESNAME = "MixCast_Logo";

        private static BlitTexture logoBlit;

        void Start()
        {
            InitWatermarks();
        }
        private void OnDestroy()
        {
            if (logoBlit != null)
                MixCastCamera.FrameEnded -= ApplyWatermarks;
        }

        private void InitWatermarks()
        {
            logoBlit = new BlitTexture();
            logoBlit.SetTexturePosition(BlitTexture.Position.BottomLeft);
            logoBlit.Material = new Material(Shader.Find(WATERMARK_SHADER));
            logoBlit.Texture = Resources.Load<Texture2D>(MC_LOGO_RESNAME);

            MixCastCamera.FrameEnded += ApplyWatermarks;
        }
        private void ApplyWatermarks(MixCastCamera cam)
        {
            bool freeApplies = MixCastSdkData.License.LicenseType == LicenseType.None || MixCastSdkData.License.LicenseType == LicenseType.Free;

            bool drawLogo = logoBlit != null && freeApplies;
            if (drawLogo)
            {
                logoBlit.ApplyToFrame(cam);
            }
        }
    }
}
#endif
