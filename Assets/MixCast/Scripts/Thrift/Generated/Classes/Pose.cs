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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;
using Thrift.Proxy;

namespace BlueprintReality.MixCast.Thrift
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class Pose : TBase
  {

    public Vector3 Position { get; set; }

    public Quaternion Rotation { get; set; }

    public Pose() {
    }

    public Pose(Vector3 position, Quaternion rotation) : this() {
      this.Position = position;
      this.Rotation = rotation;
    }

    public void Read (TProtocol iprot)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        bool isset_position = false;
        bool isset_rotation = false;
        TField field;
        iprot.ReadStructBegin();
        while (true)
        {
          field = iprot.ReadFieldBegin();
          if (field.Type == TType.Stop) { 
            break;
          }
          switch (field.ID)
          {
            case 1:
              if (field.Type == TType.Struct) {
                Position = new Vector3();
                Position.Read(iprot);
                isset_position = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.Struct) {
                Rotation = new Quaternion();
                Rotation.Read(iprot);
                isset_rotation = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            default: 
              TProtocolUtil.Skip(iprot, field.Type);
              break;
          }
          iprot.ReadFieldEnd();
        }
        iprot.ReadStructEnd();
        if (!isset_position)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field Position not set");
        if (!isset_rotation)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field Rotation not set");
      }
      finally
      {
        iprot.DecrementRecursionDepth();
      }
    }

    public void Write(TProtocol oprot) {
      oprot.IncrementRecursionDepth();
      try
      {
        TStruct struc = new TStruct("Pose");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (Position == null)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field Position not set");
        field.Name = "position";
        field.Type = TType.Struct;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        Position.Write(oprot);
        oprot.WriteFieldEnd();
        if (Rotation == null)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field Rotation not set");
        field.Name = "rotation";
        field.Type = TType.Struct;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        Rotation.Write(oprot);
        oprot.WriteFieldEnd();
        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }
      finally
      {
        oprot.DecrementRecursionDepth();
      }
    }

    public override string ToString() {
      StringBuilder __sb = new StringBuilder("Pose(");
      __sb.Append(", Position: ");
      __sb.Append(Position== null ? "<null>" : Position.ToString());
      __sb.Append(", Rotation: ");
      __sb.Append(Rotation== null ? "<null>" : Rotation.ToString());
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
