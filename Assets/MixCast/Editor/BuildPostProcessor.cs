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
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

namespace BlueprintReality.MixCast
{
    public class BuildPostProcessor
    {
        public const string VERSION_FILENAME = "version.txt";
        public const string LEGAL_SRC_FILENAME = "mixcast_legal.txt";
        public const string LEGAL_DST_FILENAME = "legal.txt";

        [PostProcessBuildAttribute(99)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target != BuildTarget.StandaloneWindows64)
                return;

            string exeName = Path.GetFileNameWithoutExtension(pathToBuiltProject);
            string buildFolder = Path.GetDirectoryName(pathToBuiltProject);
            string dataFolder = Path.Combine(buildFolder, exeName + "_Data");
            string streamingAssetsFolder = Path.Combine(dataFolder, "StreamingAssets");
            if (!Directory.Exists(streamingAssetsFolder))
                Directory.CreateDirectory(streamingAssetsFolder);
            string mixcastFolder = Path.Combine(streamingAssetsFolder, "MixCast");
            if (!Directory.Exists(mixcastFolder))
                Directory.CreateDirectory(mixcastFolder);

            CreateVersionTxt(mixcastFolder);
            CopyLegalTxt(mixcastFolder);
        }

        static void CreateVersionTxt(string mixcastFolder)
        {
            File.WriteAllText(Path.Combine(mixcastFolder, VERSION_FILENAME), MixCastSdk.VERSION_STRING);
        }

        static void CopyLegalTxt(string mixcastFolder)
        {
            string[] srcFile = Directory.GetFiles(Application.dataPath, LEGAL_SRC_FILENAME, SearchOption.AllDirectories);
            if (srcFile.Length > 0)
                File.Copy(srcFile[0], Path.Combine(mixcastFolder, LEGAL_DST_FILENAME), true);
        }
    }
}
#endif
