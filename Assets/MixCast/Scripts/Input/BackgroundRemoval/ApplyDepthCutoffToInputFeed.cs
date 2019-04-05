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

namespace BlueprintReality.MixCast
{
    public class ApplyDepthCutoffToInputFeed : ApplyRemovalToInputFeed
    {
        private const string SHADER_KEYWORD = "BG_REMOVAL_DEPTH_CUTOFF";

        public const string MIN_DEPTH = "_BgDepthCutoff_MinDepth";
        public const string MAX_DEPTH = "_BgDepthCutoff_MaxDepth";

        protected override void StartRender(InputFeed feed)
        {
            if (context.Data == null || feed.context.Data != context.Data)
                return;

            Material blitMaterial = feed.ProcessTextureMaterial;
            if (blitMaterial != null && IsPossible())
            {
                blitMaterial.EnableKeyword(SHADER_KEYWORD);
                blitMaterial.SetFloat(MIN_DEPTH, context.Data.depthCutoffData.minDepth);
                blitMaterial.SetFloat(MAX_DEPTH, context.Data.depthCutoffData.maxDepth);
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
            return context.Data != null && context.Data.depthCutoffData.active;
        }
    }
}
#endif
