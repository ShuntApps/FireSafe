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
namespace BlueprintReality.MixCast.RealSense
{
    [CreateAssetMenu]
    public class RsProcessingProfile : ScriptableObject, IEnumerable<RsProcessingBlock>
    {
        // [HideInInspector]
        [SerializeField]
        public List<RsProcessingBlock> _processingBlocks;

        public IEnumerator<RsProcessingBlock> GetEnumerator()
        {
            return _processingBlocks.GetEnumerator() as IEnumerator<RsProcessingBlock>;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _processingBlocks.GetEnumerator();
        }


#if UNITY_EDITOR
        void Reset()
        {

            var obj = new UnityEditor.SerializedObject(this);
            obj.Update();

            var blocks = obj.FindProperty("_processingBlocks");
            blocks.ClearArray();

            var p = UnityEditor.AssetDatabase.GetAssetPath(this);
            var bl = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(p);
            foreach (var a in bl)
            {
                if (a == this)
                    continue;
                // Debug.Log(a, a);
                // DestroyImmediate(a, true);
                int i = blocks.arraySize++;
                var e = blocks.GetArrayElementAtIndex(i);
                e.objectReferenceValue = a;
            }

            obj.ApplyModifiedProperties();
            // UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();

        }
#endif
    }
}
#endif
