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
using UnityEditor;
using UnityEngine;

namespace BlueprintReality.MixCast {
	public class DebugFGAlphaWindow : EditorWindow {
        Material drawAlphaMat;
        int cameraIndex = -1;

        [MenuItem("MixCast/Debug Foreground Transparency", priority = 6)]
        static void Create()
        {
            DebugFGAlphaWindow window = (DebugFGAlphaWindow)EditorWindow.GetWindow(typeof(DebugFGAlphaWindow));
            window.titleContent = new GUIContent("FG Alpha Preview");
            window.Show();
            window.autoRepaintOnSceneChange = true;
        }

        private void OnGUI()
        {
            if(drawAlphaMat == null )
            {
                drawAlphaMat = new Material(Shader.Find("Hidden/BPR/AlphaOut"));
            }
            Rect fullRect = new Rect(Vector2.zero, position.size);
            if ( Application.isPlaying && MixCast.Settings != null )
            {
                string[] cameraNames = new string[MixCast.Settings.cameras.Count];
                for( int i = 0; i < cameraNames.Length; i++ )
                {
                    cameraNames[i] = "Camera " + i;
                }
                cameraIndex = Mathf.Clamp(cameraIndex, 0, cameraNames.Length - 1);
                cameraIndex = EditorGUILayout.Popup(cameraIndex, cameraNames);
                for( int i = 0; i < MixCastCamera.ActiveCameras.Count; i++ )
                {
                    if( MixCastCamera.ActiveCameras[i].context.Data == MixCast.Settings.cameras[cameraIndex] )
                    {
                        if( MixCastCamera.ActiveCameras[i] is BufferedMixCastCamera )
                        {
                            BufferedMixCastCamera buffCam = MixCastCamera.ActiveCameras[i] as BufferedMixCastCamera;
                            Texture foregroundTex = buffCam.LastFrameAlpha;
                            if (foregroundTex != null)
                            {
#if UNITY_5_5_OR_NEWER
                                Graphics.DrawTexture(fullRect, foregroundTex, drawAlphaMat, 0);
#else
                                Graphics.DrawTexture(fullRect, foregroundTex, drawAlphaMat);
#endif
                            }
                        }
                        else
                        {
                            EditorGUILayout.HelpBox("Debugging can only occur with a camera with video latency compensation above 0 (buffered mode)", MessageType.Error);
                        }

                        break;
                    }
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Debugging can only occur while the application is running and MixCast is active", MessageType.Warning);
            }
        }
    }
}
#endif
