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
  public partial class TrackedObject : TBase
  {
    private string _name;
    private BlueprintReality.MixCast.Data.TrackingSource _source;
    private BlueprintReality.MixCast.Data.ObjectType _objectType;
    private BlueprintReality.MixCast.Data.AssignedRole _assignedRole;
    private bool _hideFromUser;
    private bool _connected;
    private BlueprintReality.MixCast.Thrift.Vector3 _position;
    private BlueprintReality.MixCast.Thrift.Quaternion _rotation;

    public string Identifier { get; set; }

    public string Name
    {
      get
      {
        return _name;
      }
      set
      {
        __isset.name = true;
        this._name = value;
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="BlueprintReality.MixCast.Data.TrackingSource"/>
    /// </summary>
    public BlueprintReality.MixCast.Data.TrackingSource Source
    {
      get
      {
        return _source;
      }
      set
      {
        __isset.source = true;
        this._source = value;
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="BlueprintReality.MixCast.Data.ObjectType"/>
    /// </summary>
    public BlueprintReality.MixCast.Data.ObjectType ObjectType
    {
      get
      {
        return _objectType;
      }
      set
      {
        __isset.objectType = true;
        this._objectType = value;
      }
    }

    /// <summary>
    /// 
    /// <seealso cref="BlueprintReality.MixCast.Data.AssignedRole"/>
    /// </summary>
    public BlueprintReality.MixCast.Data.AssignedRole AssignedRole
    {
      get
      {
        return _assignedRole;
      }
      set
      {
        __isset.assignedRole = true;
        this._assignedRole = value;
      }
    }

    public bool HideFromUser
    {
      get
      {
        return _hideFromUser;
      }
      set
      {
        __isset.hideFromUser = true;
        this._hideFromUser = value;
      }
    }

    public bool Connected
    {
      get
      {
        return _connected;
      }
      set
      {
        __isset.connected = true;
        this._connected = value;
      }
    }

    public BlueprintReality.MixCast.Thrift.Vector3 Position
    {
      get
      {
        return _position;
      }
      set
      {
        __isset.position = true;
        this._position = value;
      }
    }

    public BlueprintReality.MixCast.Thrift.Quaternion Rotation
    {
      get
      {
        return _rotation;
      }
      set
      {
        __isset.rotation = true;
        this._rotation = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool name;
      public bool source;
      public bool objectType;
      public bool assignedRole;
      public bool hideFromUser;
      public bool connected;
      public bool position;
      public bool rotation;
    }

    public TrackedObject() {
    }

    public TrackedObject(string identifier) : this() {
      this.Identifier = identifier;
    }

    public void Read (TProtocol iprot)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        bool isset_identifier = false;
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
                Identifier = iprot.ReadString();
                isset_identifier = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.String) {
                Name = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.I32) {
                Source = (BlueprintReality.MixCast.Data.TrackingSource)iprot.ReadI32();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.I32) {
                ObjectType = (BlueprintReality.MixCast.Data.ObjectType)iprot.ReadI32();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 5:
              if (field.Type == TType.I32) {
                AssignedRole = (BlueprintReality.MixCast.Data.AssignedRole)iprot.ReadI32();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 6:
              if (field.Type == TType.Bool) {
                HideFromUser = iprot.ReadBool();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 7:
              if (field.Type == TType.Bool) {
                Connected = iprot.ReadBool();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 8:
              if (field.Type == TType.Struct) {
                Position = new BlueprintReality.MixCast.Thrift.Vector3();
                Position.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 9:
              if (field.Type == TType.Struct) {
                Rotation = new BlueprintReality.MixCast.Thrift.Quaternion();
                Rotation.Read(iprot);
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
        if (!isset_identifier)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field Identifier not set");
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
        TStruct struc = new TStruct("TrackedObject");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (Identifier == null)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field Identifier not set");
        field.Name = "identifier";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Identifier);
        oprot.WriteFieldEnd();
        if (Name != null && __isset.name) {
          field.Name = "name";
          field.Type = TType.String;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Name);
          oprot.WriteFieldEnd();
        }
        if (__isset.source) {
          field.Name = "source";
          field.Type = TType.I32;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32((int)Source);
          oprot.WriteFieldEnd();
        }
        if (__isset.objectType) {
          field.Name = "objectType";
          field.Type = TType.I32;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32((int)ObjectType);
          oprot.WriteFieldEnd();
        }
        if (__isset.assignedRole) {
          field.Name = "assignedRole";
          field.Type = TType.I32;
          field.ID = 5;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32((int)AssignedRole);
          oprot.WriteFieldEnd();
        }
        if (__isset.hideFromUser) {
          field.Name = "hideFromUser";
          field.Type = TType.Bool;
          field.ID = 6;
          oprot.WriteFieldBegin(field);
          oprot.WriteBool(HideFromUser);
          oprot.WriteFieldEnd();
        }
        if (__isset.connected) {
          field.Name = "connected";
          field.Type = TType.Bool;
          field.ID = 7;
          oprot.WriteFieldBegin(field);
          oprot.WriteBool(Connected);
          oprot.WriteFieldEnd();
        }
        if (Position != null && __isset.position) {
          field.Name = "position";
          field.Type = TType.Struct;
          field.ID = 8;
          oprot.WriteFieldBegin(field);
          Position.Write(oprot);
          oprot.WriteFieldEnd();
        }
        if (Rotation != null && __isset.rotation) {
          field.Name = "rotation";
          field.Type = TType.Struct;
          field.ID = 9;
          oprot.WriteFieldBegin(field);
          Rotation.Write(oprot);
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
      StringBuilder __sb = new StringBuilder("TrackedObject(");
      __sb.Append(", Identifier: ");
      __sb.Append(Identifier);
      if (Name != null && __isset.name) {
        __sb.Append(", Name: ");
        __sb.Append(Name);
      }
      if (__isset.source) {
        __sb.Append(", Source: ");
        __sb.Append(Source);
      }
      if (__isset.objectType) {
        __sb.Append(", ObjectType: ");
        __sb.Append(ObjectType);
      }
      if (__isset.assignedRole) {
        __sb.Append(", AssignedRole: ");
        __sb.Append(AssignedRole);
      }
      if (__isset.hideFromUser) {
        __sb.Append(", HideFromUser: ");
        __sb.Append(HideFromUser);
      }
      if (__isset.connected) {
        __sb.Append(", Connected: ");
        __sb.Append(Connected);
      }
      if (Position != null && __isset.position) {
        __sb.Append(", Position: ");
        __sb.Append(Position== null ? "<null>" : Position.ToString());
      }
      if (Rotation != null && __isset.rotation) {
        __sb.Append(", Rotation: ");
        __sb.Append(Rotation== null ? "<null>" : Rotation.ToString());
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
