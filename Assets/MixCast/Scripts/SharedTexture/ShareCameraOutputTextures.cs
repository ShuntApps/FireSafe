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

namespace BlueprintReality.MixCast.SharedTexture
{
    public class ShareCameraOutputTextures : CameraComponent
    {
        public MixCastCamera cam;

        SharedTextureSender foregroundSlot = new SharedTextureSender();
        SharedTextureSender backgroundSlot = new SharedTextureSender();
        SharedTextureSender compositedSlot = new SharedTextureSender(true);

        protected override void OnEnable()
        {
            base.OnEnable();

            if (cam == null)
            {
                cam = GetComponentInParent<MixCastCamera>();
            }

            MixCastCamera.FrameEnded += HandleFrameEnded;
        }

        protected override void OnDisable()
        {
            foregroundSlot.Clear();
            backgroundSlot.Clear();
            compositedSlot.Clear();

            MixCastCamera.FrameEnded -= HandleFrameEnded;
        }

        private void HandleFrameEnded(MixCastCamera renderingCam)
        {
            if (renderingCam != cam)
                return;

            if(cam is BufferedMixCastCamera)
            {
                foregroundSlot.UpdateFromTexture(cam.context.Data.id + "_foreground", (cam as BufferedMixCastCamera).RenderedForeground);
                backgroundSlot.UpdateFromTexture(cam.context.Data.id + "_background", (cam as BufferedMixCastCamera).RenderedBackground);
            }

            compositedSlot.UpdateFromTexture(cam.context.Data.id + "_composited", cam.Output);
        }
    }
}
#endif
