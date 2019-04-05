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
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BlueprintReality.MixCast
{
    public class UnityInfo
    {
        public static string GetExePath()
        {
#if !UNITY_EDITOR
            DirectoryInfo appDir = new DirectoryInfo(Application.dataPath);
            string exeName = appDir.Name.Substring(0, appDir.Name.Length - "_Data".Length);
            appDir = appDir.Parent;
            return Path.Combine(appDir.FullName, exeName + ".exe");
#else
            return UnityEditor.EditorApplication.applicationPath;
#endif
        }

        public static bool AreAllScenesLoaded()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).isLoaded == false)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsInputFieldFocused()
        {
            for (int i = 0; i < Selectable.allSelectables.Count; i++)
            {
                if (Selectable.allSelectables[i] is InputField && (Selectable.allSelectables[i] as InputField).isFocused)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
#endif
