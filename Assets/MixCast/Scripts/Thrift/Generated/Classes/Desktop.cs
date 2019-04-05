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
  public partial class Desktop : TBase
  {
    private bool _hideMixCastOutput;
    private int _maxDisplayingCameras;
    private List<string> _displayingCameraIds;
    private bool _hideMixCastUI;
    private double _uiZoomFactor;

    public bool HideMixCastOutput
    {
      get
      {
        return _hideMixCastOutput;
      }
      set
      {
        __isset.hideMixCastOutput = true;
        this._hideMixCastOutput = value;
      }
    }

    public int MaxDisplayingCameras
    {
      get
      {
        return _maxDisplayingCameras;
      }
      set
      {
        __isset.maxDisplayingCameras = true;
        this._maxDisplayingCameras = value;
      }
    }

    public List<string> DisplayingCameraIds
    {
      get
      {
        return _displayingCameraIds;
      }
      set
      {
        __isset.displayingCameraIds = true;
        this._displayingCameraIds = value;
      }
    }

    public bool HideMixCastUI
    {
      get
      {
        return _hideMixCastUI;
      }
      set
      {
        __isset.hideMixCastUI = true;
        this._hideMixCastUI = value;
      }
    }

    public double UiZoomFactor
    {
      get
      {
        return _uiZoomFactor;
      }
      set
      {
        __isset.uiZoomFactor = true;
        this._uiZoomFactor = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool hideMixCastOutput;
      public bool maxDisplayingCameras;
      public bool displayingCameraIds;
      public bool hideMixCastUI;
      public bool uiZoomFactor;
    }

    public Desktop() {
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
              if (field.Type == TType.Bool) {
                HideMixCastOutput = iprot.ReadBool();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.I32) {
                MaxDisplayingCameras = iprot.ReadI32();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.List) {
                {
                  DisplayingCameraIds = new List<string>();
                  TList _list0 = iprot.ReadListBegin();
                  for( int _i1 = 0; _i1 < _list0.Count; ++_i1)
                  {
                    string _elem2;
                    _elem2 = iprot.ReadString();
                    DisplayingCameraIds.Add(_elem2);
                  }
                  iprot.ReadListEnd();
                }
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.Bool) {
                HideMixCastUI = iprot.ReadBool();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 5:
              if (field.Type == TType.Double) {
                UiZoomFactor = iprot.ReadDouble();
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
        TStruct struc = new TStruct("Desktop");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (__isset.hideMixCastOutput) {
          field.Name = "hideMixCastOutput";
          field.Type = TType.Bool;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          oprot.WriteBool(HideMixCastOutput);
          oprot.WriteFieldEnd();
        }
        if (__isset.maxDisplayingCameras) {
          field.Name = "maxDisplayingCameras";
          field.Type = TType.I32;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32(MaxDisplayingCameras);
          oprot.WriteFieldEnd();
        }
        if (DisplayingCameraIds != null && __isset.displayingCameraIds) {
          field.Name = "displayingCameraIds";
          field.Type = TType.List;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          {
            oprot.WriteListBegin(new TList(TType.String, DisplayingCameraIds.Count));
            foreach (string _iter3 in DisplayingCameraIds)
            {
              oprot.WriteString(_iter3);
            }
            oprot.WriteListEnd();
          }
          oprot.WriteFieldEnd();
        }
        if (__isset.hideMixCastUI) {
          field.Name = "hideMixCastUI";
          field.Type = TType.Bool;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          oprot.WriteBool(HideMixCastUI);
          oprot.WriteFieldEnd();
        }
        if (__isset.uiZoomFactor) {
          field.Name = "uiZoomFactor";
          field.Type = TType.Double;
          field.ID = 5;
          oprot.WriteFieldBegin(field);
          oprot.WriteDouble(UiZoomFactor);
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
      StringBuilder __sb = new StringBuilder("Desktop(");
      bool __first = true;
      if (__isset.hideMixCastOutput) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("HideMixCastOutput: ");
        __sb.Append(HideMixCastOutput);
      }
      if (__isset.maxDisplayingCameras) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("MaxDisplayingCameras: ");
        __sb.Append(MaxDisplayingCameras);
      }
      if (DisplayingCameraIds != null && __isset.displayingCameraIds) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("DisplayingCameraIds: ");
        __sb.Append(DisplayingCameraIds);
      }
      if (__isset.hideMixCastUI) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("HideMixCastUI: ");
        __sb.Append(HideMixCastUI);
      }
      if (__isset.uiZoomFactor) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("UiZoomFactor: ");
        __sb.Append(UiZoomFactor);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
#endif
