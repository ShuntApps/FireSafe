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
using UnityEngine.Rendering;

namespace BlueprintReality.MixCast
{
    public class ApplyEdgeCroppingToInputFeed : ApplyRemovalToInputFeed
    {
        private const string SHADER_KEYWORD = "BG_REMOVAL_EDGE_CROP";

        public const string CROP_AMOUNTS_PROP = "_CropFromEdgeAmounts";

        private Vector4 amountPropVal;

        protected override void StartRender(InputFeed feed)
        {
            if (context.Data == null || feed.context.Data != context.Data)
                return;

            Material blitMaterial = feed.ProcessTextureMaterial;
            if (blitMaterial != null && IsPossible())
            {
                blitMaterial.EnableKeyword(SHADER_KEYWORD);

                if (!feed.FlipX)
                {
                    amountPropVal.x = feed.context.Data.edgeCroppingData.leftPercent;
                    amountPropVal.z = feed.context.Data.edgeCroppingData.rightPercent;
                }
                else
                {
                    amountPropVal.x = feed.context.Data.edgeCroppingData.rightPercent;
                    amountPropVal.z = feed.context.Data.edgeCroppingData.leftPercent;
                }

                if(!feed.FlipY)
                {
                    amountPropVal.y = feed.context.Data.edgeCroppingData.bottomPercent;
                    amountPropVal.w = feed.context.Data.edgeCroppingData.topPercent;
                }
                else
                {
                    amountPropVal.y = feed.context.Data.edgeCroppingData.topPercent;
                    amountPropVal.w = feed.context.Data.edgeCroppingData.bottomPercent;
                }
                
                blitMaterial.SetVector(CROP_AMOUNTS_PROP, amountPropVal);
            }
        }
        protected override void StopRender(InputFeed feed)
        {
            if (context.Data == null || feed.context.Data != context.Data)
                return;

            Material blitMaterial = feed.ProcessTextureMaterial;
            if (blitMaterial != null && IsPossible())
            {
                blitMaterial.DisableKeyword(SHADER_KEYWORD);
            }
        }

        protected override bool IsPossible()
        {
            return context.Data != null && context.Data.edgeCroppingData.active;
        }
    }
}
#endif
