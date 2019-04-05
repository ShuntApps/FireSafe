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
namespace BlueprintReality.MixCast
{
	public class SetTextFromCameraDisplayName : CameraComponent
    {
        public UnityEngine.UI.Text text;

        private void Update()
        {
            if (context.Data == null)
                return;

            if (context.Data.displayName != text.text)
                text.text = context.Data.displayName;
        }
        
    }
}
#endif
