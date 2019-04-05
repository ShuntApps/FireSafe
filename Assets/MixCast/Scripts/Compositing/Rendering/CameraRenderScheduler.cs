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

namespace BlueprintReality.MixCast
{
    /// <summary>
    /// The component responsible for driving the creation of frames by MixCast cameras
    /// </summary>
    public class CameraRenderScheduler : MonoBehaviour
    {
        public static event System.Action OnBeforeRender;

        protected Transform lastParent;

        private float nextRenderTime;
        private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        private Coroutine renderUsedCamerasRoutine, renderSpareCamerasRoutine;

        private void OnEnable()
        {
            renderUsedCamerasRoutine = StartCoroutine(RenderUsedCameras());
            renderSpareCamerasRoutine = StartCoroutine(RenderSpareCameras());

            nextRenderTime = Time.unscaledTime + 1f / MixCast.Settings.global.targetFramerate;
        }
        private void OnDisable()
        {
            StopCoroutine(renderUsedCamerasRoutine);
            StopCoroutine(renderSpareCamerasRoutine);
        }

        IEnumerator RenderUsedCameras()
        {
            while (isActiveAndEnabled)
            {
                if (Time.unscaledTime >= nextRenderTime)
                {
                    if (OnBeforeRender != null)
                        OnBeforeRender();

                    for (int i = 0; i < MixCastCamera.ActiveCameras.Count; i++)
                    {
                        MixCastCamera cam = MixCastCamera.ActiveCameras[i];
                        if (cam.IsRunningInRealtime)
                            cam.RenderScene();
                    }

                    nextRenderTime += 1f / MixCast.Settings.global.targetFramerate;
                }

                yield return waitForEndOfFrame;
            }
        }

        IEnumerator RenderSpareCameras()
        {
            int lastSpareRenderedIndex = 0;
            while (isActiveAndEnabled)
            {
                if (MixCastCamera.ActiveCameras.Count > 0)
                {
                    int startIndex = lastSpareRenderedIndex;
                    lastSpareRenderedIndex++;
                    while (MixCastCamera.ActiveCameras[lastSpareRenderedIndex % MixCastCamera.ActiveCameras.Count].IsRunningInRealtime && (lastSpareRenderedIndex - startIndex) <= MixCastCamera.ActiveCameras.Count)
                        lastSpareRenderedIndex++;

                    if (lastSpareRenderedIndex - startIndex <= MixCastCamera.ActiveCameras.Count)
                        MixCastCamera.ActiveCameras[lastSpareRenderedIndex % MixCastCamera.ActiveCameras.Count].RenderScene();
                }

                for (int i = 0; i < MixCast.Settings.global.framesPerSpareRender; i++)
                {
                    yield return waitForEndOfFrame;
                }
            }
        }
    }
}
#endif
