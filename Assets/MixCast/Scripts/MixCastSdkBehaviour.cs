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

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_STANDALONE_WIN
using Thrift.Unity;
using BlueprintReality.MixCast.Data;
using BlueprintReality.MixCast.Thrift;
#endif

namespace BlueprintReality.MixCast
{
    public class MixCastSdkBehaviour : MonoBehaviour
    {
#if UNITY_STANDALONE_WIN
        public static MixCastSdkBehaviour Instance { get; protected set; }

        public SdkVirtualTrackedObjectManager VirtualTrackedObjectManager { get; protected set; }

        private void Awake()
        {
            GameObject thriftObjGroup = new GameObject("Thrift Objects");
            thriftObjGroup.transform.SetParent(transform);
            UnityThriftBase.GroupTransform = thriftObjGroup.transform;
        }

        private IEnumerator Start()
        {
            bool shouldntExist = false;

            if (Instance != null)
                shouldntExist = true;

#if UNITY_EDITOR
            if (!MixCastSdkData.ProjectSettings.enableMixCastInEditor)
                shouldntExist = true;
#else
            if( MixCastSdkData.ProjectSettings.requireCommandLineArg && System.Array.IndexOf<string>(System.Environment.GetCommandLineArgs(), "-mixcast") == -1 )
                shouldntExist = true;
#endif

            if (shouldntExist)
            {
                enabled = false;
                Destroy(this);
                yield break;
            }

            while (!ThriftInitializer.CheckConfig() || !UnityThriftMixCastClient.ValidateCentralHub())
            {
                yield return null;
            }

            transform.parent = null;
            DontDestroyOnLoad(gameObject);

            Instance = this;

            SetExperienceInfo();

            UnityThriftMixCastServer.Get<Service_SDK_Handler>();

            VirtualTrackedObjectManager = new SdkVirtualTrackedObjectManager();
            VirtualTrackedObjectManager.Activate();

            UnityThriftMixCastClient.Get<SDK_Service.Client>().TryNotifySdkStarted(MixCastSdkData.ExperienceInfo);

            InvokeProtector.InvokeRepeating(VirtualTrackedObjectManager.Update, 0f, 1f / 90f);
            InvokeProtector.InvokeRepeating(MixCastAV.LogOutput, 0f, 1f / 10f);
            InvokeProtector.InvokeRepeating(VerifyServiceConnection, 0f, 1f / 10f);

            UnityEngine.SceneManagement.SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void WarnIfWrongPlatform()
        {
            if (IntPtr.Size == 4)
                Debug.LogWarning("This application is built for 32 bit (x86) platform but MixCast is only compatible with 64 bit (x64). MixCast may not work correctly");
        }
        private void SetExperienceInfo()
        {
            MixCastSdkData.ExperienceInfo.ExperienceExePath = UnityInfo.GetExePath();
            MixCastSdkData.ExperienceInfo.ExperienceTitle = Application.productName;
            MixCastSdkData.ExperienceInfo.OrganizationName = Application.companyName;
            MixCastSdkData.ExperienceInfo.MixcastVersion = MixCastSdk.VERSION_STRING;
            MixCastSdkData.ExperienceInfo.EngineVersion = Application.unityVersion;
            MixCastSdkData.ExperienceInfo.ProjectId = MixCastSdkData.ProjectSettings.ProjectID;
            MixCastSdkData.ExperienceInfo.AlphaIsPremultiplied = MixCastSdkData.ProjectSettings.usingPMA;
        }

        private void HandleSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            MixCastSdk.SendCustomEvent("sceneLoaded(" + scene.name + ")");
        }

        private void OnApplicationQuit()
        {
            if (Instance != this)
                return;

            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= HandleSceneLoaded;

            InvokeProtector.CancelInvokeRepeating(VirtualTrackedObjectManager.Update);
            InvokeProtector.CancelInvokeRepeating(MixCastAV.LogOutput);
            InvokeProtector.CancelInvokeRepeating(VerifyServiceConnection);

            VirtualTrackedObjectManager.Deactivate();
            VirtualTrackedObjectManager = null;

            UnityThriftMixCastClient.Get<SDK_Service.Client>().TryNotifySdkStopped();

            Instance = null;
        }

        private void VerifyServiceConnection()
        {
            if (MixCastSdk.Active)
            {
                if (!UnityThriftMixCastClient.Validate<SDK_Service.Client>(true))
                {
                    Debug.LogWarning("MixCast Service quit abruptly");
                    MixCastSdk.Active = false;
                    UnityThriftMixCastClient.Get<SDK_Service.Client>().TryNotifySdkStarted(MixCastSdkData.ExperienceInfo);
                }
            }
        }
#endif
    }
}
