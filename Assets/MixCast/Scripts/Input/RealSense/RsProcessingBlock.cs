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
using System.Collections.Generic;
using UnityEngine;
using Intel.RealSense;
using System.Linq;

namespace BlueprintReality.MixCast.RealSense
{
    public interface IProcessingBlock
    {
        Frame Process(Frame frame, FrameSource frameSource);
    }

    [Serializable]
    public abstract class RsProcessingBlock : ScriptableObject, IProcessingBlock
    {
        public bool enabled = true;

        public bool Enabled
        {
            get
            {
                return enabled;
            }

            set
            {
                enabled = value;
            }
        }

        public abstract Frame Process(Frame frame, FrameSource frameSource);

        public virtual void Reset()
        {
            this.name = GetType().Name;

#if UNITY_EDITOR && UNITY_2018_1_OR_NEWER
        var p = UnityEditor.AssetDatabase.GetAssetPath(this);
        Debug.Log(p);
        var names = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(p).Where(a => a).Select(a => a.name).ToList();
        names.Remove(GetType().Name);
        this.name = UnityEditor.ObjectNames.GetUniqueName(names.ToArray(), GetType().Name);
        UnityEditor.AssetDatabase.SaveAssets();
#endif
        }
    }
}
#endif
