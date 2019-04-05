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
    public class SdkVirtualTrackedObjectManager
    {
        public List<CustomTrackedObjectBehaviour> VirtualTrackedObjectBehaviours { get; protected set; }

        private List<Data.TrackedObject> customTrackedObjectMetadata = new List<Data.TrackedObject>();
        private List<Thrift.Pose> customTrackedObjectPoses = new List<Thrift.Pose>();
        private bool metaDataChanged = false;
        //private bool posesChanged = false;

        public SdkVirtualTrackedObjectManager()
        {
            VirtualTrackedObjectBehaviours = new List<CustomTrackedObjectBehaviour>();
        }

        public void Activate()
        {

        }
        public void Deactivate()
        {
            if (customTrackedObjectMetadata.Count > 0)
            {
                customTrackedObjectMetadata.Clear();
                metaDataChanged = true;
            }
            if( customTrackedObjectPoses.Count > 0 )
            {
                customTrackedObjectPoses.Clear();
                //posesChanged = true;
            }
            
            SendUpdateToService();
        }

        public void Update()
        {
            if (MixCastSdkBehaviour.Instance == null)
                return;

            UpdateObjectList();
            UpdatePoses();

            SendUpdateToService();
        }

        void UpdateObjectList()
        {
            while (customTrackedObjectMetadata.Count < VirtualTrackedObjectBehaviours.Count)
            {
                customTrackedObjectMetadata.Add(new Data.TrackedObject()
                {
                    Connected = true,
                    Position = new Thrift.Vector3(),
                    Rotation = new Thrift.Quaternion(),
                });
                metaDataChanged = true;
            }
            while (customTrackedObjectMetadata.Count > VirtualTrackedObjectBehaviours.Count)
            {
                customTrackedObjectMetadata.RemoveAt(customTrackedObjectMetadata.Count - 1);
                metaDataChanged = true;
            }
            for (int i = 0; i < customTrackedObjectMetadata.Count; i++)
            {
                metaDataChanged |= UpdateTrackedObjectMetadata(VirtualTrackedObjectBehaviours[i], customTrackedObjectMetadata[i]);
            }
        }
        bool UpdateTrackedObjectMetadata(CustomTrackedObjectBehaviour srcBehaviour, Data.TrackedObject dstObj)
        {
            bool changed = false;

            changed |= dstObj.Identifier != srcBehaviour.objectIdentifier;
            dstObj.Identifier = srcBehaviour.objectIdentifier;

            changed |= dstObj.Name != srcBehaviour.objectName;
            dstObj.Name = srcBehaviour.objectName;

            changed |= dstObj.ObjectType != srcBehaviour.objectType;
            dstObj.ObjectType = srcBehaviour.objectType;

            changed |= dstObj.AssignedRole != srcBehaviour.objectRole;
            dstObj.AssignedRole = srcBehaviour.objectRole;

            changed |= dstObj.HideFromUser != srcBehaviour.hideFromUser;
            dstObj.HideFromUser = srcBehaviour.hideFromUser;

            Vector3 pos = srcBehaviour.GetPosition();
            changed |= Vector3.SqrMagnitude(pos - dstObj.Position.unity) > 0.01f * 0.01f;
            dstObj.Position.unity = pos;

            Quaternion rot = srcBehaviour.GetRotation();
            changed |= Quaternion.Angle(rot, dstObj.Rotation.unity) > 0.01f;
            dstObj.Rotation.unity = rot;

            return changed;
        }
        void UpdatePoses()
        {
            //Can assume that CustomTrackedObjects and customTrackedObjectMetadata are now synced
            while (customTrackedObjectPoses.Count < VirtualTrackedObjectBehaviours.Count)
                customTrackedObjectPoses.Add(new Thrift.Pose() { Position = new Thrift.Vector3(), Rotation = new Thrift.Quaternion() });
            while (customTrackedObjectPoses.Count > VirtualTrackedObjectBehaviours.Count)
                customTrackedObjectPoses.RemoveAt(customTrackedObjectPoses.Count - 1);

            for( int i = 0; i < VirtualTrackedObjectBehaviours.Count; i++ )
            {
                //Assigning pose in 2 places for now
                customTrackedObjectPoses[i].Position.unity = customTrackedObjectMetadata[i].Position.unity = VirtualTrackedObjectBehaviours[i].GetPosition();
                customTrackedObjectPoses[i].Rotation.unity = customTrackedObjectMetadata[i].Rotation.unity = VirtualTrackedObjectBehaviours[i].GetRotation();
                //posesChanged = true;
            }
        }

        void SendUpdateToService()
        {
            if (metaDataChanged)
            {
                Thrift.UnityThriftMixCastClient.Get<Thrift.SDK_Service.Client>().TryUpdateExperienceTrackedObjectMetadata(customTrackedObjectMetadata);
            }
            //if( metaDataChanged || posesChanged )
            //{
            //    Thrift.UnityThriftMixCastClient.Get<Thrift.SDK_Service.Client>().TryUpdateExperienceTrackedObjectPoses(customTrackedObjectPoses);
            //}
            metaDataChanged = false;
            //posesChanged = false;
        }
    }
}
#endif
