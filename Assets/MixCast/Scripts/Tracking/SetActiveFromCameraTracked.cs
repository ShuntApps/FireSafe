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
using System.Collections.Generic;
using UnityEngine;

namespace BlueprintReality.MixCast
{
    public class SetActiveFromCameraTracked : CameraComponent
    {
        public List<GameObject> tracked = new List<GameObject>();
        public List<GameObject> untracked = new List<GameObject>();

        private bool lastState;

        protected override void OnEnable()
        {
            base.OnEnable();
     
            SetState(CalculateNewState());
        }
        void Update()
        {
            bool newState = CalculateNewState();
            if (newState != lastState)
                SetState(newState);
        }

        bool CalculateNewState()
        {
            if (context.Data == null || context.Data.wasTracked == false)
                return false;

            for(int i =0; i< MixCastSdkData.TrackedObjects.Count;i++)
            {
                if (context.Data.trackedByDeviceId == MixCastSdkData.TrackedObjects[i].Identifier)
                    return MixCastSdkData.TrackedObjects[i].Connected;
            }

            return false;
        }
        void SetState(bool newState)
        {
            tracked.ForEach(g => g.SetActive(newState));
            untracked.ForEach(g => g.SetActive(!newState));
            lastState = newState;
        }
    }
}
#endif
