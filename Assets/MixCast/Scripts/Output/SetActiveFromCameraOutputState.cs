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

namespace BlueprintReality.MixCast
{
    public class SetActiveFromCameraOutputState : CameraComponent
    {
        public enum Mode
        {
            Desktop = 0,
            AutoSnapshot = 1,
            VideoRecording = 2,
            VideoStreaming = 3,
        }

        public Mode mode = Mode.Desktop;

        public List<GameObject> active = new List<GameObject>();
        public List<GameObject> inactive = new List<GameObject>();

        private bool lastState;

        protected override void OnEnable()
        {
            base.OnEnable();

            ApplyState(CalculateNewState());
        }

        private void Update()
        {
            bool newState = CalculateNewState();
            if (newState != lastState)
                ApplyState(newState);
        }

        bool CalculateNewState()
        {
            if (context.Data == null)
                return false;

            if( mode == Mode.Desktop )
            {
                for (int i = 0; i < MixCastSdkData.Desktop.DisplayingCameraIds.Count; i++)
                    if (MixCastSdkData.Desktop.DisplayingCameraIds[i] == context.Data.id)
                        return true;
                return false;
            }


            VirtualCamera cam = GetCamera();
            if (cam == null)
                return false;

            switch (mode)
            {
                case Mode.AutoSnapshot:
                    return cam.AutoSnapshotEnabled;

                case Mode.VideoRecording:
                    return cam.VideoRecordingEnabled;

                case Mode.VideoStreaming:
                    return cam.VideoStreamingEnabled;

                default:
                    return false;
            }
        }
        VirtualCamera GetCamera()
        {
            for (int i = 0; i < MixCastSdkData.Cameras.Count; i++)
                if (MixCastSdkData.Cameras[i].Identifier == context.Data.id)
                    return MixCastSdkData.Cameras[i];
            return null;
        }

        void ApplyState(bool newState)
        {
            lastState = newState;
            for (int i = 0; i < active.Count; i++)
                active[i].SetActive(newState);
            for (int i = 0; i < inactive.Count; i++)
                inactive[i].SetActive(!newState);
        }
    }
}
#endif
