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
using BlueprintReality.MixCast;
using BlueprintReality.MixCast.Data;

namespace BlueprintReality.MixCast
{
    public class TrackedObjectManager : MonoBehaviour
    {
        public static TrackedObject GetTrackedObjectById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            TrackedObject trackObj = null;
            for (int i = 0; i < MixCastSdkData.TrackedObjects.Count; i++)
            {
                TrackedObject obj = MixCastSdkData.TrackedObjects[i];
                if (obj.Identifier == id)
                {
                    trackObj = obj;
                    break;
                }
            }
            return trackObj;
        }

        public static TrackedObject GetTrackedObjectByRole(AssignedRole role)
        {
            if (MixCastSdkBehaviour.Instance == null)
                return null;

            TrackedObject trackObj = null;
            for (int i = 0; i < MixCastSdkData.TrackedObjects.Count; i++)
            {
                TrackedObject obj = MixCastSdkData.TrackedObjects[i];
                if (obj.AssignedRole == role)
                {
                    trackObj = obj;
                }
            }
            return trackObj;
        }

        public static bool GetTransformById(string id, out Vector3 pos, out Quaternion rotation)
        {
            var obj = GetTrackedObjectById(id);
            return GetTransform(obj, out pos, out rotation);
        }

        public static bool GetTransformByRole(AssignedRole role, out Vector3 pos, out Quaternion rotation)
        {
            var obj = GetTrackedObjectByRole(role);
            return GetTransform(obj, out pos, out rotation);
        }

        static bool GetTransform(TrackedObject trackedObj, out Vector3 pos, out Quaternion rotation)
        {
            if (trackedObj != null)
            {
                pos = trackedObj.Position.unity;
                rotation = trackedObj.Rotation.unity;
                return true;
            }
            pos = Vector3.zero;
            rotation = Quaternion.identity;
            return false;
        }

        public static bool IsHMDConnected()
        {
            if (MixCastSdkBehaviour.Instance == null)
                return false;

            bool found = false;
            for( int i = 0; i < MixCastSdkData.TrackedObjects.Count; i++ )
            {
                TrackedObject obj = MixCastSdkData.TrackedObjects[i];
                if (obj.ObjectType == ObjectType.HMD)
                    found = true;
            }
            return found;
        }

        public static bool HasTracking(ObjectType objType)
        {
            return objType == ObjectType.CONTROLLER || objType == ObjectType.HMD || objType == ObjectType.TRACKER;
        }
    }
}
#endif
