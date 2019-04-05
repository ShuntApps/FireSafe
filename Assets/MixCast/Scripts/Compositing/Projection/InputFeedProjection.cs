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

namespace BlueprintReality.MixCast {
    [RequireComponent(typeof(SetTransformFromCameraSettings))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
	public class InputFeedProjection : CameraComponent {
        public const string ANTI_ALIASING_SHADER_KEYWORD = "AA_ON";
        private const float NEAR_PLANE_PADDING = 0.01f; //Projection can only get as close to the camera as 101% of the near clip plane

        public static List<InputFeedProjection> ActiveProjections { get; protected set; }
        public static InputFeedProjection FindProjection(CameraConfigContext context)
        {
            for (int i = 0; i < ActiveProjections.Count; i++)
            {
                var proj = ActiveProjections[i];
                if (proj.context.Data == context.Data)
                {
                    return proj;
                }
            }
            return null;
        }
        static InputFeedProjection()
        {
            ActiveProjections = new List<InputFeedProjection>();
        }

        public string textureProperty = "_MainTex";

        private Mesh projectionMesh;
        private List<Vector3> vertBuffer = new List<Vector3>();

        public MeshRenderer MeshRenderer { get; protected set; }
        public bool readyToRender;

        public InputFeed FindFeed()
        {
            if (context.Data == null)
                return null;

            for (int i = 0; i < InputFeed.ActiveFeeds.Count; i++)
                if (InputFeed.ActiveFeeds[i].context.Data == context.Data)
                    return InputFeed.ActiveFeeds[i];
            return null;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            Camera.onPreCull += HandleRenderStarted;
            Camera.onPostRender += HandleRenderEnded;

            InitializeMesh();
            MeshRenderer = GetComponent<MeshRenderer>();

            ActiveProjections.Add(this);

#if UNITY_EDITOR
            InvokeProtector.InvokeRepeating(RefreshSceneCameras, 0, 1);
#endif
        }
        protected override void OnDisable()
        {
            base.OnDisable();

            ActiveProjections.Remove(this);

            Camera.onPreCull -= HandleRenderStarted;
            Camera.onPostRender -= HandleRenderEnded;

            GetComponent<MeshFilter>().sharedMesh = null;
            MeshRenderer.sharedMaterial.SetTexture(textureProperty, null);

#if UNITY_EDITOR
            InvokeProtector.CancelInvokeRepeating(RefreshSceneCameras);
#endif
        }

        void Update ()
        {
            InputFeed feed = FindFeed();
            readyToRender = feed != null && feed.isActiveAndEnabled && feed.ProcessedTexture != null;
            if (!readyToRender)
                return;

            UpdateMesh(feed);
            MeshRenderer.sharedMaterial.SetTexture(textureProperty, feed.ProcessedTexture);
            bool usingAA = MeshRenderer.sharedMaterial.IsKeywordEnabled(ANTI_ALIASING_SHADER_KEYWORD);
            if ( feed.context.Data.antiAliasSegmentationResults )
            {
                if (!usingAA)
                    MeshRenderer.sharedMaterial.EnableKeyword(ANTI_ALIASING_SHADER_KEYWORD);
            }
            else
            {
                if (usingAA)
                    MeshRenderer.sharedMaterial.DisableKeyword(ANTI_ALIASING_SHADER_KEYWORD);

            }
        }

#if UNITY_EDITOR
        private List<Camera> cachedSceneCameras = new List<Camera>();
        void RefreshSceneCameras()
        {
            cachedSceneCameras.Clear();
            cachedSceneCameras.AddRange(UnityEditor.SceneView.GetAllSceneCameras());
        }
#endif

        void HandleRenderStarted(Camera cam)
        {
            if (!readyToRender)
            {
                MeshRenderer.enabled = false;
                return;
            }

            bool isSceneCam = false;
#if UNITY_EDITOR
            isSceneCam = cachedSceneCameras.Contains(cam);
#endif

            if( isSceneCam )
            {
                MeshRenderer.enabled = MixCastSdkData.ProjectSettings.displaySubjectInScene;
            }
            else if ( MixCastCamera.Current != null )
            {
                if( MixCastCamera.Current.context.Data == context.Data )
                    MeshRenderer.enabled = MixCastCamera.Current is ImmediateMixCastCamera;
                else
                    MeshRenderer.enabled = context.Data.projectionData.displayToOtherCams;
            }
            else if (cam.stereoTargetEye != StereoTargetEyeMask.None)
            {
                MeshRenderer.enabled = context.Data.projectionData.displayToHeadset;
            }
            else
            {
                MeshRenderer.enabled = false;
            }

            if (MeshRenderer.enabled)
            {
                InputFeed feed = FindFeed();
                
                if( feed != null && feed.ShouldRender ) {
                    UpdateMesh(feed);
                    feed.StartRender();
                } else {
                    MeshRenderer.enabled = false; // gets re-enabled automatically when device is plugged back in.
                }
            }
        }
        void HandleRenderEnded(Camera cam)
        {
            if (MeshRenderer.enabled)
            {
                InputFeed feed = FindFeed();
                if (feed != null && feed.ShouldRender)
                    feed.StopRender();
            }
        }

        void InitializeMesh()
        {
            if (projectionMesh != null)
                Destroy(projectionMesh);

            projectionMesh = new Mesh();
            projectionMesh.MarkDynamic();

            vertBuffer = new List<Vector3>();
            for (int i = 0; i < 4; i++)
                vertBuffer.Add(Vector3.zero);
            projectionMesh.SetVertices(vertBuffer);

            List<Vector2> uvs = new List<Vector2>();
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(0, 0));
            projectionMesh.SetUVs(0, uvs);

            projectionMesh.SetIndices(new int[] { 0, 1, 2, 3 }, MeshTopology.Quads, 0);

            GetComponent<MeshFilter>().sharedMesh = projectionMesh;
        }
        void UpdateMesh(InputFeed feed)
        {
            MixCastCamera cam = MixCastCamera.FindCamera(feed.context);
            if (cam == null)
                return;

            float playerDist = Mathf.Max(cam.gameCamera.nearClipPlane * (1f + NEAR_PLANE_PADDING), cam.CalculatePlayerDistance());
            float playerQuadHalfHeight = playerDist * Mathf.Tan(feed.context.Data.deviceFoV * 0.5f * Mathf.Deg2Rad);
            float playerQuadHalfWidth = playerQuadHalfHeight * feed.context.Data.deviceFeedWidth / feed.context.Data.deviceFeedHeight;

            vertBuffer[0] = new Vector3(-playerQuadHalfWidth, playerQuadHalfHeight, playerDist);
            vertBuffer[1] = new Vector3(playerQuadHalfWidth, playerQuadHalfHeight, playerDist);
            vertBuffer[2] = new Vector3(playerQuadHalfWidth, -playerQuadHalfHeight, playerDist);
            vertBuffer[3] = new Vector3(-playerQuadHalfWidth, -playerQuadHalfHeight, playerDist);

            projectionMesh.SetVertices(vertBuffer);
            projectionMesh.RecalculateBounds();
            projectionMesh.UploadMeshData(false);
        }
	}
}
#endif
