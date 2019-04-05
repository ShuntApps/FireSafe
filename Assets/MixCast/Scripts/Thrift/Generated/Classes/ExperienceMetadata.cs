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
  public partial class ExperienceMetadata : TBase
  {
    private string _experienceExePath;
    private string _experienceTitle;
    private string _organizationName;
    private string _mixcastVersion;
    private string _engineVersion;
    private string _projectId;
    private bool _alphaIsPremultiplied;

    public string ExperienceExePath
    {
      get
      {
        return _experienceExePath;
      }
      set
      {
        __isset.experienceExePath = true;
        this._experienceExePath = value;
      }
    }

    public string ExperienceTitle
    {
      get
      {
        return _experienceTitle;
      }
      set
      {
        __isset.experienceTitle = true;
        this._experienceTitle = value;
      }
    }

    public string OrganizationName
    {
      get
      {
        return _organizationName;
      }
      set
      {
        __isset.organizationName = true;
        this._organizationName = value;
      }
    }

    public string MixcastVersion
    {
      get
      {
        return _mixcastVersion;
      }
      set
      {
        __isset.mixcastVersion = true;
        this._mixcastVersion = value;
      }
    }

    public string EngineVersion
    {
      get
      {
        return _engineVersion;
      }
      set
      {
        __isset.engineVersion = true;
        this._engineVersion = value;
      }
    }

    public string ProjectId
    {
      get
      {
        return _projectId;
      }
      set
      {
        __isset.projectId = true;
        this._projectId = value;
      }
    }

    public bool AlphaIsPremultiplied
    {
      get
      {
        return _alphaIsPremultiplied;
      }
      set
      {
        __isset.alphaIsPremultiplied = true;
        this._alphaIsPremultiplied = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool experienceExePath;
      public bool experienceTitle;
      public bool organizationName;
      public bool mixcastVersion;
      public bool engineVersion;
      public bool projectId;
      public bool alphaIsPremultiplied;
    }

    public ExperienceMetadata() {
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
              if (field.Type == TType.String) {
                ExperienceExePath = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.String) {
                ExperienceTitle = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.String) {
                OrganizationName = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.String) {
                MixcastVersion = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 5:
              if (field.Type == TType.String) {
                EngineVersion = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 6:
              if (field.Type == TType.String) {
                ProjectId = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 7:
              if (field.Type == TType.Bool) {
                AlphaIsPremultiplied = iprot.ReadBool();
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
        TStruct struc = new TStruct("ExperienceMetadata");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (ExperienceExePath != null && __isset.experienceExePath) {
          field.Name = "experienceExePath";
          field.Type = TType.String;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(ExperienceExePath);
          oprot.WriteFieldEnd();
        }
        if (ExperienceTitle != null && __isset.experienceTitle) {
          field.Name = "experienceTitle";
          field.Type = TType.String;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(ExperienceTitle);
          oprot.WriteFieldEnd();
        }
        if (OrganizationName != null && __isset.organizationName) {
          field.Name = "organizationName";
          field.Type = TType.String;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(OrganizationName);
          oprot.WriteFieldEnd();
        }
        if (MixcastVersion != null && __isset.mixcastVersion) {
          field.Name = "mixcastVersion";
          field.Type = TType.String;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(MixcastVersion);
          oprot.WriteFieldEnd();
        }
        if (EngineVersion != null && __isset.engineVersion) {
          field.Name = "engineVersion";
          field.Type = TType.String;
          field.ID = 5;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(EngineVersion);
          oprot.WriteFieldEnd();
        }
        if (ProjectId != null && __isset.projectId) {
          field.Name = "projectId";
          field.Type = TType.String;
          field.ID = 6;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(ProjectId);
          oprot.WriteFieldEnd();
        }
        if (__isset.alphaIsPremultiplied) {
          field.Name = "alphaIsPremultiplied";
          field.Type = TType.Bool;
          field.ID = 7;
          oprot.WriteFieldBegin(field);
          oprot.WriteBool(AlphaIsPremultiplied);
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
      StringBuilder __sb = new StringBuilder("ExperienceMetadata(");
      bool __first = true;
      if (ExperienceExePath != null && __isset.experienceExePath) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("ExperienceExePath: ");
        __sb.Append(ExperienceExePath);
      }
      if (ExperienceTitle != null && __isset.experienceTitle) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("ExperienceTitle: ");
        __sb.Append(ExperienceTitle);
      }
      if (OrganizationName != null && __isset.organizationName) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("OrganizationName: ");
        __sb.Append(OrganizationName);
      }
      if (MixcastVersion != null && __isset.mixcastVersion) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("MixcastVersion: ");
        __sb.Append(MixcastVersion);
      }
      if (EngineVersion != null && __isset.engineVersion) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("EngineVersion: ");
        __sb.Append(EngineVersion);
      }
      if (ProjectId != null && __isset.projectId) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("ProjectId: ");
        __sb.Append(ProjectId);
      }
      if (__isset.alphaIsPremultiplied) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("AlphaIsPremultiplied: ");
        __sb.Append(AlphaIsPremultiplied);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
