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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlueprintReality.MixCast.Data;

namespace BlueprintReality.MixCast
{
    [AddComponentMenu("MixCast/Custom Tracked Object")]
    public class CustomTrackedObjectBehaviour : MonoBehaviour
    {
        [Tooltip("This should be a unique identifier for the tracked object, which can be referenced on successive application runs")]
        public string objectIdentifier = "";
        [Tooltip("This should be a human-readable name for the tracked object")]
        public string objectName = "";
        [Tooltip("This describes the form factor of the tracked object, which may be used in determining if/how MixCast uses it")]
        public ObjectType objectType = ObjectType.VIRTUAL;
        [Tooltip("Should only be set if the head or hands aren't already specified through OpenVR or Oculus")]
        public AssignedRole objectRole = AssignedRole.UNKNOWN;
        [Tooltip("This should be true unless you are creating tracked objects that shouldn't be displayed")]
        public bool hideFromUser = false;

        [Tooltip("If this is set, poses reported to MixCast will be relative to the origin's transform rather than being in world space")]
        public Transform origin;

#if UNITY_STANDALONE_WIN
        private bool registered = false;

        private void OnEnable()
        {
            registered = false;
        }
        private void Update()
        {
            if( !registered && MixCastSdkBehaviour.Instance != null)
            {
                MixCastSdkBehaviour.Instance.VirtualTrackedObjectManager.VirtualTrackedObjectBehaviours.Add(this);
                registered = true;
            }
        }
        private void OnDisable()
        {
            if (registered && MixCastSdkBehaviour.Instance != null && MixCastSdkBehaviour.Instance.VirtualTrackedObjectManager != null)
                MixCastSdkBehaviour.Instance.VirtualTrackedObjectManager.VirtualTrackedObjectBehaviours.Remove(this);
        }


        public Vector3 GetPosition()
        {
            if (origin != null)
                return origin.InverseTransformPoint(transform.position);
            else
                return transform.position;
        }
        public Quaternion GetRotation()
        {
            if (origin != null)
                return transform.rotation * Quaternion.Inverse(origin.rotation);
            else
                return transform.rotation;
        }
#endif
    }
}
