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
using System.Collections.Generic;
using System.IO;
using Thrift.Transport;
using Thrift.Protocol;
using Thrift.Configuration;
using UnityEngine;

namespace BlueprintReality.MixCast.Thrift
{
    public partial class ThriftInitializer
    {
        public static bool CheckConfig()
        {
            return Internal.ThriftInitializer.ReadFromFile(true);
        }
    }
}

namespace BlueprintReality.MixCast.Thrift.Internal
{
    internal partial class ThriftInitializer
    {
        internal const string MAIN_FOLDER_NAME = "MixCast";
        internal const string CONFIG_FOLDER_NAME = "Config";
        internal const string THRIFT_CONFIG_FILE = "Thrift_Initializer.config";

        internal static bool configSet = false;

        internal static string GetThriftConfigFolderPath()
        {
            var mainfolder = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), MAIN_FOLDER_NAME);
            var configfolder = Path.Combine(mainfolder, CONFIG_FOLDER_NAME);
            return configfolder;
        }

        internal static string GetThriftConfigFilePath()
        {
            return Path.Combine(GetThriftConfigFolderPath(), THRIFT_CONFIG_FILE);
        }

        internal static bool ReadFromFile(bool nolog = false)
        {
            if (!configSet)
            {
                var _path = GetThriftConfigFilePath();
                if (File.Exists(_path))
                {
                    var _bytes = File.ReadAllBytes(_path);

                    CentralConnectionConfig.config = TMemoryBuffer.DeSerialize<CentralHubConfig, TJSONProtocol>(_bytes);

                    configSet = true;

                    return true;
                }
                else
                {
                    if (!nolog)
                    {
                        Debug.LogError("Central Hub Config file is missing!");
                    }
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
    }
}
#endif
