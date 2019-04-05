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
using UnityEngine;
using UnityEngine.Rendering;

namespace BlueprintReality.MixCast
{
    //Place at the root of the camera hierarchy.
    public class BufferedMixCastCamera : MixCastCamera
    {
        public const string DEPTH_WRITE_SHADER = "Hidden/BPR/DepthWrite";
        public const string FIX_FG_SHADOW_SHADER = "Hidden/BPR/ScreenSpaceShadows NoFade";
        public const string BLIT_BACKGROUND_SHADER = "Hidden/BPR/Background Blit";
        public const string BLIT_FOREGROUND_SHADER = "Hidden/BPR/Foreground Blit";
        public const string BLIT_FOREGROUND_PMA_SHADER = "Hidden/BPR/Foreground Blit PMA";
        public const string COPY_ALPHA_CHANNEL_SHADER = "Hidden/BPR/AlphaTransfer";

        [System.Serializable]
        public class GameFrame
        {
            public float playerDist;

            public RenderTexture foregroundBuffer;
            public RenderTexture backgroundBuffer;

            public Matrix4x4 cameraToWorld;
            public Matrix4x4 cameraProjection;

            public void Release()
            {
                if (backgroundBuffer != null)
                    backgroundBuffer.Release();
                if (foregroundBuffer != null)
                    foregroundBuffer.Release();
                backgroundBuffer = null;
                foregroundBuffer = null;
            }
        }

        public float layerOverlap = 0f;
        public LayerMask forceToBackground = 0;

        public event System.Action BackgroundRendered;

        private FrameDelayQueue<GameFrame> frames;
        private RenderTexture renderTarget;
        public RenderTexture LastFrameAlpha { get; protected set; }

        public RenderTexture RenderedForeground;
        public RenderTexture RenderedBackground;

        private Shader blockoutBackgroundShader;
        private Shader noForegroundShadowFadeShader;

        private Material blitBackgroundMat;
        private Material blitForegroundMat;
        private Material postBlit;

        private CommandBuffer grabAlphaCommand;
        private Material copyAlphaMat;

        private CommandBuffer renderProjectionCommand;

        private Camera cleanProxyCamera;
        private CommandBuffer finalizeFrameCommand;

        protected void Awake()
        {
            blockoutBackgroundShader = Shader.Find(DEPTH_WRITE_SHADER);
            noForegroundShadowFadeShader = Shader.Find(FIX_FG_SHADOW_SHADER);
            blitBackgroundMat = new Material(Shader.Find(BLIT_BACKGROUND_SHADER));
            if (!MixCastSdkData.ProjectSettings.usingPMA)
                blitForegroundMat = new Material(Shader.Find(BLIT_FOREGROUND_SHADER));
            else
                blitForegroundMat = new Material(Shader.Find(BLIT_FOREGROUND_PMA_SHADER));
            postBlit = new Material(Shader.Find("Hidden/BPR/AlphaWrite"));

            grabAlphaCommand = new CommandBuffer();
            grabAlphaCommand.name = "Get Real Alpha";
            copyAlphaMat = new Material(Shader.Find(COPY_ALPHA_CHANNEL_SHADER));

            renderProjectionCommand = new CommandBuffer();
            renderProjectionCommand.name = "Render Input Projection";

            GameObject cleanProxyObj = new GameObject("Clean Camera");
            cleanProxyObj.hideFlags = HideFlags.HideAndDontSave;
            cleanProxyObj.transform.SetParent(gameCamera.transform);
            cleanProxyObj.transform.localPosition = Vector3.zero;
            cleanProxyObj.transform.localRotation = Quaternion.identity;
            cleanProxyObj.transform.localScale = Vector3.one;
            cleanProxyCamera = cleanProxyObj.AddComponent<Camera>();
            cleanProxyCamera.enabled = false;

            finalizeFrameCommand = new CommandBuffer();
            finalizeFrameCommand.name = "Finalize Frame";
        }

