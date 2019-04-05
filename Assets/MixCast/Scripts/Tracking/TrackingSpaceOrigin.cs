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

#if UNITY_STANDALONE_WIN && UNITY_2017_1_OR_NEWER
using UnityEngine;
using System.Collections.Generic;

#if UNITY_2017_2_OR_NEWER
using TrackingNodeState = UnityEngine.XR.XRNodeState;
using Node = UnityEngine.XR.XRNode;
#elif UNITY_2017_1_OR_NEWER
using TrackingNodeState = UnityEngine.VR.VRNodeState;
using Node = UnityEngine.VR.VRNode;
#endif

namespace BlueprintReality.MixCast
{
    public class TrackingSpaceOrigin
    {
        public static TrackingNodeState? GetReferenceSensorNode(List<TrackingNodeState> nodeStates)
        {
            TrackingNodeState? highestSensorNode = null;
            float highestSensorVal = 0;
            for (int i = 0; i < nodeStates.Count; i++)
            {
                if (nodeStates[i].nodeType != Node.TrackingReference)
                    continue;

                Vector3 pos;
                if (!nodeStates[i].TryGetPosition(out pos))
                    continue;

                if (highestSensorNode == null || highestSensorVal < pos.y)
                {
                    highestSensorNode = nodeStates[i];
                    highestSensorVal = pos.y;
                }
            }
            return highestSensorNode;
        }

        public static void CorrectWorldPose(TrackingNodeState nodeState, ref Vector3 pos, ref Quaternion rot)
        {
            Vector3 newSensorPos;
            nodeState.TryGetPosition(out newSensorPos);
            Quaternion newSensorRot;
            nodeState.TryGetRotation(out newSensorRot);
            newSensorRot = Quaternion.Euler(0, newSensorRot.eulerAngles.y + 180, 0);

            Matrix4x4 oldSensorMat = Matrix4x4.TRS(MixCast.Settings.sensorPose.position, MixCast.Settings.sensorPose.rotation, Vector3.one);
            Matrix4x4 newSensorMat = Matrix4x4.TRS(newSensorPos, newSensorRot, Vector3.one);

            Matrix4x4 oldCameraMat = Matrix4x4.TRS(pos, rot, Vector3.one);
            Matrix4x4 sensorSpaceCameraMat = oldSensorMat.inverse * oldCameraMat;
            Matrix4x4 newCameraMat = newSensorMat * sensorSpaceCameraMat;

            pos = newCameraMat.MultiplyPoint(Vector3.zero);
            rot = GetRotation(newCameraMat);
        }

        public static Quaternion GetRotation(Matrix4x4 matrix)
        {
            return Quaternion.LookRotation(matrix.GetColumn(2), matrix.GetColumn(1));
        }
    }
}
#endif
