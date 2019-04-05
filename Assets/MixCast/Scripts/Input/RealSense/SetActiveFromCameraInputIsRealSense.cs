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

namespace BlueprintReality.MixCast.RealSense
{
	public class SetActiveFromCameraInputIsRealSense : CameraComponent
    {
        public List<GameObject> activeIfMatch = new List<GameObject>();
        public List<GameObject> inactiveIfMatch = new List<GameObject>();

        private bool lastState;
        [SerializeField] bool mustBePluggedIn = false;

        protected override void OnEnable()
        {
            base.OnEnable();
            SetNewState(CalculateNewState());
        }
        private void Update()
        {
            bool newState = CalculateNewState();
            if (newState != lastState)
                SetNewState(newState);
        }

        bool CalculateNewState()
        {
            if (context.Data == null)
                return false;

            if (RealSenseInputFeed.IsVideoInputRealSense(context.Data.deviceName))
            {
                if (mustBePluggedIn)
                    return !context.Data.unplugged;
                else
                    return true;
            }
            else
                return false;
        }

        void SetNewState(bool newState)
        {
            lastState = newState;
            activeIfMatch.ForEach(g => g.SetActive(newState));
            inactiveIfMatch.ForEach(g => g.SetActive(!newState));
        }
	}
}
#endif