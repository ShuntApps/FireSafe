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
namespace BlueprintReality.MixCast.Thrift
{
    public partial class Vector3
    {
        public UnityEngine.Vector3 unity
        {
            get { return new UnityEngine.Vector3((float)X, (float)Y, (float)Z); }
            set
            {
                X = value.x;
                Y = value.y;
                Z = value.z;
            }
        }

        public override bool Equals(object obj) 
        {
            var other = obj as Vector3;

            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return unity == other.unity;
        }

        public override int GetHashCode()
        {
            return unity.GetHashCode();
        }
    }
}
#endif