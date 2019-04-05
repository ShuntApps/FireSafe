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
using BlueprintReality.MixCast.Thrift;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace BlueprintReality.MixCast.SharedTexture
{
    public partial class SharedTextureManager : MonoBehaviour
    {
        static bool initialized = false;

        private static void EnsureInitialize()
        {
            if (!initialized)
            {
                var go = new GameObject("Shared Texture Manager");
                DontDestroyOnLoad(go);
                var instance = go.AddComponent<SharedTextureManager>();
                instance.Initialize();
                initialized = true;
            }
        }

        private class RegisteredLocalTexture
        {
            public Texture source;
            public Texture destination;
            public string texId;
        }

        [DllImport("ExTexture")]
        private static extern IntPtr GetRenderEventFunc();

        private void Initialize()
        {
            StartCoroutine("CallCopyLocalTextures");
            StartCoroutine("CallPluginAtEndOfFrames");
        }

        private IEnumerator CallPluginAtEndOfFrames()
        {
            while (true)
            {
                // Wait until all frame rendering is done
                yield return new WaitForEndOfFrame();

                GL.IssuePluginEvent(GetRenderEventFunc(), 1);
            }
        }

        private static Dictionary<string, RegisteredLocalTexture> registeredLocalTextures = new Dictionary<string, RegisteredLocalTexture>();

        private IEnumerator CallCopyLocalTextures()
        {
            while(true)
            {
                CopyLocalTextures();
                yield return null;
            }
        }

        private void CopyLocalTextures()
        {
            foreach(var label in registeredLocalTextures.Values)
            {
                if(label.source != null && label.destination != null)
                {
                    Graphics.CopyTexture(label.source, label.destination);
                }
            }
        }

        private static bool CheckFormat(int format, ref TextureFormat convFmt)
        {
            if (format == 28)
            {
                convFmt = TextureFormat.RGBA32;
                return true;
            }
            else if (format == 87)
            {
                convFmt = TextureFormat.BGRA32;
                return true;
            }
            else if (format == 56)
            {
                convFmt = TextureFormat.R16;
                return true;
            }
            else if (format == 2)
            {
                convFmt = TextureFormat.RGBAFloat;
                return true;
            }
            else if (format == 24)
            {
                //Debug.LogWarning("Unreal Back Buffer Format");
                convFmt = TextureFormat.RGBA32;
                return false;
            }
            else
            {
                // Add more pixel format support
                Debug.LogError("Unsupported Pixel Format:" + format + "!!!");
                convFmt = TextureFormat.RGBA32;
                return false;
            }
        }

        [DllImport("ExTexture")]
        private static extern IntPtr CreateSharedTexture(IntPtr texture, ref int width, ref int height, ref int format, ref ulong handle);

        private static SharedTex getSharedTex(Texture texture, out IntPtr srv)
        {
            srv = IntPtr.Zero;

            if (texture == null)
            {
                return null;
            }

            int width = 0;
            int height = 0;
            int format = 0;
            ulong handle = 0;

            try
            {
                srv = CreateSharedTexture(texture.GetNativeTexturePtr(), ref width, ref height, ref format, ref handle);
            }
            catch
            {
                Debug.LogError("Register Local Texture failed");
                return null;
            }
            if (srv == IntPtr.Zero || handle <= 0)
            {
                Debug.LogError("Register Local Texture failed");
                return null;
            }
            return new SharedTex((long)handle, width, height, format);
        }

        public static string RegisterLocal(string texId, Texture texture)
        {
            EnsureInitialize();
            IntPtr srv = IntPtr.Zero;
            SharedTex tex = getSharedTex(texture, out srv);
            bool result = (tex != null) && (srv != IntPtr.Zero) && UnityThriftMixCastClient.Get<SharedTextureCommunication.Client>().TrySharedTextureNotify(texId, tex);
            if(result)
            {
                TextureFormat convFormat = TextureFormat.RGBA32;
                CheckFormat(tex.Format, ref convFormat);
                Texture newDst = Texture2D.CreateExternalTexture(tex.Width, tex.Height, convFormat, false, QualitySettings.activeColorSpace == ColorSpace.Linear, srv);
                RegisteredLocalTexture label = new RegisteredLocalTexture();
                label.source = texture;
                label.destination = newDst;
                label.texId = texId;
                string newId = Guid.NewGuid().ToString();
                registeredLocalTextures.Add(newId, label);
                return newId;
            }
            else
            {
                return null;
            }
        }

        public static bool UnregisterLocal(string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            EnsureInitialize();

            if(registeredLocalTextures.ContainsKey(id))
            {
                var label = registeredLocalTextures[id];
                registeredLocalTextures.Remove(id);
                return UnityThriftMixCastClient.Get<SharedTextureCommunication.Client>().TrySharedTextureNotify(label.texId, new SharedTex(0, 0, 0, 0));
            }
            else
            {
                Debug.LogError("No Local Texture Id is Found!");
                return false;
            }
        }
    }
}
#endif
