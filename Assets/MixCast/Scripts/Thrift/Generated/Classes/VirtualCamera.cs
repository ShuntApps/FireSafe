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
  public partial class VirtualCamera : TBase
  {
    private string _identifier;
    private double _fieldOfView;
    private BlueprintReality.MixCast.Thrift.Vector3 _currentPosition;
    private BlueprintReality.MixCast.Thrift.Quaternion _currentRotation;
    private List<string> _videoInputIds;
    private int _renderResolutionWidth;
    private int _renderResolutionHeight;
    private int _renderFramerate;
    private bool _externalComposite;
    private double _frameBufferDelay;
    private long _outputPtr;
    private bool _autoSnapshotEnabled;
    private bool _videoRecordingEnabled;
    private string _videoRecordingOutputPath;
    private bool _videoStreamingEnabled;

    public string Identifier
    {
      get
      {
        return _identifier;
      }
      set
      {
        __isset.identifier = true;
        this._identifier = value;
      }
    }

    public double FieldOfView
    {
      get
      {
        return _fieldOfView;
      }
      set
      {
        __isset.fieldOfView = true;
        this._fieldOfView = value;
      }
    }

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

    public List<string> VideoInputIds
    {
      get
      {
        return _videoInputIds;
      }
      set
      {
        __isset.videoInputIds = true;
        this._videoInputIds = value;
      }
    }

    public int RenderResolutionWidth
    {
      get
      {
        return _renderResolutionWidth;
      }
      set
      {
        __isset.renderResolutionWidth = true;
        this._renderResolutionWidth = value;
      }
    }

    public int RenderResolutionHeight
    {
      get
      {
        return _renderResolutionHeight;
      }
      set
      {
        __isset.renderResolutionHeight = true;
        this._renderResolutionHeight = value;
      }
    }

    public int RenderFramerate
    {
      get
      {
        return _renderFramerate;
      }
      set
      {
        __isset.renderFramerate = true;
        this._renderFramerate = value;
      }
    }

    public bool ExternalComposite
    {
      get
      {
        return _externalComposite;
      }
      set
      {
        __isset.externalComposite = true;
        this._externalComposite = value;
      }
    }

    public double FrameBufferDelay
    {
      get
      {
        return _frameBufferDelay;
      }
      set
      {
        __isset.frameBufferDelay = true;
        this._frameBufferDelay = value;
      }
    }

    public long OutputPtr
    {
      get
      {
        return _outputPtr;
      }
      set
      {
        __isset.outputPtr = true;
        this._outputPtr = value;
      }
    }

    public bool AutoSnapshotEnabled
    {
      get
      {
        return _autoSnapshotEnabled;
      }
      set
      {
        __isset.autoSnapshotEnabled = true;
        this._autoSnapshotEnabled = value;
      }
    }

    public bool VideoRecordingEnabled
    {
      get
      {
        return _videoRecordingEnabled;
      }
      set
      {
        __isset.videoRecordingEnabled = true;
        this._videoRecordingEnabled = value;
      }
    }

    public string VideoRecordingOutputPath
    {
      get
      {
        return _videoRecordingOutputPath;
      }
      set
      {
        __isset.videoRecordingOutputPath = true;
        this._videoRecordingOutputPath = value;
      }
    }

    public bool VideoStreamingEnabled
    {
      get
      {
        return _videoStreamingEnabled;
      }
      set
      {
        __isset.videoStreamingEnabled = true;
        this._videoStreamingEnabled = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool identifier;
      public bool fieldOfView;
      public bool currentPosition;
      public bool currentRotation;
      public bool videoInputIds;
      public bool renderResolutionWidth;
      public bool renderResolutionHeight;
      public bool renderFramerate;
      public bool externalComposite;
      public bool frameBufferDelay;
      public bool outputPtr;
      public bool autoSnapshotEnabled;
      public bool videoRecordingEnabled;
      public bool videoRecordingOutputPath;
      public bool videoStreamingEnabled;
    }

    public VirtualCamera() {
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
                Identifier = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.Double) {
                FieldOfView = iprot.ReadDouble();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.Struct) {
                CurrentPosition = new BlueprintReality.MixCast.Thrift.Vector3();
                CurrentPosition.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.Struct) {
                CurrentRotation = new BlueprintReality.MixCast.Thrift.Quaternion();
                CurrentRotation.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 5:
              if (field.Type == TType.List) {
                {
                  VideoInputIds = new List<string>();
                  TList _list0 = iprot.ReadListBegin();
                  for( int _i1 = 0; _i1 < _list0.Count; ++_i1)
                  {
                    string _elem2;
                    _elem2 = iprot.ReadString();
                    VideoInputIds.Add(_elem2);
                  }
                  iprot.ReadListEnd();
                }
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 6:
              if (field.Type == TType.I32) {
                RenderResolutionWidth = iprot.ReadI32();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 7:
              if (field.Type == TType.I32) {
                RenderResolutionHeight = iprot.ReadI32();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 8:
              if (field.Type == TType.I32) {
                RenderFramerate = iprot.ReadI32();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 9:
              if (field.Type == TType.Bool) {
                ExternalComposite = iprot.ReadBool();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 10:
              if (field.Type == TType.Double) {
                FrameBufferDelay = iprot.ReadDouble();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 11:
              if (field.Type == TType.I64) {
                OutputPtr = iprot.ReadI64();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 12:
              if (field.Type == TType.Bool) {
                AutoSnapshotEnabled = iprot.ReadBool();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 13:
              if (field.Type == TType.Bool) {
                VideoRecordingEnabled = iprot.ReadBool();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 14:
              if (field.Type == TType.String) {
                VideoRecordingOutputPath = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 15:
              if (field.Type == TType.Bool) {
                VideoStreamingEnabled = iprot.ReadBool();
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
        TStruct struc = new TStruct("VirtualCamera");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (Identifier != null && __isset.identifier) {
          field.Name = "identifier";
          field.Type = TType.String;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(Identifier);
          oprot.WriteFieldEnd();
        }
        if (__isset.fieldOfView) {
          field.Name = "fieldOfView";
          field.Type = TType.Double;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteDouble(FieldOfView);
          oprot.WriteFieldEnd();
        }
        if (CurrentPosition != null && __isset.currentPosition) {
          field.Name = "currentPosition";
          field.Type = TType.Struct;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          CurrentPosition.Write(oprot);
          oprot.WriteFieldEnd();
        }
        if (CurrentRotation != null && __isset.currentRotation) {
          field.Name = "currentRotation";
          field.Type = TType.Struct;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          CurrentRotation.Write(oprot);
          oprot.WriteFieldEnd();
        }
        if (VideoInputIds != null && __isset.videoInputIds) {
          field.Name = "videoInputIds";
          field.Type = TType.List;
          field.ID = 5;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteListBegin(new TList(TType.String, VideoInputIds.Count));
            foreach (string _iter3 in VideoInputIds)
            {
              oprot.WriteString(_iter3);
            }
            oprot.WriteListEnd();
          }
          oprot.WriteFieldEnd();
        }
        if (__isset.renderResolutionWidth) {
          field.Name = "renderResolutionWidth";
          field.Type = TType.I32;
          field.ID = 6;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32(RenderResolutionWidth);
          oprot.WriteFieldEnd();
        }
        if (__isset.renderResolutionHeight) {
          field.Name = "renderResolutionHeight";
          field.Type = TType.I32;
          field.ID = 7;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32(RenderResolutionHeight);
          oprot.WriteFieldEnd();
        }
        if (__isset.renderFramerate) {
          field.Name = "renderFramerate";
          field.Type = TType.I32;
          field.ID = 8;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32(RenderFramerate);
          oprot.WriteFieldEnd();
        }
        if (__isset.externalComposite) {
          field.Name = "externalComposite";
          field.Type = TType.Bool;
          field.ID = 9;
          oprot.WriteFieldBegin(field);
          oprot.WriteBool(ExternalComposite);
          oprot.WriteFieldEnd();
        }
        if (__isset.frameBufferDelay) {
          field.Name = "frameBufferDelay";
          field.Type = TType.Double;
          field.ID = 10;
          oprot.WriteFieldBegin(field);
          oprot.WriteDouble(FrameBufferDelay);
          oprot.WriteFieldEnd();
        }
        if (__isset.outputPtr) {
          field.Name = "outputPtr";
          field.Type = TType.I64;
          field.ID = 11;
          oprot.WriteFieldBegin(field);
          oprot.WriteI64(OutputPtr);
          oprot.WriteFieldEnd();
        }
        if (__isset.autoSnapshotEnabled) {
          field.Name = "autoSnapshotEnabled";
          field.Type = TType.Bool;
          field.ID = 12;
          oprot.WriteFieldBegin(field);
          oprot.WriteBool(AutoSnapshotEnabled);
          oprot.WriteFieldEnd();
        }
        if (__isset.videoRecordingEnabled) {
          field.Name = "videoRecordingEnabled";
          field.Type = TType.Bool;
          field.ID = 13;
          oprot.WriteFieldBegin(field);
          oprot.WriteBool(VideoRecordingEnabled);
          oprot.WriteFieldEnd();
        }
        if (VideoRecordingOutputPath != null && __isset.videoRecordingOutputPath) {
          field.Name = "videoRecordingOutputPath";
          field.Type = TType.String;
          field.ID = 14;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(VideoRecordingOutputPath);
          oprot.WriteFieldEnd();
        }
        if (__isset.videoStreamingEnabled) {
          field.Name = "videoStreamingEnabled";
          field.Type = TType.Bool;
          field.ID = 15;
          oprot.WriteFieldBegin(field);
          oprot.WriteBool(VideoStreamingEnabled);
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
      StringBuilder __sb = new StringBuilder("VirtualCamera(");
      bool __first = true;
      if (Identifier != null && __isset.identifier) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Identifier: ");
        __sb.Append(Identifier);
      }
      if (__isset.fieldOfView) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("FieldOfView: ");
        __sb.Append(FieldOfView);
      }
      if (CurrentPosition != null && __isset.currentPosition) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("CurrentPosition: ");
        __sb.Append(CurrentPosition== null ? "<null>" : CurrentPosition.ToString());
      }
      if (CurrentRotation != null && __isset.currentRotation) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("CurrentRotation: ");
        __sb.Append(CurrentRotation== null ? "<null>" : CurrentRotation.ToString());
      }
      if (VideoInputIds != null && __isset.videoInputIds) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("VideoInputIds: ");
        __sb.Append(VideoInputIds);
      }
      if (__isset.renderResolutionWidth) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("RenderResolutionWidth: ");
        __sb.Append(RenderResolutionWidth);
      }
      if (__isset.renderResolutionHeight) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("RenderResolutionHeight: ");
        __sb.Append(RenderResolutionHeight);
      }
      if (__isset.renderFramerate) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("RenderFramerate: ");
        __sb.Append(RenderFramerate);
      }
      if (__isset.externalComposite) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("ExternalComposite: ");
        __sb.Append(ExternalComposite);
      }
      if (__isset.frameBufferDelay) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("FrameBufferDelay: ");
        __sb.Append(FrameBufferDelay);
      }
      if (__isset.outputPtr) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("OutputPtr: ");
        __sb.Append(OutputPtr);
      }
      if (__isset.autoSnapshotEnabled) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("AutoSnapshotEnabled: ");
        __sb.Append(AutoSnapshotEnabled);
      }
      if (__isset.videoRecordingEnabled) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("VideoRecordingEnabled: ");
        __sb.Append(VideoRecordingEnabled);
      }
      if (VideoRecordingOutputPath != null && __isset.videoRecordingOutputPath) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("VideoRecordingOutputPath: ");
        __sb.Append(VideoRecordingOutputPath);
      }
      if (__isset.videoStreamingEnabled) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("VideoStreamingEnabled: ");
        __sb.Append(VideoStreamingEnabled);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
