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
using Thrift.Unity.Multicast;
using Thrift.Proxy;

namespace BlueprintReality.MixCast.Thrift
{
    public class UnityThriftMixCastMulticast : UnityThriftMulticast
    {
        static UnityThriftMixCastMulticast()
        {
            Internal.ThriftInitializer.ReadFromFile();
        }

        public new static MulticastServer<C, H> GetServer<C, H>() where C : ClientProxy where H : HandlerProxy
        {
            try
            {
                return UnityThriftMulticast.GetServer<C, H>();
            }
            catch
            {
                UnityEngine.Debug.LogError("Application is about to quit, fail to obtain the Multicast Server, this is likely because of the missing thrift config");
                UnityEngine.Application.Quit();
                return null;
            }
        }

        public new static MulticastClient<C, H> GetClient<C, H>() where C : ClientProxy where H : HandlerProxy
        {
            try
            {
                return UnityThriftMulticast.GetClient<C, H>();
            }
            catch
            {
                UnityEngine.Debug.LogError("Application is about to quit, fail to obtain the Multicast Client, this is likely because of the missing thrift config");
                UnityEngine.Application.Quit();
                return null;
            }
        }
    }
}
#endif
