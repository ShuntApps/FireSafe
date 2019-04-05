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
  public partial class Vector3 : TBase
  {

    public double X { get; set; }

    public double Y { get; set; }

    public double Z { get; set; }

    public Vector3() {
    }

    public Vector3(double x, double y, double z) : this() {
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    public void Read (TProtocol iprot)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        bool isset_x = false;
        bool isset_y = false;
        bool isset_z = false;
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
              if (field.Type == TType.Double) {
                X = iprot.ReadDouble();
                isset_x = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.Double) {
                Y = iprot.ReadDouble();
                isset_y = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.Double) {
                Z = iprot.ReadDouble();
                isset_z = true;
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
        if (!isset_x)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field X not set");
        if (!isset_y)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field Y not set");
        if (!isset_z)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field Z not set");
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
        TStruct struc = new TStruct("Vector3");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        field.Name = "x";
        field.Type = TType.Double;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(X);
        oprot.WriteFieldEnd();
        field.Name = "y";
        field.Type = TType.Double;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(Y);
        oprot.WriteFieldEnd();
        field.Name = "z";
        field.Type = TType.Double;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(Z);
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
      StringBuilder __sb = new StringBuilder("Vector3(");
      __sb.Append(", X: ");
      __sb.Append(X);
      __sb.Append(", Y: ");
      __sb.Append(Y);
      __sb.Append(", Z: ");
      __sb.Append(Z);
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
