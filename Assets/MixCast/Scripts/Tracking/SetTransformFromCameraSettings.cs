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
using System.Collections.Generic;
using UnityEngine;
#if UNITY_2017_2_OR_NEWER
using UnityEngine.XR;
using NodeState = UnityEngine.XR.XRNodeState;
#elif UNITY_2017_1_OR_NEWER
using UnityEngine.VR;
using NodeState = UnityEngine.VR.VRNodeState;
#endif

namespace BlueprintReality.MixCast
{
    public class SetTransformFromCameraSettings : CameraComponent
    {
        public class TransformData
        {
            public Vector3 pos;
            public Quaternion rot;
        }

        public bool delayWithBuffer = false;

        protected Vector3 smoothPositionVel, smoothRotationVel;
        protected FrameDelayQueue<TransformData> frames = new FrameDelayQueue<TransformData>();
#if UNITY_2017_1_OR_NEWER
        protected List<NodeState> trackingNodeStates = new List<NodeState>();
#endif

        protected override void OnEnable()
        {
            base.OnEnable();

            /// Called in Update() anyway
            //TrackedDeviceManager.OnTransformsUpdated += UpdateTransform;

            HandleDataChanged();
        }
        protected override void OnDisable()
        {
            /// Called in Update() anyway
            //TrackedDeviceManager.OnTransformsUpdated -= UpdateTransform;

            base.OnDisable();
        }

        protected override void HandleDataChanged()
        {
            base.HandleDataChanged();

            if (context.Data != null)
                UpdateTransform();
        }

        private void Update()
        {
            if (context.Data == null)
                return;

            UpdateTransform();
        }

        protected virtual void UpdateTransform()
        {
            frames.delayDuration = delayWithBuffer ? context.Data.bufferTime : 0;
            frames.Update();

#if UNITY_2017_1_OR_NEWER
            InputTracking.GetNodeStates(trackingNodeStates);
#endif

            FrameDelayQueue<TransformData>.Frame<TransformData> newFrame = frames.GetNewFrame();
            if (newFrame.data == null)
                newFrame.data = new TransformData();
            GetCameraTransform(out newFrame.data.pos, out newFrame.data.rot);

            TransformData oldFrame = frames.OldestFrameData;
            SetTransformFromTarget(oldFrame.pos, oldFrame.rot);
        }

        public void GetCameraTransform(out Vector3 position, out Quaternion rotation)
        {
            CameraUtilities.GetCameraPose(context.Data, out position, out rotation);

#if UNITY_2017_1_OR_NEWER
            if( VRInfo.IsVRSystemOculus() )
            {
                NodeState? refNode = TrackingSpaceOrigin.GetReferenceSensorNode(trackingNodeStates);
                if (refNode.HasValue)
                {
                    TrackingSpaceOrigin.CorrectWorldPose(refNode.Value, ref position, ref rotation);
                }
            }
#endif
        }

        void SetTransformFromTarget(Vector3 newPos, Quaternion newRot, bool instant = false)
        {
            if (context.Data.positionSmoothTime > 0 && !instant)
                newPos = Vector3.SmoothDamp(transform.localPosition, newPos, ref smoothPositionVel, context.Data.positionSmoothTime);
            transform.localPosition = newPos;

            if (context.Data.rotationSmoothTime > 0 && !instant)
            {
                Vector3 curEuler = transform.localEulerAngles;
                Vector3 newEuler = newRot.eulerAngles;
                newRot = Quaternion.Euler(new Vector3(
                    Mathf.SmoothDampAngle(curEuler.x, newEuler.x, ref smoothRotationVel.x, context.Data.rotationSmoothTime),
                    Mathf.SmoothDampAngle(curEuler.y, newEuler.y, ref smoothRotationVel.y, context.Data.rotationSmoothTime),
                    Mathf.SmoothDampAngle(curEuler.z, newEuler.z, ref smoothRotationVel.z, context.Data.rotationSmoothTime)
                    ));
            }
            transform.localRotation = newRot;
        }
    }
}
#endif
