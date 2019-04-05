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
using System.IO;
using UnityEngine;
#if MIXCAST_STEAMVR
using Valve.VR;
#endif

namespace BlueprintReality.MixCast
{
    public class MixCastCameras : MonoBehaviour
    {
        public static MixCastCameras Instance { get; protected set; }

        public CameraConfigContext cameraPrefab;

        [Tooltip("Please keep this enable to avoid losing your camera feed")]
        public bool keepCameraWhenChangingScene = true;

        public List<CameraConfigContext> CameraInstances { get; protected set; }

        protected Transform lastParent;

        //Caching objects for memory management
        List<MixCastData.CameraCalibrationData> createCams = new List<MixCastData.CameraCalibrationData>();
        List<CameraConfigContext> destroyCams = new List<CameraConfigContext>();

        void Awake()
        {
#if !UNITY_EDITOR
            if( MixCastSdkData.ProjectSettings.requireCommandLineArg && System.Array.IndexOf<string>(System.Environment.GetCommandLineArgs(), "-mixcast") == -1 )
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
#endif

            if (GetComponent<SetTransformFromTrackingOrigin>() == null)
                gameObject.AddComponent<SetTransformFromTrackingOrigin>();

            if (GetComponent<RenderWatermarkOnMixCastCameras>() == null)
                gameObject.AddComponent<RenderWatermarkOnMixCastCameras>();

            if (GetComponent<CameraRenderScheduler>() == null)
                gameObject.AddComponent<CameraRenderScheduler>();
        }

        private void OnEnable()
        {
            if (transform.parent != null)
                lastParent = transform.parent;

            if (Instance != null)
            {
                Instance.lastParent = lastParent;
                DestroyImmediate(gameObject);
                return;
            }

            Instance = this;
            //Move Cameras to the root so DontDestroyOnLoad works
            if (keepCameraWhenChangingScene)
            {
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
            }

            GenerateCameras();
        }

        private void OnDisable()
        {
            if (Instance != this) { return; }

            DestroyCameras();

            Instance = null;
        }

        private void Update()
        {
            if (MixCastRoomBehaviour.ActiveRoomBehaviours.Count == 0 && lastParent != null && lastParent.GetComponent<MixCastRoomBehaviour>() == null)
                lastParent.gameObject.AddComponent<MixCastRoomBehaviour>();

            if (MixCastSdk.Active)
            {
                if (CameraInstances.Count > 0 &&
                    !CameraInstances[0].gameObject.activeSelf &&
                    UnityInfo.AreAllScenesLoaded())
                {
                    SetCamerasActive(true);
                }
            }
            else
            {
                if(CameraInstances.Count > 0 &&
                    CameraInstances[0].gameObject.activeSelf )
                {
                    SetCamerasActive(false);
                }
            }

            for (int i = 0; i < MixCast.Settings.cameras.Count; i++)
                createCams.Add(MixCast.Settings.cameras[i]);
            for (int i = 0; i < CameraInstances.Count; i++)
                destroyCams.Add(CameraInstances[i]);
            for (int i = 0; i < CameraInstances.Count; i++)
            {
                MixCastData.CameraCalibrationData camData = CameraInstances[i].Data;
                for (int j = createCams.Count - 1; j >= 0; j--)
                    if (createCams[j] == camData)
                        createCams.RemoveAt(j);
            }
            for (int i = 0; i < MixCast.Settings.cameras.Count; i++)
            {
                for (int j = destroyCams.Count - 1; j >= 0; j--)
                    if (destroyCams[j].Data == MixCast.Settings.cameras[i])
                        destroyCams.RemoveAt(j);
            }

            for (int i = 0; i < destroyCams.Count; i++)
            {
                CameraInstances.Remove(destroyCams[i]);
                Destroy(destroyCams[i].gameObject);
            }

            for (int i = 0; i < createCams.Count; i++)
            {
                bool wasPrefabActive = cameraPrefab.gameObject.activeSelf;
                cameraPrefab.gameObject.SetActive(false);

                CameraConfigContext instance = Instantiate(cameraPrefab, transform, false) as CameraConfigContext;

                instance.Data = createCams[i];

                CameraInstances.Add(instance);

                cameraPrefab.gameObject.SetActive(wasPrefabActive);

                instance.gameObject.SetActive(MixCastSdk.Active);
            }

            destroyCams.Clear();
            createCams.Clear();
        }

        void GenerateCameras()
        {
            CameraInstances = new List<CameraConfigContext>();

            bool wasPrefabActive = cameraPrefab.gameObject.activeSelf;
            cameraPrefab.gameObject.SetActive(false);
            for (int i = 0; i < MixCast.Settings.cameras.Count; i++)
            {
                CameraConfigContext instance = Instantiate(cameraPrefab, transform, false) as CameraConfigContext;

                instance.transform.localPosition = Vector3.zero;
                instance.transform.localRotation = Quaternion.identity;
                instance.transform.localScale = Vector3.one;

                instance.Data = MixCast.Settings.cameras[i];

                CameraInstances.Add(instance);
            }
            cameraPrefab.gameObject.SetActive(wasPrefabActive);
        }
        void DestroyCameras()
        {
            for (int i = 0; i < CameraInstances.Count; i++)
            {
                Destroy(CameraInstances[i].gameObject);
            }

            CameraInstances.Clear();
            CameraInstances = null;
        }

        void SetCamerasActive(bool active)
        {
            if (CameraInstances == null) { return; }

            for (int i = 0; i < CameraInstances.Count; i++)
            {
                CameraInstances[i].gameObject.SetActive(active);
            }
        }
    }
}
#endif
