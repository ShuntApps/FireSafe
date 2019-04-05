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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlueprintReality.MixCast.SDK {
    public class SetActiveFromViewfinder : MonoBehaviour {
        public List<GameObject> active = new List<GameObject>();
        public List<GameObject> inactive = new List<GameObject>();

        private bool lastState;

        protected virtual void OnEnable() {
            SetState( CalculateNewState() );
        }
        protected virtual void Update() {
            bool newState = CalculateNewState();
            if( newState != lastState )
                SetState( newState );
        }

        protected virtual void SetState( bool newState ) {
            lastState = newState;
            active.ForEach( g => g.SetActive( newState ) );
            inactive.ForEach( g => g.SetActive( !newState ) );
        }

        protected bool CalculateNewState() {
            return MixCastSdkData.Viewfinders.Count > 0;
        }
    }
}
#endif
