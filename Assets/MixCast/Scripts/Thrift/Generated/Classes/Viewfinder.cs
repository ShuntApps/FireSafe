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
  public partial class Viewfinder : TBase
  {
    private BlueprintReality.MixCast.Thrift.Vector3 _currentPosition;
    private BlueprintReality.MixCast.Thrift.Quaternion _currentRotation;
    private BlueprintReality.MixCast.Thrift.Vector3 _currentScale;

    public string CameraId { get; set; }

    public BlueprintReality.MixCast.Thrift.Vector3 CurrentPosition
    {
      get
      {
        return _currentPosition;
      }
      set
      {
        __isset.currentPosition = true;
        this._currentPosition = value;
      }
    }

    public BlueprintReality.MixCast.Thrift.Quaternion CurrentRotation
    {
      get
      {
        return _currentRotation;
      }
      set
      {
        __isset.currentRotation = true;
        this._currentRotation = value;
      }
    }

    public BlueprintReality.MixCast.Thrift.Vector3 CurrentScale
    {
      get
      {
        return _currentScale;
      }
      set
      {
        __isset.currentScale = true;
        this._currentScale = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool currentPosition;
      public bool currentRotation;
      public bool currentScale;
    }

    public Viewfinder() {
    }

    public Viewfinder(string cameraId) : this() {
      this.CameraId = cameraId;
    }

    public void Read (TProtocol iprot)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        bool isset_cameraId = false;
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
              if (field.Type == TType.String) {
                CameraId = iprot.ReadString();
                isset_cameraId = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.Struct) {
                CurrentPosition = new BlueprintReality.MixCast.Thrift.Vector3();
                CurrentPosition.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.Struct) {
                CurrentRotation = new BlueprintReality.MixCast.Thrift.Quaternion();
                CurrentRotation.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.Struct) {
                CurrentScale = new BlueprintReality.MixCast.Thrift.Vector3();
                CurrentScale.Read(iprot);
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
        if (!isset_cameraId)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field CameraId not set");
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
        TStruct struc = new TStruct("Viewfinder");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (CameraId == null)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field CameraId not set");
        field.Name = "cameraId";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(CameraId);
        oprot.WriteFieldEnd();
        if (CurrentPosition != null && __isset.currentPosition) {
          field.Name = "currentPosition";
          field.Type = TType.Struct;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          CurrentPosition.Write(oprot);
          oprot.WriteFieldEnd();
        }
        if (CurrentRotation != null && __isset.currentRotation) {
          field.Name = "currentRotation";
          field.Type = TType.Struct;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          CurrentRotation.Write(oprot);
          oprot.WriteFieldEnd();
        }
        if (CurrentScale != null && __isset.currentScale) {
          field.Name = "currentScale";
          field.Type = TType.Struct;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          CurrentScale.Write(oprot);
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
      StringBuilder __sb = new StringBuilder("Viewfinder(");
      __sb.Append(", CameraId: ");
      __sb.Append(CameraId);
      if (CurrentPosition != null && __isset.currentPosition) {
        __sb.Append(", CurrentPosition: ");
        __sb.Append(CurrentPosition== null ? "<null>" : CurrentPosition.ToString());
      }
      if (CurrentRotation != null && __isset.currentRotation) {
        __sb.Append(", CurrentRotation: ");
        __sb.Append(CurrentRotation== null ? "<null>" : CurrentRotation.ToString());
      }
      if (CurrentScale != null && __isset.currentScale) {
        __sb.Append(", CurrentScale: ");
        __sb.Append(CurrentScale== null ? "<null>" : CurrentScale.ToString());
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
