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

using UnityEngine;

namespace BlueprintReality.MixCast
{
	public class MixCastProjectSettings : ScriptableObject
#if UNITY_EDITOR
        , ISerializationCallbackReceiver
#endif
    {
        //Generated
        [SerializeField]
        private string projectId;
        public string ProjectID { get { return projectId; } }

        //Transparency
        public bool usingPMA = false;
        public bool grabUnfilteredAlpha = true;

        //Quality
        public bool overrideQualitySettingsAA = true;
        public int overrideAntialiasingVal = 0;

        //Effects
        public string subjectLayerName = "Default";
        public bool specifyLightsManually = false;
        public float directionalLightPower = 1f;
        public float pointLightPower = 1f;
        public bool includeBacklighting = true;

        //Editor
        public bool requireCommandLineArg = false;
        public bool enableMixCastInEditor = true;
        public bool displaySubjectInScene = false;
        public bool applySdkFlagsAutomatically = true;

#if UNITY_EDITOR
        public void OnBeforeSerialize()
        {
            if (string.IsNullOrEmpty(projectId))
                projectId = System.Guid.NewGuid().ToString();
        }

        public void OnAfterDeserialize()
        {
            if (string.IsNullOrEmpty(projectId))
                projectId = System.Guid.NewGuid().ToString();
        }
#endif
    }
}
