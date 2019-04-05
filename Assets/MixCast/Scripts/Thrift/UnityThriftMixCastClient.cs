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
using Thrift.Unity;
using Thrift.Proxy;

namespace BlueprintReality.MixCast.Thrift
{
    public class UnityThriftMixCastClient : UnityThriftClient
    {
        static UnityThriftMixCastClient()
        {
            Internal.ThriftInitializer.ReadFromFile();
        }

        public new static T Get<T>() where T : ClientProxy
        {
            try
            {
                return UnityThriftClient.Get<T>();
            }
            catch
            {
                UnityEngine.Debug.LogError("Application is about to quit, fail to obtain the Thrift Client, this is likely because of the missing thrift config");
                UnityEngine.Application.Quit();
                return null;
            }
        }

        public new static bool ValidateOnce<T>(bool nolog = false) where T : ClientProxy
        {
            try
            {
                return UnityThriftClient.ValidateOnce<T>(nolog);
            }
            catch
            {
                UnityEngine.Debug.LogError("Fail to validate the Thrift Client, this is likely because of the missing thrift config");
                return false;
            }
        }

        public new static bool Validate<T>(bool nolog = false) where T : ClientProxy
        {
            try
            {
                return UnityThriftClient.Validate<T>(nolog);
            }
            catch
            {
                UnityEngine.Debug.LogError("Fail to validate the Thrift Client, this is likely because of the missing thrift config");
                return false;
            }
        }
        
        public new static bool ValidateCentralHub()
        {
            return UnityThriftClient.ValidateCentralHub();
        }

        public new static bool ValidateFunction<T>(string function_name, bool createDefaultClientIfNecessary = true) where T : ClientProxy
        {
            try
            {
                return UnityThriftClient.ValidateFunction<T>(function_name, createDefaultClientIfNecessary);
            }
            catch
            {
                UnityEngine.Debug.LogError("Fail to validate the Thrift Function, this is likely because of the missing thrift config");
                return false;
            }
        }
    }
}
#endif
