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

namespace BlueprintReality.MixCast
{
    public class CameraComponent : MonoBehaviour
    {
        public CameraConfigContext context;

        protected virtual void OnEnable()
        {
            if (context == null)
                context = GetComponentInParent<CameraConfigContext>();
            if(context != null)
                context.DataChanged += HandleDataChanged;
        }

        protected virtual void OnDisable()
        {
            if(context != null)
                context.DataChanged -= HandleDataChanged;
        }

        protected virtual void HandleDataChanged()
        {
        }

        public virtual void HandleDataUpdate() {
            HandleDataChanged();
        }

        protected virtual void Reset()
        {
            context = GetComponentInParent<CameraConfigContext>();
        }
    }
}
#endif
