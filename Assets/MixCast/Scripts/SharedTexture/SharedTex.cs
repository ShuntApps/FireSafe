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

namespace BlueprintReality.MixCast.SharedTexture
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class SharedTex : TBase
  {

    public long Handle { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public int Format { get; set; }

    public SharedTex() {
    }

    public SharedTex(long handle, int width, int height, int format) : this() {
      this.Handle = handle;
      this.Width = width;
      this.Height = height;
      this.Format = format;
    }

    public void Read (TProtocol iprot)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        bool isset_handle = false;
        bool isset_width = false;
        bool isset_height = false;
        bool isset_format = false;
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
              if (field.Type == TType.I64) {
                Handle = iprot.ReadI64();
                isset_handle = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.I32) {
                Width = iprot.ReadI32();
                isset_width = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.I32) {
                Height = iprot.ReadI32();
                isset_height = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.I32) {
                Format = iprot.ReadI32();
                isset_format = true;
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
        if (!isset_handle)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field Handle not set");
        if (!isset_width)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field Width not set");
        if (!isset_height)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field Height not set");
        if (!isset_format)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field Format not set");
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
        TStruct struc = new TStruct("SharedTex");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        field.Name = "handle";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(Handle);
        oprot.WriteFieldEnd();
        field.Name = "width";
        field.Type = TType.I32;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Width);
        oprot.WriteFieldEnd();
        field.Name = "height";
        field.Type = TType.I32;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Height);
        oprot.WriteFieldEnd();
        field.Name = "format";
        field.Type = TType.I32;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Format);
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
      StringBuilder __sb = new StringBuilder("SharedTex(");
      __sb.Append(", Handle: ");
      __sb.Append(Handle);
      __sb.Append(", Width: ");
      __sb.Append(Width);
      __sb.Append(", Height: ");
      __sb.Append(Height);
      __sb.Append(", Format: ");
      __sb.Append(Format);
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
