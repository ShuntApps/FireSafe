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
using BlueprintReality.MixCast.Data;
using BlueprintReality.MixCast.Thrift;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueprintReality.MixCast
{
    public class Service_SDK_Handler : Service_SDK.Handler
    {
        public static event System.Action OnResetWorldOrientation;
        public static event System.Action<float> OnModifyWorldOrientation;

        public static event System.Action<string> OnCommandReceived;

        public override void SetActivationState(bool active)
        {
            //Debug.Log("SetActivationState(" + active + ")");
            MixCastSdk.Active = active;
        }

        public override void NotifyServiceStarted()
        {
            UnityThriftMixCastClient.Get<SDK_Service.Client>().TryNotifySdkStarted(MixCastSdkData.ExperienceInfo);
        }

        public override void UpdateLicenseMetadata(License license)
        {
            MixCastSdkData.License = license;
        }
        public override void UpdateTrackedObjectMetadata(List<TrackedObject> trackedObjects)
        {
            MixCastSdkData.TrackedObjects = trackedObjects;
        }
        public override void UpdateCameraMetadata(List<VirtualCamera> cameras)
        {
            MixCastSdkData.Cameras = cameras;
        }
        public override void UpdateDesktopMetadata(Desktop desktop)
        {
            MixCastSdkData.Desktop = desktop;
        }
        public override void UpdateViewfinderMetadata(List<Viewfinder> viewfinders)
        {
            MixCastSdkData.Viewfinders = viewfinders;
        }


        public override void ResetWorldOrientation()
        {
            Debug.Log("MixCast called: ResetWorldOrientation()");
            if (OnResetWorldOrientation != null)
                OnResetWorldOrientation();
        }

        public override void ModifyWorldOrientation(double degrees)
        {
            Debug.Log("MixCast called: ModifyWorldOrientation(" + degrees + ")");
            if (OnModifyWorldOrientation != null)
                OnModifyWorldOrientation((float)degrees);
        }

        public override void SendExperienceCommand(string eventId)
        {
            Debug.Log("MixCast called: SendExperienceCommand(" + eventId + ")");
            if (OnCommandReceived != null)
                OnCommandReceived(eventId);
        }
    }
}
#endif
