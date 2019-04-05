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
  public partial class SharedTextureCommunication {
    public interface ISyncServer {
      void validate_ping();
      List<string> validate_function_list();
      void SharedTextureNotify(string texId, SharedTex texture);
      SharedTex SharedTextureRequest(string texId);
    }

    public interface IFaceServer : ISyncServer {
      #if SILVERLIGHT
      IAsyncResult Begin_validate_ping(AsyncCallback callback, object state);
      void End_validate_ping(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_validate_function_list(AsyncCallback callback, object state);
      List<string> End_validate_function_list(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_SharedTextureNotify(AsyncCallback callback, object state, string texId, SharedTex texture);
      void End_SharedTextureNotify(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_SharedTextureRequest(AsyncCallback callback, object state, string texId);
      SharedTex End_SharedTextureRequest(IAsyncResult asyncResult);
      #endif
    }

    public interface ISyncClient {
      bool TrySharedTextureNotify(string texId, SharedTex texture);
      bool TrySharedTextureRequest(string texId, out SharedTex result);
    }

    public interface IFaceClient : ISyncClient {
      #if SILVERLIGHT
      IAsyncResult Begin_validate_ping(AsyncCallback callback, object state);
      void End_validate_ping(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_validate_function_list(AsyncCallback callback, object state);
      List<string> End_validate_function_list(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_SharedTextureNotify(AsyncCallback callback, object state, string texId, SharedTex texture);
      void End_SharedTextureNotify(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_SharedTextureRequest(AsyncCallback callback, object state, string texId);
      SharedTex End_SharedTextureRequest(IAsyncResult asyncResult);
      #endif
    }

    public abstract class Handler : HandlerProxy, IFaceServer {
      public void validate_ping(){ }
      public List<string> validate_function_list(){ return new List<string>(){ "validate_ping", "validate_function_list", "SharedTextureNotify", "SharedTextureRequest", }; }
      public abstract void SharedTextureNotify(string texId, SharedTex texture);
      public abstract SharedTex SharedTextureRequest(string texId);
    }

    public class Client : ClientProxy, IDisposable, IFaceClient {
      public Client(TProtocol prot) : this(prot, prot)
      {
      }

      public Client(TProtocol iprot, TProtocol oprot)
      {
        iprot_ = iprot;
        oprot_ = oprot;
      }

      protected TProtocol iprot_;
      protected TProtocol oprot_;
      protected int seqid_;

      public TProtocol InputProtocol
      {
        get { return iprot_; }
      }
      public TProtocol OutputProtocol
      {
        get { return oprot_; }
      }


      #region " IDisposable Support "
      private bool _IsDisposed;

      // IDisposable
      public void Dispose()
      {
        Dispose(true);
      }
      

      protected virtual void Dispose(bool disposing)
      {
        if (!_IsDisposed)
        {
          if (disposing)
          {
            if (iprot_ != null)
            {
              ((IDisposable)iprot_).Dispose();
            }
            if (oprot_ != null)
            {
              ((IDisposable)oprot_).Dispose();
            }
          }
        }
        _IsDisposed = true;
      }
      #endregion


      
      #if SILVERLIGHT
      public IAsyncResult Begin_validate_ping(AsyncCallback callback, object state)
      {
        return send_validate_ping(callback, state);
      }

      public void End_validate_ping(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
        recv_validate_ping();
      }

      #endif

      protected void validate_ping()
      {
        #if !SILVERLIGHT
        send_validate_ping();
        recv_validate_ping();

        #else
        var asyncResult = Begin_validate_ping(null, null);
        End_validate_ping(asyncResult);

        #endif
      }
      #if SILVERLIGHT
      protected IAsyncResult send_validate_ping(AsyncCallback callback, object state)
      #else
      protected void send_validate_ping()
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("validate_ping", TMessageType.Call, seqid_));
        validate_ping_args args = new validate_ping_args();
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      protected void recv_validate_ping()
      {
        TMessage msg = iprot_.ReadMessageBegin();
        if (msg.Type == TMessageType.Exception) {
          TApplicationException x = TApplicationException.Read(iprot_);
          iprot_.ReadMessageEnd();
          throw x;
        }
        validate_ping_result result = new validate_ping_result();
        result.Read(iprot_);
        iprot_.ReadMessageEnd();
        return;
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_validate_function_list(AsyncCallback callback, object state)
      {
        return send_validate_function_list(callback, state);
      }

      public List<string> End_validate_function_list(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
        return recv_validate_function_list();
      }

      #endif

      protected List<string> validate_function_list()
      {
        #if !SILVERLIGHT
        send_validate_function_list();
        return recv_validate_function_list();

        #else
        var asyncResult = Begin_validate_function_list(null, null);
        return End_validate_function_list(asyncResult);

        #endif
      }
      #if SILVERLIGHT
      protected IAsyncResult send_validate_function_list(AsyncCallback callback, object state)
      #else
      protected void send_validate_function_list()
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("validate_function_list", TMessageType.Call, seqid_));
        validate_function_list_args args = new validate_function_list_args();
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      protected List<string> recv_validate_function_list()
      {
        TMessage msg = iprot_.ReadMessageBegin();
        if (msg.Type == TMessageType.Exception) {
          TApplicationException x = TApplicationException.Read(iprot_);
          iprot_.ReadMessageEnd();
          throw x;
        }
        validate_function_list_result result = new validate_function_list_result();
        result.Read(iprot_);
        iprot_.ReadMessageEnd();
        if (result.__isset.success) {
          return result.Success;
        }
        throw new TApplicationException(TApplicationException.ExceptionType.MissingResult, "validate_function_list failed: unknown result");
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_SharedTextureNotify(AsyncCallback callback, object state, string texId, SharedTex texture)
      {
        return send_SharedTextureNotify(callback, state, texId, texture);
      }

      public void End_SharedTextureNotify(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
        recv_SharedTextureNotify();
      }

      #endif

      public bool TrySharedTextureNotify(string texId, SharedTex texture){ try { SharedTextureNotify(texId, texture); return true; } catch { return false; } }

      protected void SharedTextureNotify(string texId, SharedTex texture)
      {
        #if !SILVERLIGHT
        send_SharedTextureNotify(texId, texture);
        recv_SharedTextureNotify();

        #else
        var asyncResult = Begin_SharedTextureNotify(null, null, texId, texture);
        End_SharedTextureNotify(asyncResult);

        #endif
      }
      #if SILVERLIGHT
      protected IAsyncResult send_SharedTextureNotify(AsyncCallback callback, object state, string texId, SharedTex texture)
      #else
      protected void send_SharedTextureNotify(string texId, SharedTex texture)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("SharedTextureNotify", TMessageType.Call, seqid_));
        SharedTextureNotify_args args = new SharedTextureNotify_args();
        args.TexId = texId;
        args.Texture = texture;
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      protected void recv_SharedTextureNotify()
      {
        TMessage msg = iprot_.ReadMessageBegin();
        if (msg.Type == TMessageType.Exception) {
          TApplicationException x = TApplicationException.Read(iprot_);
          iprot_.ReadMessageEnd();
          throw x;
        }
        SharedTextureNotify_result result = new SharedTextureNotify_result();
        result.Read(iprot_);
        iprot_.ReadMessageEnd();
        return;
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_SharedTextureRequest(AsyncCallback callback, object state, string texId)
      {
        return send_SharedTextureRequest(callback, state, texId);
      }

      public SharedTex End_SharedTextureRequest(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
        return recv_SharedTextureRequest();
      }

      #endif

      public bool TrySharedTextureRequest(string texId, out SharedTex result){ try { result = SharedTextureRequest(texId); return true; } catch { result = default(SharedTex); return false; } }

      protected SharedTex SharedTextureRequest(string texId)
      {
        #if !SILVERLIGHT
        send_SharedTextureRequest(texId);
        return recv_SharedTextureRequest();

        #else
        var asyncResult = Begin_SharedTextureRequest(null, null, texId);
        return End_SharedTextureRequest(asyncResult);

        #endif
      }
      #if SILVERLIGHT
      protected IAsyncResult send_SharedTextureRequest(AsyncCallback callback, object state, string texId)
      #else
      protected void send_SharedTextureRequest(string texId)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("SharedTextureRequest", TMessageType.Call, seqid_));
        SharedTextureRequest_args args = new SharedTextureRequest_args();
        args.TexId = texId;
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      protected SharedTex recv_SharedTextureRequest()
      {
        TMessage msg = iprot_.ReadMessageBegin();
        if (msg.Type == TMessageType.Exception) {
          TApplicationException x = TApplicationException.Read(iprot_);
          iprot_.ReadMessageEnd();
          throw x;
        }
        SharedTextureRequest_result result = new SharedTextureRequest_result();
        result.Read(iprot_);
        iprot_.ReadMessageEnd();
        if (result.__isset.success) {
          return result.Success;
        }
        throw new TApplicationException(TApplicationException.ExceptionType.MissingResult, "SharedTextureRequest failed: unknown result");
      }

    }
    public class Processor : TProcessor {
      public Processor(ISyncServer iface)
      {
        iface_ = iface;
        processMap_["validate_ping"] = validate_ping_Process;
        processMap_["validate_function_list"] = validate_function_list_Process;
        processMap_["SharedTextureNotify"] = SharedTextureNotify_Process;
        processMap_["SharedTextureRequest"] = SharedTextureRequest_Process;
      }

      protected delegate void ProcessFunction(int seqid, TProtocol iprot, TProtocol oprot);
      private ISyncServer iface_;
      protected Dictionary<string, ProcessFunction> processMap_ = new Dictionary<string, ProcessFunction>();

      public bool Process(TProtocol iprot, TProtocol oprot)
      {
        try
        {
          TMessage msg = iprot.ReadMessageBegin();
          ProcessFunction fn;
          processMap_.TryGetValue(msg.Name, out fn);
          if (fn == null) {
            TProtocolUtil.Skip(iprot, TType.Struct);
            iprot.ReadMessageEnd();
            TApplicationException x = new TApplicationException (TApplicationException.ExceptionType.UnknownMethod, "Invalid method name: '" + msg.Name + "'");
            oprot.WriteMessageBegin(new TMessage(msg.Name, TMessageType.Exception, msg.SeqID));
            x.Write(oprot);
            oprot.WriteMessageEnd();
            oprot.Transport.Flush();
            return true;
          }
          fn(msg.SeqID, iprot, oprot);
        }
        catch (IOException)
        {
          return false;
        }
        return true;
      }

      public void validate_ping_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        validate_ping_args args = new validate_ping_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        validate_ping_result result = new validate_ping_result();
        try
        {
          iface_.validate_ping();
          oprot.WriteMessageBegin(new TMessage("validate_ping", TMessageType.Reply, seqid)); 
          result.Write(oprot);
        }
        catch (TTransportException)
        {
          throw;
        }
        catch (Exception ex)
        {
          Console.Error.WriteLine("Error occurred in processor:");
          Console.Error.WriteLine(ex.ToString());
          TApplicationException x = new TApplicationException        (TApplicationException.ExceptionType.InternalError," Internal error.");
          oprot.WriteMessageBegin(new TMessage("validate_ping", TMessageType.Exception, seqid));
          x.Write(oprot);
        }
        oprot.WriteMessageEnd();
        oprot.Transport.Flush();
      }

      public void validate_function_list_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        validate_function_list_args args = new validate_function_list_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        validate_function_list_result result = new validate_function_list_result();
        try
        {
          result.Success = iface_.validate_function_list();
          oprot.WriteMessageBegin(new TMessage("validate_function_list", TMessageType.Reply, seqid)); 
          result.Write(oprot);
        }
        catch (TTransportException)
        {
          throw;
        }
        catch (Exception ex)
        {
          Console.Error.WriteLine("Error occurred in processor:");
          Console.Error.WriteLine(ex.ToString());
          TApplicationException x = new TApplicationException        (TApplicationException.ExceptionType.InternalError," Internal error.");
          oprot.WriteMessageBegin(new TMessage("validate_function_list", TMessageType.Exception, seqid));
          x.Write(oprot);
        }
        oprot.WriteMessageEnd();
        oprot.Transport.Flush();
      }

      public void SharedTextureNotify_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        SharedTextureNotify_args args = new SharedTextureNotify_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        SharedTextureNotify_result result = new SharedTextureNotify_result();
        try
        {
          iface_.SharedTextureNotify(args.TexId, args.Texture);
          oprot.WriteMessageBegin(new TMessage("SharedTextureNotify", TMessageType.Reply, seqid)); 
          result.Write(oprot);
        }
        catch (TTransportException)
        {
          throw;
        }
        catch (Exception ex)
        {
          Console.Error.WriteLine("Error occurred in processor:");
          Console.Error.WriteLine(ex.ToString());
          TApplicationException x = new TApplicationException        (TApplicationException.ExceptionType.InternalError," Internal error.");
          oprot.WriteMessageBegin(new TMessage("SharedTextureNotify", TMessageType.Exception, seqid));
          x.Write(oprot);
        }
        oprot.WriteMessageEnd();
        oprot.Transport.Flush();
      }

      public void SharedTextureRequest_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        SharedTextureRequest_args args = new SharedTextureRequest_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        SharedTextureRequest_result result = new SharedTextureRequest_result();
        try
        {
          result.Success = iface_.SharedTextureRequest(args.TexId);
          oprot.WriteMessageBegin(new TMessage("SharedTextureRequest", TMessageType.Reply, seqid)); 
          result.Write(oprot);
        }
        catch (TTransportException)
        {
          throw;
        }
        catch (Exception ex)
        {
          Console.Error.WriteLine("Error occurred in processor:");
          Console.Error.WriteLine(ex.ToString());
          TApplicationException x = new TApplicationException        (TApplicationException.ExceptionType.InternalError," Internal error.");
          oprot.WriteMessageBegin(new TMessage("SharedTextureRequest", TMessageType.Exception, seqid));
          x.Write(oprot);
        }
        oprot.WriteMessageEnd();
        oprot.Transport.Flush();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class validate_ping_args : TBase
    {

      public validate_ping_args() {
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
          TStruct struc = new TStruct("validate_ping_args");
          oprot.WriteStructBegin(struc);
          oprot.WriteFieldStop();
          oprot.WriteStructEnd();
        }
        finally
        {
          oprot.DecrementRecursionDepth();
        }
      }

      public override string ToString() {
        StringBuilder __sb = new StringBuilder("validate_ping_args(");
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class validate_ping_result : TBase
    {

      public validate_ping_result() {
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
          TStruct struc = new TStruct("validate_ping_result");
          oprot.WriteStructBegin(struc);

          oprot.WriteFieldStop();
          oprot.WriteStructEnd();
        }
        finally
        {
          oprot.DecrementRecursionDepth();
        }
      }

      public override string ToString() {
        StringBuilder __sb = new StringBuilder("validate_ping_result(");
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class validate_function_list_args : TBase
    {

      public validate_function_list_args() {
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
          TStruct struc = new TStruct("validate_function_list_args");
          oprot.WriteStructBegin(struc);
          oprot.WriteFieldStop();
          oprot.WriteStructEnd();
        }
        finally
        {
          oprot.DecrementRecursionDepth();
        }
      }

      public override string ToString() {
        StringBuilder __sb = new StringBuilder("validate_function_list_args(");
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class validate_function_list_result : TBase
    {
      private List<string> _success;

      public List<string> Success
      {
        get
        {
          return _success;
        }
        set
        {
          __isset.success = true;
          this._success = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool success;
      }

      public validate_function_list_result() {
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
              case 0:
                if (field.Type == TType.List) {
                  {
                    Success = new List<string>();
                    TList _list0 = iprot.ReadListBegin();
                    for( int _i1 = 0; _i1 < _list0.Count; ++_i1)
                    {
                      string _elem2;
                      _elem2 = iprot.ReadString();
                      Success.Add(_elem2);
                    }
                    iprot.ReadListEnd();
                  }
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
          TStruct struc = new TStruct("validate_function_list_result");
          oprot.WriteStructBegin(struc);
          TField field = new TField();

          if (this.__isset.success) {
            if (Success != null) {
              field.Name = "Success";
              field.Type = TType.List;
              field.ID = 0;
              oprot.WriteFieldBegin(field);
              {
                oprot.WriteListBegin(new TList(TType.String, Success.Count));
                foreach (string _iter3 in Success)
                {
                  oprot.WriteString(_iter3);
                }
                oprot.WriteListEnd();
              }
              oprot.WriteFieldEnd();
            }
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
        StringBuilder __sb = new StringBuilder("validate_function_list_result(");
        bool __first = true;
        if (Success != null && __isset.success) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("Success: ");
          __sb.Append(Success);
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class SharedTextureNotify_args : TBase
    {
      private string _texId;
      private SharedTex _texture;

      public string TexId
      {
        get
        {
          return _texId;
        }
        set
        {
          __isset.texId = true;
          this._texId = value;
        }
      }

      public SharedTex Texture
      {
        get
        {
          return _texture;
        }
        set
        {
          __isset.texture = true;
          this._texture = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool texId;
        public bool texture;
      }

      public SharedTextureNotify_args() {
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
                  TexId = iprot.ReadString();
                } else { 
                  TProtocolUtil.Skip(iprot, field.Type);
                }
                break;
              case 2:
                if (field.Type == TType.Struct) {
                  Texture = new SharedTex();
                  Texture.Read(iprot);
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
          TStruct struc = new TStruct("SharedTextureNotify_args");
          oprot.WriteStructBegin(struc);
          TField field = new TField();
          if (TexId != null && __isset.texId) {
            field.Name = "texId";
            field.Type = TType.String;
            field.ID = 1;
            oprot.WriteFieldBegin(field);
            oprot.WriteString(TexId);
            oprot.WriteFieldEnd();
          }
          if (Texture != null && __isset.texture) {
            field.Name = "texture";
            field.Type = TType.Struct;
            field.ID = 2;
            oprot.WriteFieldBegin(field);
            Texture.Write(oprot);
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
        StringBuilder __sb = new StringBuilder("SharedTextureNotify_args(");
        bool __first = true;
        if (TexId != null && __isset.texId) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("TexId: ");
          __sb.Append(TexId);
        }
        if (Texture != null && __isset.texture) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("Texture: ");
          __sb.Append(Texture== null ? "<null>" : Texture.ToString());
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class SharedTextureNotify_result : TBase
    {

      public SharedTextureNotify_result() {
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
          TStruct struc = new TStruct("SharedTextureNotify_result");
          oprot.WriteStructBegin(struc);

          oprot.WriteFieldStop();
          oprot.WriteStructEnd();
        }
        finally
        {
          oprot.DecrementRecursionDepth();
        }
      }

      public override string ToString() {
        StringBuilder __sb = new StringBuilder("SharedTextureNotify_result(");
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class SharedTextureRequest_args : TBase
    {
      private string _texId;

      public string TexId
      {
        get
        {
          return _texId;
        }
        set
        {
          __isset.texId = true;
          this._texId = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool texId;
      }

      public SharedTextureRequest_args() {
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
                  TexId = iprot.ReadString();
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
          TStruct struc = new TStruct("SharedTextureRequest_args");
          oprot.WriteStructBegin(struc);
          TField field = new TField();
          if (TexId != null && __isset.texId) {
            field.Name = "texId";
            field.Type = TType.String;
            field.ID = 1;
            oprot.WriteFieldBegin(field);
            oprot.WriteString(TexId);
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
        StringBuilder __sb = new StringBuilder("SharedTextureRequest_args(");
        bool __first = true;
        if (TexId != null && __isset.texId) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("TexId: ");
          __sb.Append(TexId);
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class SharedTextureRequest_result : TBase
    {
      private SharedTex _success;

      public SharedTex Success
      {
        get
        {
          return _success;
        }
        set
        {
          __isset.success = true;
          this._success = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool success;
      }

      public SharedTextureRequest_result() {
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
              case 0:
                if (field.Type == TType.Struct) {
                  Success = new SharedTex();
                  Success.Read(iprot);
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
          TStruct struc = new TStruct("SharedTextureRequest_result");
          oprot.WriteStructBegin(struc);
          TField field = new TField();

          if (this.__isset.success) {
            if (Success != null) {
              field.Name = "Success";
              field.Type = TType.Struct;
              field.ID = 0;
              oprot.WriteFieldBegin(field);
              Success.Write(oprot);
              oprot.WriteFieldEnd();
            }
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
        StringBuilder __sb = new StringBuilder("SharedTextureRequest_result(");
        bool __first = true;
        if (Success != null && __isset.success) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("Success: ");
          __sb.Append(Success== null ? "<null>" : Success.ToString());
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }

  }
}
#endif
