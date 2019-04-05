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
using System;
using Intel.RealSense;
using UnityEngine;

namespace BlueprintReality.MixCast.RealSense
{
    public abstract class RsFrameProvider : MonoBehaviour
    {
        public bool Streaming { get; protected set; }
        public PipelineProfile ActiveProfile { get; protected set; }

        public abstract event Action<PipelineProfile> OnStart;
        public abstract event Action OnStop;
        public abstract event Action<Frame> OnNewSample;
    }
}
#endif
