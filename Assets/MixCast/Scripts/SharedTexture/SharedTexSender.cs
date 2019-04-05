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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueprintReality.MixCast.SharedTexture
{
    [System.Serializable]
    public class SharedTextureSender
    {
        public bool ConvertColorIfRequired { get; protected set; }

        private string sharingId = null;
        public RenderTexture colorCorrectedTex = null;
        public Texture Tex { get; protected set; }

        public SharedTextureSender(bool convertColorIfRequired = false)
        {
            ConvertColorIfRequired = convertColorIfRequired;
        }

        public void UpdateFromTexture(string texId, Texture tex)
        {
            if (tex != Tex || (tex != null && string.IsNullOrEmpty(sharingId)))
            {
                Clear();

                Tex = tex;
                
                if (Tex != null)
                {
                    if (ConvertColorIfRequired && QualitySettings.activeColorSpace == ColorSpace.Linear)
                    {
                        colorCorrectedTex = new RenderTexture(Tex.width, Tex.height, (Tex as RenderTexture).depth, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
                        colorCorrectedTex.Create();
                        sharingId = SharedTextureManager.RegisterLocal(texId, colorCorrectedTex);
                    }
                    else
                    {
                        sharingId = SharedTextureManager.RegisterLocal(texId, Tex);
                    }
                }
            }

            if (tex != null && colorCorrectedTex != null)
            {
                Graphics.Blit(tex, colorCorrectedTex);
            }
        }

        public void Clear()
        {
            SharedTextureManager.UnregisterLocal(sharingId);
            sharingId = null;

            Tex = null;

            if (colorCorrectedTex != null)
            {
                colorCorrectedTex.Release();
                colorCorrectedTex = null;
            }
        }
    }
}
#endif