        protected override void BuildOutput()
        {
            base.BuildOutput();

            frames = new FrameDelayQueue<GameFrame>();
            frames.delayDuration = context.Data.bufferTime;
            frames.AllocateFrames();

            if (MixCastSdkData.ProjectSettings.grabUnfilteredAlpha)
            {
                LastFrameAlpha = new RenderTexture(Output.width, Output.height, 0)
                {
                    useMipMap = false,
#if UNITY_5_5_OR_NEWER
                    autoGenerateMips = false,
#else
                    generateMips = false,
#endif
                };
                LastFrameAlpha.Create();
                grabAlphaCommand.Blit(BuiltinRenderTextureType.CurrentActive, LastFrameAlpha);
            }
            else
                LastFrameAlpha = null;

            renderTarget = new RenderTexture(Output.width, Output.height, 24, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear)
            {
                antiAliasing = CalculateAntiAliasingValue(),
                useMipMap = false,
#if UNITY_5_5_OR_NEWER
                autoGenerateMips = false,
#else
                generateMips = false,
#endif
            };

#if UNITY_5_5_OR_NEWER
            finalizeFrameCommand.Blit(Output as RenderTexture, BuiltinRenderTextureType.CurrentActive);
#else
            finalizeFrameCommand.Blit(Output as Texture, BuiltinRenderTextureType.CurrentActive);
#endif
        }
        protected override void ReleaseOutput()
        {
            frames.unusedFrames.ForEach(f =>
            {
                if (f.data != null)
                {
                    f.data.Release();
                }
            });
            frames.usedFrames.ForEach(f =>
            {
                if (f.data != null)
                {
                    f.data.Release();
                }
            });
            frames = null;

            if (LastFrameAlpha != null)
            {
                LastFrameAlpha.Release();
                LastFrameAlpha = null;
                grabAlphaCommand.Clear();
            }
            if (renderTarget != null)
            {
                renderTarget.Release();
                renderTarget = null;
            }

            finalizeFrameCommand.Clear();

            base.ReleaseOutput();
        }

        void OnGUI()
        {
            //magic declaration that causes rendering to work
        }


        public override void RenderScene()
        {
            StartFrame();

            frames.delayDuration = context.Data.unplugged == false ? context.Data.bufferTime : 0f;
            frames.Update();

            GameFrame newFrame = PrepareNewFrame();

            newFrame.playerDist = gameCamera.transform.TransformVector(Vector3.forward).magnitude * CalculatePlayerDistance();  //Scale distance by camera scale
            newFrame.cameraToWorld = cleanProxyCamera.cameraToWorldMatrix;
            newFrame.cameraProjection = cleanProxyCamera.projectionMatrix;

            cleanProxyCamera.CopyFrom(gameCamera);
            RenderBackground(newFrame);
            RenderForeground(newFrame);

            GameFrame bufferedFrame = frames.OldestFrameData;

            GL.LoadIdentity();
            ClearBuffer();
            BlitBackground(bufferedFrame);
            if (BackgroundRendered != null)
                BackgroundRendered();

            //Update shader properties for Feeds to access
            Shader.SetGlobalFloat("_CamNear", cleanProxyCamera.nearClipPlane);
            Shader.SetGlobalFloat("_CamFar", cleanProxyCamera.farClipPlane);
            Shader.SetGlobalMatrix("_WorldToCam", bufferedFrame.cameraToWorld.inverse);
            Shader.SetGlobalMatrix("_CamToWorld", bufferedFrame.cameraToWorld);
            Shader.SetGlobalMatrix("_CamProjection", bufferedFrame.cameraProjection);

            if (!context.Data.unplugged)
            {
                for (int i = 0; i < InputFeedProjection.ActiveProjections.Count; i++)
                {
                    if (InputFeedProjection.ActiveProjections[i].context.Data == context.Data)
                    {
                        InputFeed feed = InputFeedProjection.ActiveProjections[i].FindFeed();
                        if (feed != null)
                        {
                            feed.StartRender();
                            RenderInputProjection(InputFeedProjection.ActiveProjections[i]);
                            feed.StopRender();
                        }
                    }
                }
            }

            BlitForeground(bufferedFrame);

            Graphics.SetRenderTarget(Output as RenderTexture);
            Graphics.Blit(null, postBlit);
            RenderFinalPass();
            CompleteFrame();

        }

        //Gets an empty frame from the pool and ensures its ready to be filled
        GameFrame PrepareNewFrame()
        {
            FrameDelayQueue<GameFrame>.Frame<GameFrame> newFrame = frames.GetNewFrame();
            if (newFrame.data == null)
                newFrame.data = new GameFrame();

            if (newFrame.data.backgroundBuffer != null && (newFrame.data.backgroundBuffer.width != Output.width || newFrame.data.backgroundBuffer.height != Output.height))
            {
                newFrame.data.backgroundBuffer.Release();
                newFrame.data.backgroundBuffer = null;
            }
            if (newFrame.data.backgroundBuffer == null)
            {
                newFrame.data.backgroundBuffer = new RenderTexture(Output.width, Output.height, 24, (Output as RenderTexture).format, (Output as RenderTexture).sRGB ? RenderTextureReadWrite.sRGB : RenderTextureReadWrite.Linear);
            }

            if (newFrame.data.foregroundBuffer != null && (newFrame.data.foregroundBuffer.width != Output.width || newFrame.data.foregroundBuffer.height != Output.height))
            {
                newFrame.data.foregroundBuffer.Release();
                newFrame.data.foregroundBuffer = null;
            }
            if (newFrame.data.foregroundBuffer == null)
            {
                newFrame.data.foregroundBuffer = new RenderTexture(Output.width, Output.height, 24, (Output as RenderTexture).format, (Output as RenderTexture).sRGB ? RenderTextureReadWrite.sRGB : RenderTextureReadWrite.Linear);
            }

            return newFrame.data;
        }

