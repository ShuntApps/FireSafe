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

namespace BlueprintReality.MixCast.Data
{

#if !SILVERLIGHT
  [Serializable]
#endif
  public partial class License : TBase
  {
    private BlueprintReality.MixCast.Data.LicenseType _licenseType;

    /// <summary>
    /// 
    /// <seealso cref="BlueprintReality.MixCast.Data.LicenseType"/>
    /// </summary>
    public BlueprintReality.MixCast.Data.LicenseType LicenseType
    {
      get
      {
        return _licenseType;
      }
      set
      {
        __isset.licenseType = true;
        this._licenseType = value;
      }
    }


    public Isset __isset;
#if !SILVERLIGHT
    [Serializable]
#endif
    public struct Isset {
      public bool licenseType;
    }

    public License() {
    }

    public void Read (TProtocol iprot)
    {
      iprot.IncrementRecursionDepth();
      try
      {
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
              if (field.Type == TType.I32) {
                LicenseType = (BlueprintReality.MixCast.Data.LicenseType)iprot.ReadI32();
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
        TStruct struc = new TStruct("License");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (__isset.licenseType) {
          field.Name = "licenseType";
          field.Type = TType.I32;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32((int)LicenseType);
          oprot.WriteFieldEnd();
        }
        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }
      finally
      {
        oprot.DecrementRecursionDepth();
      }
    }

    public override string ToString() {
      StringBuilder __sb = new StringBuilder("License(");
      bool __first = true;
      if (__isset.licenseType) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("LicenseType: ");
        __sb.Append(LicenseType);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
