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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if MIXCAST_STEAMVR
using Valve.VR;
#endif
#if UNITY_2017_2_OR_NEWER
using Device = UnityEngine.XR.XRDevice;
using Tracking = UnityEngine.XR.InputTracking;
using Node = UnityEngine.XR.XRNode;
#else
using Device = UnityEngine.VR.VRDevice;
using Tracking = UnityEngine.VR.InputTracking;
using Node = UnityEngine.VR.VRNode;
#endif

namespace BlueprintReality.MixCast
{
    public class SetTransformFromTrackingOrigin : MonoBehaviour
    {
        private int lastRenderedFrameCount = -1;

        private Camera hmdCamera;

        private Vector3 trackingSpaceOffsetPos;
        private Quaternion trackingSpaceOffsetRot;

        private void OnEnable()
        {
            Camera.onPreRender += ApplyPoses;
        }
        private void OnDisable()
        {
            Camera.onPreRender -= ApplyPoses;
        }

        void ApplyPoses(Camera cam)
        {
            if (lastRenderedFrameCount == Time.renderedFrameCount)
                return;

            lastRenderedFrameCount = Time.renderedFrameCount;
            UpdateTrackingSpaceOffset();
            UpdateTransform();
        }

        void UpdateTransform()
        {
            if (!Device.isPresent)
            {
                ApplyTransformFromFirstRoom();
                return;
            }

            if (hmdCamera == null || !hmdCamera.isActiveAndEnabled)
            {
                hmdCamera = VRInfo.FindHMDCamera();
            }

            float playerScale = 1;
            if (hmdCamera != null && hmdCamera.transform.parent != null)
            {
                playerScale = hmdCamera.transform.parent.TransformVector(Vector3.forward).magnitude;
            }

            Vector3 mixcastTrackingOriginPos;
            Quaternion mixcastTrackingOriginRot;
            GetEngineOrigin(playerScale, out mixcastTrackingOriginPos, out mixcastTrackingOriginRot);

            ApplyTrackingSpaceCompensationToOrigin(playerScale, ref mixcastTrackingOriginPos, ref mixcastTrackingOriginRot);

            transform.position = mixcastTrackingOriginPos;
            transform.rotation = mixcastTrackingOriginRot;
            transform.localScale = Vector3.one * playerScale;
        }

        void ApplyTransformFromFirstRoom()
        {
            if (MixCastRoomBehaviour.ActiveRoomBehaviours.Count > 0)
            {
                transform.position = MixCastRoomBehaviour.ActiveRoomBehaviours[0].transform.position;
                transform.rotation = MixCastRoomBehaviour.ActiveRoomBehaviours[0].transform.rotation;
                transform.localScale = Vector3.one * MixCastRoomBehaviour.ActiveRoomBehaviours[0].transform.TransformVector(Vector3.forward).magnitude;
            }
        }

        void GetEngineOrigin(float playerScale, out Vector3 engineOriginPos, out Quaternion engineOriginRot)
        {
            Vector3 hmdLocalPos = Tracking.GetLocalPosition(Node.CenterEye);
            Quaternion hmdLocalRot = Tracking.GetLocalRotation(Node.CenterEye);

            engineOriginRot = hmdCamera.transform.rotation * Quaternion.Inverse(hmdLocalRot);
            engineOriginPos = hmdCamera.transform.position - playerScale * (engineOriginRot * hmdLocalPos);
        }

        void ApplyTrackingSpaceCompensationToOrigin(float playerScale, ref Vector3 pos, ref Quaternion rot)
        {
            rot = rot * Quaternion.Inverse(trackingSpaceOffsetRot);
            pos = pos - playerScale * (rot * trackingSpaceOffsetPos);
        }

#if MIXCAST_STEAMVR
        TrackedDevicePose_t[] standingPoses = new TrackedDevicePose_t[OpenVR.k_unMaxTrackedDeviceCount];
        TrackedDevicePose_t[] seatedPoses = new TrackedDevicePose_t[OpenVR.k_unMaxTrackedDeviceCount];
#endif

        void UpdateTrackingSpaceOffset()
        {
#if MIXCAST_STEAMVR
            if (VRInfo.IsDeviceOpenVR())
            {
                if (OpenVR.Compositor.GetTrackingSpace() == ETrackingUniverseOrigin.TrackingUniverseSeated)
                {
                    OpenVR.System.GetDeviceToAbsoluteTrackingPose(ETrackingUniverseOrigin.TrackingUniverseStanding, 0, standingPoses);
                    OpenVR.System.GetDeviceToAbsoluteTrackingPose(ETrackingUniverseOrigin.TrackingUniverseSeated, 0, seatedPoses);

                    for( int i = 0; i < standingPoses.Length; i++ )
                    {
                        if (standingPoses[i].bPoseIsValid && seatedPoses[i].bPoseIsValid)
                        {
                            SteamVR_Utils.RigidTransform standingHmdTransform = new SteamVR_Utils.RigidTransform(standingPoses[i].mDeviceToAbsoluteTracking);
                            SteamVR_Utils.RigidTransform seatedHmdTransform = new SteamVR_Utils.RigidTransform(seatedPoses[i].mDeviceToAbsoluteTracking);

                            SteamVR_Utils.RigidTransform sittingToStanding = SteamVR_Utils.RigidTransform.identity;
                            sittingToStanding.Multiply(standingHmdTransform, seatedHmdTransform.GetInverse());

                            if( Vector3.SqrMagnitude(trackingSpaceOffsetPos - sittingToStanding.pos) > 0.001f || Quaternion.Angle(trackingSpaceOffsetRot, sittingToStanding.rot) > 0.1f )
                            {
                                Debug.Log("Seated mode compensation \n" +
                                    "Pos: " + sittingToStanding.pos + "\n" +
                                    "Rot: " + sittingToStanding.rot);
                            }
                            trackingSpaceOffsetPos = sittingToStanding.pos;
                            trackingSpaceOffsetRot = sittingToStanding.rot;

                            break;
                        }
                    }
                }
                else
                {
                    trackingSpaceOffsetPos = Vector3.zero;
                    trackingSpaceOffsetRot = Quaternion.identity;
                }
            }
#else
            trackingSpaceOffsetPos = Vector3.zero;
            trackingSpaceOffsetRot = Quaternion.identity;
#endif
        }
    }
}
#endif