        void ClearBuffer()
        {
            Graphics.SetRenderTarget(Output as RenderTexture);
            GL.Clear(true, true, Color.black);
            //Graphics.SetRenderTarget(null);
        }

        void RenderBackground(GameFrame targetFrame)
        {
            float oldNearClip = cleanProxyCamera.nearClipPlane;
            bool oldOC = cleanProxyCamera.useOcclusionCulling;

            cleanProxyCamera.nearClipPlane = Mathf.Clamp(targetFrame.playerDist - 0.5f * layerOverlap, 0.001f, cleanProxyCamera.farClipPlane - 0.001f);
            cleanProxyCamera.useOcclusionCulling = false;

            RenderGameCamera(cleanProxyCamera, renderTarget);

            if (forceToBackground > 0)
            {
                float oldFarClip = cleanProxyCamera.farClipPlane;
                LayerMask oldCull = cleanProxyCamera.cullingMask;
                CameraClearFlags oldClear = cleanProxyCamera.clearFlags;

                cleanProxyCamera.farClipPlane = targetFrame.playerDist + 0.5f * layerOverlap;
                cleanProxyCamera.nearClipPlane = oldNearClip;
                cleanProxyCamera.cullingMask = forceToBackground;
                cleanProxyCamera.clearFlags = CameraClearFlags.Depth;

                RenderGameCamera(cleanProxyCamera, renderTarget);

                cleanProxyCamera.cullingMask = oldCull;
                cleanProxyCamera.farClipPlane = oldFarClip;
                cleanProxyCamera.clearFlags = oldClear;
            }

            cleanProxyCamera.nearClipPlane = oldNearClip;
            cleanProxyCamera.useOcclusionCulling = oldOC;

            Graphics.Blit(renderTarget, targetFrame.backgroundBuffer);
        }

        void RequestRenderTexture(RenderTexture src, ref RenderTexture request)
        {
            if(request == null)
            {
                request = new RenderTexture(src.width, src.height, src.depth, src.format, src.sRGB ? RenderTextureReadWrite.sRGB : RenderTextureReadWrite.Linear);
            }
            else if(request.width != src.width || request.height != src.height)
            {
                request.Release();
                request = new RenderTexture(src.width, src.height, src.depth, src.format, src.sRGB ? RenderTextureReadWrite.sRGB : RenderTextureReadWrite.Linear);
            }
        }

        void BlitBackground(GameFrame frame)
        {
            RenderTexture src = frame.backgroundBuffer;

            bool oldSRGB = GL.sRGBWrite;
            GL.sRGBWrite = false;

            Graphics.SetRenderTarget(Output as RenderTexture);
            Graphics.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), src, blitBackgroundMat);

            RequestRenderTexture(src, ref RenderedBackground);
            Graphics.Blit(src, RenderedBackground);

