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
using UnityEditor;
using System.IO;

namespace BlueprintReality.MixCast {
    [InitializeOnLoad]
    public class SdkImporter : AssetPostprocessor {
        const string PROJECT_SETTINGS_FILENAME = "MixCast_ProjectSettings.asset";

        static SdkImporter()
        {
            EnsureProjectSettingsExist();
        }

        static void EnsureProjectSettingsExist()
        {
            string[] settingsPaths = Directory.GetFiles(Application.dataPath, PROJECT_SETTINGS_FILENAME, SearchOption.AllDirectories);
            if (settingsPaths.Length > 0)
                return;

            string resourcesPath = Application.dataPath + "/Resources";
            if (!Directory.Exists(resourcesPath))
                Directory.CreateDirectory(resourcesPath);

            string settingsPath = resourcesPath + "/" + PROJECT_SETTINGS_FILENAME;
            int assetFolderIndex = settingsPath.LastIndexOf("/Assets/");
            settingsPath = settingsPath.Substring(assetFolderIndex + 1);

            MixCastProjectSettings settingsAsset = ScriptableObject.CreateInstance<MixCastProjectSettings>();
            settingsAsset.name = Path.GetFileNameWithoutExtension(PROJECT_SETTINGS_FILENAME);

            AssetDatabase.CreateAsset(settingsAsset, settingsPath);
            AssetDatabase.SaveAssets();
        }

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            bool deletingProjectSettings = false;
            for (int i = 0; i < deletedAssets.Length; i++)
            {
                if (deletedAssets[i].EndsWith(PROJECT_SETTINGS_FILENAME))
                    deletingProjectSettings = true;
            }
            for( int i = 0; i < importedAssets.Length; i++ )
            {
                if (importedAssets[i].EndsWith(PROJECT_SETTINGS_FILENAME))
                    deletingProjectSettings = false;
            }

            if( deletingProjectSettings )
            {
                Debug.LogError("MixCast requires that its Project Settings are included in the project!");
                EnsureProjectSettingsExist();
            }
        }
    }
}
#endif