            GL.sRGBWrite = oldSRGB;
        }

        void RenderForeground(GameFrame targetFrame)
        {
            float oldFarClip = cleanProxyCamera.farClipPlane;
            LayerMask oldCull = cleanProxyCamera.cullingMask;
            CameraClearFlags oldClear = cleanProxyCamera.clearFlags;
            Color oldBGCol = cleanProxyCamera.backgroundColor;
            bool oldOC = cleanProxyCamera.useOcclusionCulling;

            Color blackAndClear = new Color(0, 0, 0, 0);

            Graphics.SetRenderTarget(renderTarget);
            GL.Clear(true, true, blackAndClear);

            cleanProxyCamera.farClipPlane = Mathf.Max(cleanProxyCamera.nearClipPlane + 0.001f, targetFrame.playerDist + 0.5f * layerOverlap);
            cleanProxyCamera.clearFlags = CameraClearFlags.Color | CameraClearFlags.Depth;   //Just cleared
            cleanProxyCamera.backgroundColor = blackAndClear;
            cleanProxyCamera.useOcclusionCulling = false;

            if (forceToBackground > 0)
            {
                cleanProxyCamera.cullingMask = forceToBackground;
                //TODO: optimize by disabling all lighting in this render
                cleanProxyCamera.targetTexture = renderTarget;
                cleanProxyCamera.RenderWithShader(blockoutBackgroundShader, null);

                //now depth buffer wont allow writes where the ground would have been
                cleanProxyCamera.clearFlags = CameraClearFlags.Nothing;
            }

            Shader originalScreenSpaceShadowShader = GraphicsSettings.GetCustomShader(BuiltinShaderType.ScreenSpaceShadows);
            GraphicsSettings.SetCustomShader(BuiltinShaderType.ScreenSpaceShadows, noForegroundShadowFadeShader);

            cleanProxyCamera.cullingMask = oldCull ^ forceToBackground;

            if (MixCastSdkData.ProjectSettings.grabUnfilteredAlpha)
                cleanProxyCamera.AddCommandBuffer(CameraEvent.BeforeImageEffects, grabAlphaCommand);  //Instruction to copy out the state of the RenderTexture before Image Effects are applied

            RenderGameCamera(cleanProxyCamera, renderTarget);

            if (MixCastSdkData.ProjectSettings.grabUnfilteredAlpha)
            {
                cleanProxyCamera.RemoveCommandBuffer(CameraEvent.BeforeImageEffects, grabAlphaCommand);
                Graphics.Blit(LastFrameAlpha, renderTarget, copyAlphaMat);      //Overwrite the potentially broken post-effects alpha channel with the pre-effect copy
            }
            else
                LastFrameAlpha = renderTarget;

            GraphicsSettings.SetCustomShader(BuiltinShaderType.ScreenSpaceShadows, originalScreenSpaceShadowShader);

            cleanProxyCamera.farClipPlane = oldFarClip;
            cleanProxyCamera.cullingMask = oldCull;
            cleanProxyCamera.clearFlags = oldClear;
            cleanProxyCamera.backgroundColor = oldBGCol;
            cleanProxyCamera.useOcclusionCulling = oldOC;

            Graphics.Blit(renderTarget, targetFrame.foregroundBuffer);
        }
        void BlitForeground(GameFrame frame)
        {
            if (frame.playerDist > 0)
            {
                bool oldSRGB = GL.sRGBWrite;
                GL.sRGBWrite = true;

                RenderTexture src = frame.foregroundBuffer;

                Graphics.SetRenderTarget(Output as RenderTexture);
                Graphics.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), src, blitForegroundMat);

                RequestRenderTexture(src, ref RenderedForeground);
                Graphics.Blit(src, RenderedForeground);

                GL.sRGBWrite = oldSRGB;
            }
            else
            {
                if (RenderedForeground != null)
                {
                    RenderedForeground.Release();
                    RenderedForeground = null;
                }
            }
        }

        void RenderInputProjection(InputFeedProjection feedProjection)
        {
            if (!feedProjection.readyToRender)
                return;

            int oldCullingMask = cleanProxyCamera.cullingMask;
            CameraClearFlags oldClearFlags = cleanProxyCamera.clearFlags;

            cleanProxyCamera.cullingMask = 0;
            cleanProxyCamera.clearFlags = CameraClearFlags.Depth;
            cleanProxyCamera.targetTexture = Output as RenderTexture;

            renderProjectionCommand.DrawRenderer(feedProjection.MeshRenderer, feedProjection.MeshRenderer.sharedMaterial);
            cleanProxyCamera.AddCommandBuffer(CameraEvent.AfterEverything, renderProjectionCommand);

            cleanProxyCamera.Render();

            cleanProxyCamera.RemoveCommandBuffer(CameraEvent.AfterEverything, renderProjectionCommand);
            renderProjectionCommand.Clear();

            cleanProxyCamera.cullingMask = oldCullingMask;
            cleanProxyCamera.clearFlags = oldClearFlags;
            cleanProxyCamera.targetTexture = null;
        }

        void RenderFinalPass()
        {
            int oldCulling = gameCamera.cullingMask;
            CameraClearFlags oldClear = gameCamera.clearFlags;
            float oldFarClip = gameCamera.farClipPlane;
            float oldNearClip = gameCamera.nearClipPlane;
            RenderingPath oldRenderPath = gameCamera.renderingPath;
            bool oldOC = gameCamera.useOcclusionCulling;

            gameCamera.cullingMask = 0;
            gameCamera.clearFlags = CameraClearFlags.Depth;
            gameCamera.farClipPlane = 1;
            gameCamera.nearClipPlane = 0.1f;
            gameCamera.renderingPath = RenderingPath.DeferredShading;
            gameCamera.useOcclusionCulling = false;

            gameCamera.AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, finalizeFrameCommand);

            gameCamera.targetTexture = Output as RenderTexture;
            gameCamera.aspect = (float)Output.width / Output.height;
            gameCamera.Render();

            gameCamera.targetTexture = null;

            gameCamera.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, finalizeFrameCommand);

            gameCamera.cullingMask = oldCulling;
            gameCamera.clearFlags = oldClear;
            gameCamera.farClipPlane = oldFarClip;
            gameCamera.nearClipPlane = oldNearClip;
            gameCamera.renderingPath = oldRenderPath;
            gameCamera.useOcclusionCulling = oldOC;
        }
    }
}
#endif
