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
  public partial class SDK_Service {
    public interface ISyncServer {
      void validate_ping();
      List<string> validate_function_list();
      void NotifySdkStarted(BlueprintReality.MixCast.Data.ExperienceMetadata expData);
      void NotifySdkStopped();
      void UpdateCameraActive(string cameraId, bool active);
      void UpdateExperienceTrackedObjectMetadata(List<BlueprintReality.MixCast.Data.TrackedObject> trackedObjects);
      void UpdateExperienceTrackedObjectPoses(List<BlueprintReality.MixCast.Thrift.Pose> poses);
      void SendExperienceEvent(string eventId);
      void RequestTakeSnapshot(string cameraId);
      void RequestSetVideoRecording(string cameraId, bool enableVideoRecording);
      void RequestSetAutoSnapshot(string cameraId, bool enableAutoSnapshot);
      void RequestSetVideoStreaming(string cameraId, bool enableVideoStreaming);
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
      IAsyncResult Begin_NotifySdkStarted(AsyncCallback callback, object state, BlueprintReality.MixCast.Data.ExperienceMetadata expData);
      void End_NotifySdkStarted(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_NotifySdkStopped(AsyncCallback callback, object state);
      void End_NotifySdkStopped(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_UpdateCameraActive(AsyncCallback callback, object state, string cameraId, bool active);
      void End_UpdateCameraActive(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_UpdateExperienceTrackedObjectMetadata(AsyncCallback callback, object state, List<BlueprintReality.MixCast.Data.TrackedObject> trackedObjects);
      void End_UpdateExperienceTrackedObjectMetadata(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_UpdateExperienceTrackedObjectPoses(AsyncCallback callback, object state, List<BlueprintReality.MixCast.Thrift.Pose> poses);
      void End_UpdateExperienceTrackedObjectPoses(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_SendExperienceEvent(AsyncCallback callback, object state, string eventId);
      void End_SendExperienceEvent(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_RequestTakeSnapshot(AsyncCallback callback, object state, string cameraId);
      void End_RequestTakeSnapshot(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_RequestSetVideoRecording(AsyncCallback callback, object state, string cameraId, bool enableVideoRecording);
      void End_RequestSetVideoRecording(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_RequestSetAutoSnapshot(AsyncCallback callback, object state, string cameraId, bool enableAutoSnapshot);
      void End_RequestSetAutoSnapshot(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_RequestSetVideoStreaming(AsyncCallback callback, object state, string cameraId, bool enableVideoStreaming);
      void End_RequestSetVideoStreaming(IAsyncResult asyncResult);
      #endif
    }

    public interface ISyncClient {
      bool TryNotifySdkStarted(BlueprintReality.MixCast.Data.ExperienceMetadata expData);
      bool TryNotifySdkStopped();
      bool TryUpdateCameraActive(string cameraId, bool active);
      bool TryUpdateExperienceTrackedObjectMetadata(List<BlueprintReality.MixCast.Data.TrackedObject> trackedObjects);
      bool TryUpdateExperienceTrackedObjectPoses(List<BlueprintReality.MixCast.Thrift.Pose> poses);
      bool TrySendExperienceEvent(string eventId);
      bool TryRequestTakeSnapshot(string cameraId);
      bool TryRequestSetVideoRecording(string cameraId, bool enableVideoRecording);
      bool TryRequestSetAutoSnapshot(string cameraId, bool enableAutoSnapshot);
      bool TryRequestSetVideoStreaming(string cameraId, bool enableVideoStreaming);
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
      IAsyncResult Begin_NotifySdkStarted(AsyncCallback callback, object state, BlueprintReality.MixCast.Data.ExperienceMetadata expData);
      void End_NotifySdkStarted(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_NotifySdkStopped(AsyncCallback callback, object state);
      void End_NotifySdkStopped(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_UpdateCameraActive(AsyncCallback callback, object state, string cameraId, bool active);
      void End_UpdateCameraActive(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_UpdateExperienceTrackedObjectMetadata(AsyncCallback callback, object state, List<BlueprintReality.MixCast.Data.TrackedObject> trackedObjects);
      void End_UpdateExperienceTrackedObjectMetadata(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_UpdateExperienceTrackedObjectPoses(AsyncCallback callback, object state, List<BlueprintReality.MixCast.Thrift.Pose> poses);
      void End_UpdateExperienceTrackedObjectPoses(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_SendExperienceEvent(AsyncCallback callback, object state, string eventId);
      void End_SendExperienceEvent(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_RequestTakeSnapshot(AsyncCallback callback, object state, string cameraId);
      void End_RequestTakeSnapshot(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_RequestSetVideoRecording(AsyncCallback callback, object state, string cameraId, bool enableVideoRecording);
      void End_RequestSetVideoRecording(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_RequestSetAutoSnapshot(AsyncCallback callback, object state, string cameraId, bool enableAutoSnapshot);
      void End_RequestSetAutoSnapshot(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_RequestSetVideoStreaming(AsyncCallback callback, object state, string cameraId, bool enableVideoStreaming);
      void End_RequestSetVideoStreaming(IAsyncResult asyncResult);
      #endif
    }

    public abstract class Handler : HandlerProxy, IFaceServer {
      public void validate_ping(){ }
      public List<string> validate_function_list(){ return new List<string>(){ "validate_ping", "validate_function_list", "NotifySdkStarted", "NotifySdkStopped", "UpdateCameraActive", "UpdateExperienceTrackedObjectMetadata", "UpdateExperienceTrackedObjectPoses", "SendExperienceEvent", "RequestTakeSnapshot", "RequestSetVideoRecording", "RequestSetAutoSnapshot", "RequestSetVideoStreaming", }; }
      public abstract void NotifySdkStarted(BlueprintReality.MixCast.Data.ExperienceMetadata expData);
      public abstract void NotifySdkStopped();
      public abstract void UpdateCameraActive(string cameraId, bool active);
      public abstract void UpdateExperienceTrackedObjectMetadata(List<BlueprintReality.MixCast.Data.TrackedObject> trackedObjects);
      public abstract void UpdateExperienceTrackedObjectPoses(List<BlueprintReality.MixCast.Thrift.Pose> poses);
      public abstract void SendExperienceEvent(string eventId);
      public abstract void RequestTakeSnapshot(string cameraId);
      public abstract void RequestSetVideoRecording(string cameraId, bool enableVideoRecording);
      public abstract void RequestSetAutoSnapshot(string cameraId, bool enableAutoSnapshot);
      public abstract void RequestSetVideoStreaming(string cameraId, bool enableVideoStreaming);
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
      public IAsyncResult Begin_NotifySdkStarted(AsyncCallback callback, object state, BlueprintReality.MixCast.Data.ExperienceMetadata expData)
      {
        return send_NotifySdkStarted(callback, state, expData);
      }

      public void End_NotifySdkStarted(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
      }

      #endif

      public bool TryNotifySdkStarted(BlueprintReality.MixCast.Data.ExperienceMetadata expData){ try { NotifySdkStarted(expData); return true; } catch { return false; } }

      protected void NotifySdkStarted(BlueprintReality.MixCast.Data.ExperienceMetadata expData)
      {
        #if !SILVERLIGHT
        send_NotifySdkStarted(expData);

        #else
        var asyncResult = Begin_NotifySdkStarted(null, null, expData);

        #endif
      }
      #if SILVERLIGHT
      protected IAsyncResult send_NotifySdkStarted(AsyncCallback callback, object state, BlueprintReality.MixCast.Data.ExperienceMetadata expData)
      #else
      protected void send_NotifySdkStarted(BlueprintReality.MixCast.Data.ExperienceMetadata expData)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("NotifySdkStarted", TMessageType.Oneway, seqid_));
        NotifySdkStarted_args args = new NotifySdkStarted_args();
        args.ExpData = expData;
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_NotifySdkStopped(AsyncCallback callback, object state)
      {
        return send_NotifySdkStopped(callback, state);
      }

      public void End_NotifySdkStopped(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
      }

      #endif

      public bool TryNotifySdkStopped(){ try { NotifySdkStopped(); return true; } catch { return false; } }

      protected void NotifySdkStopped()
      {
        #if !SILVERLIGHT
        send_NotifySdkStopped();

        #else
        var asyncResult = Begin_NotifySdkStopped(null, null);

        #endif
      }
      #if SILVERLIGHT
      protected IAsyncResult send_NotifySdkStopped(AsyncCallback callback, object state)
      #else
      protected void send_NotifySdkStopped()
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("NotifySdkStopped", TMessageType.Oneway, seqid_));
        NotifySdkStopped_args args = new NotifySdkStopped_args();
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_UpdateCameraActive(AsyncCallback callback, object state, string cameraId, bool active)
      {
        return send_UpdateCameraActive(callback, state, cameraId, active);
      }

      public void End_UpdateCameraActive(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
      }

      #endif

      public bool TryUpdateCameraActive(string cameraId, bool active){ try { UpdateCameraActive(cameraId, active); return true; } catch { return false; } }

      protected void UpdateCameraActive(string cameraId, bool active)
      {
        #if !SILVERLIGHT
        send_UpdateCameraActive(cameraId, active);

        #else
        var asyncResult = Begin_UpdateCameraActive(null, null, cameraId, active);

        #endif
      }
      #if SILVERLIGHT
      protected IAsyncResult send_UpdateCameraActive(AsyncCallback callback, object state, string cameraId, bool active)
      #else
      protected void send_UpdateCameraActive(string cameraId, bool active)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("UpdateCameraActive", TMessageType.Oneway, seqid_));
        UpdateCameraActive_args args = new UpdateCameraActive_args();
        args.CameraId = cameraId;
        args.Active = active;
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_UpdateExperienceTrackedObjectMetadata(AsyncCallback callback, object state, List<BlueprintReality.MixCast.Data.TrackedObject> trackedObjects)
      {
        return send_UpdateExperienceTrackedObjectMetadata(callback, state, trackedObjects);
      }

      public void End_UpdateExperienceTrackedObjectMetadata(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
      }

      #endif

      public bool TryUpdateExperienceTrackedObjectMetadata(List<BlueprintReality.MixCast.Data.TrackedObject> trackedObjects){ try { UpdateExperienceTrackedObjectMetadata(trackedObjects); return true; } catch { return false; } }

      protected void UpdateExperienceTrackedObjectMetadata(List<BlueprintReality.MixCast.Data.TrackedObject> trackedObjects)
      {
        #if !SILVERLIGHT
        send_UpdateExperienceTrackedObjectMetadata(trackedObjects);

        #else
        var asyncResult = Begin_UpdateExperienceTrackedObjectMetadata(null, null, trackedObjects);

        #endif
      }
      #if SILVERLIGHT
      protected IAsyncResult send_UpdateExperienceTrackedObjectMetadata(AsyncCallback callback, object state, List<BlueprintReality.MixCast.Data.TrackedObject> trackedObjects)
      #else
      protected void send_UpdateExperienceTrackedObjectMetadata(List<BlueprintReality.MixCast.Data.TrackedObject> trackedObjects)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("UpdateExperienceTrackedObjectMetadata", TMessageType.Oneway, seqid_));
        UpdateExperienceTrackedObjectMetadata_args args = new UpdateExperienceTrackedObjectMetadata_args();
        args.TrackedObjects = trackedObjects;
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_UpdateExperienceTrackedObjectPoses(AsyncCallback callback, object state, List<BlueprintReality.MixCast.Thrift.Pose> poses)
      {
        return send_UpdateExperienceTrackedObjectPoses(callback, state, poses);
      }

      public void End_UpdateExperienceTrackedObjectPoses(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
      }

      #endif

      public bool TryUpdateExperienceTrackedObjectPoses(List<BlueprintReality.MixCast.Thrift.Pose> poses){ try { UpdateExperienceTrackedObjectPoses(poses); return true; } catch { return false; } }

      protected void UpdateExperienceTrackedObjectPoses(List<BlueprintReality.MixCast.Thrift.Pose> poses)
      {
        #if !SILVERLIGHT
        send_UpdateExperienceTrackedObjectPoses(poses);

        #else
        var asyncResult = Begin_UpdateExperienceTrackedObjectPoses(null, null, poses);

        #endif
      }
      #if SILVERLIGHT
      protected IAsyncResult send_UpdateExperienceTrackedObjectPoses(AsyncCallback callback, object state, List<BlueprintReality.MixCast.Thrift.Pose> poses)
      #else
      protected void send_UpdateExperienceTrackedObjectPoses(List<BlueprintReality.MixCast.Thrift.Pose> poses)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("UpdateExperienceTrackedObjectPoses", TMessageType.Oneway, seqid_));
        UpdateExperienceTrackedObjectPoses_args args = new UpdateExperienceTrackedObjectPoses_args();
        args.Poses = poses;
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_SendExperienceEvent(AsyncCallback callback, object state, string eventId)
      {
        return send_SendExperienceEvent(callback, state, eventId);
      }

      public void End_SendExperienceEvent(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
      }

      #endif

      public bool TrySendExperienceEvent(string eventId){ try { SendExperienceEvent(eventId); return true; } catch { return false; } }

      protected void SendExperienceEvent(string eventId)
      {
        #if !SILVERLIGHT
        send_SendExperienceEvent(eventId);

        #else
        var asyncResult = Begin_SendExperienceEvent(null, null, eventId);

        #endif
      }
      #if SILVERLIGHT
      protected IAsyncResult send_SendExperienceEvent(AsyncCallback callback, object state, string eventId)
      #else
      protected void send_SendExperienceEvent(string eventId)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("SendExperienceEvent", TMessageType.Oneway, seqid_));
        SendExperienceEvent_args args = new SendExperienceEvent_args();
        args.EventId = eventId;
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_RequestTakeSnapshot(AsyncCallback callback, object state, string cameraId)
      {
        return send_RequestTakeSnapshot(callback, state, cameraId);
      }

      public void End_RequestTakeSnapshot(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
      }

      #endif

      public bool TryRequestTakeSnapshot(string cameraId){ try { RequestTakeSnapshot(cameraId); return true; } catch { return false; } }

      protected void RequestTakeSnapshot(string cameraId)
      {
        #if !SILVERLIGHT
        send_RequestTakeSnapshot(cameraId);

        #else
        var asyncResult = Begin_RequestTakeSnapshot(null, null, cameraId);

        #endif
      }
      #if SILVERLIGHT
      protected IAsyncResult send_RequestTakeSnapshot(AsyncCallback callback, object state, string cameraId)
      #else
      protected void send_RequestTakeSnapshot(string cameraId)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("RequestTakeSnapshot", TMessageType.Oneway, seqid_));
        RequestTakeSnapshot_args args = new RequestTakeSnapshot_args();
        args.CameraId = cameraId;
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_RequestSetVideoRecording(AsyncCallback callback, object state, string cameraId, bool enableVideoRecording)
      {
        return send_RequestSetVideoRecording(callback, state, cameraId, enableVideoRecording);
      }

      public void End_RequestSetVideoRecording(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
      }

      #endif

      public bool TryRequestSetVideoRecording(string cameraId, bool enableVideoRecording){ try { RequestSetVideoRecording(cameraId, enableVideoRecording); return true; } catch { return false; } }

      protected void RequestSetVideoRecording(string cameraId, bool enableVideoRecording)
      {
        #if !SILVERLIGHT
        send_RequestSetVideoRecording(cameraId, enableVideoRecording);

        #else
        var asyncResult = Begin_RequestSetVideoRecording(null, null, cameraId, enableVideoRecording);

        #endif
      }
      #if SILVERLIGHT
      protected IAsyncResult send_RequestSetVideoRecording(AsyncCallback callback, object state, string cameraId, bool enableVideoRecording)
      #else
      protected void send_RequestSetVideoRecording(string cameraId, bool enableVideoRecording)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("RequestSetVideoRecording", TMessageType.Oneway, seqid_));
        RequestSetVideoRecording_args args = new RequestSetVideoRecording_args();
        args.CameraId = cameraId;
        args.EnableVideoRecording = enableVideoRecording;
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_RequestSetAutoSnapshot(AsyncCallback callback, object state, string cameraId, bool enableAutoSnapshot)
      {
        return send_RequestSetAutoSnapshot(callback, state, cameraId, enableAutoSnapshot);
      }

      public void End_RequestSetAutoSnapshot(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
      }

      #endif

      public bool TryRequestSetAutoSnapshot(string cameraId, bool enableAutoSnapshot){ try { RequestSetAutoSnapshot(cameraId, enableAutoSnapshot); return true; } catch { return false; } }

      protected void RequestSetAutoSnapshot(string cameraId, bool enableAutoSnapshot)
      {
        #if !SILVERLIGHT
        send_RequestSetAutoSnapshot(cameraId, enableAutoSnapshot);

        #else
        var asyncResult = Begin_RequestSetAutoSnapshot(null, null, cameraId, enableAutoSnapshot);

        #endif
      }
      #if SILVERLIGHT
      protected IAsyncResult send_RequestSetAutoSnapshot(AsyncCallback callback, object state, string cameraId, bool enableAutoSnapshot)
      #else
      protected void send_RequestSetAutoSnapshot(string cameraId, bool enableAutoSnapshot)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("RequestSetAutoSnapshot", TMessageType.Oneway, seqid_));
        RequestSetAutoSnapshot_args args = new RequestSetAutoSnapshot_args();
        args.CameraId = cameraId;
        args.EnableAutoSnapshot = enableAutoSnapshot;
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_RequestSetVideoStreaming(AsyncCallback callback, object state, string cameraId, bool enableVideoStreaming)
      {
        return send_RequestSetVideoStreaming(callback, state, cameraId, enableVideoStreaming);
      }

      public void End_RequestSetVideoStreaming(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
      }

      #endif

      public bool TryRequestSetVideoStreaming(string cameraId, bool enableVideoStreaming){ try { RequestSetVideoStreaming(cameraId, enableVideoStreaming); return true; } catch { return false; } }

      protected void RequestSetVideoStreaming(string cameraId, bool enableVideoStreaming)
      {
        #if !SILVERLIGHT
        send_RequestSetVideoStreaming(cameraId, enableVideoStreaming);

        #else
        var asyncResult = Begin_RequestSetVideoStreaming(null, null, cameraId, enableVideoStreaming);

        #endif
      }
      #if SILVERLIGHT
      protected IAsyncResult send_RequestSetVideoStreaming(AsyncCallback callback, object state, string cameraId, bool enableVideoStreaming)
      #else
      protected void send_RequestSetVideoStreaming(string cameraId, bool enableVideoStreaming)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("RequestSetVideoStreaming", TMessageType.Oneway, seqid_));
        RequestSetVideoStreaming_args args = new RequestSetVideoStreaming_args();
        args.CameraId = cameraId;
        args.EnableVideoStreaming = enableVideoStreaming;
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

    }
    public class Processor : TProcessor {
      public Processor(ISyncServer iface)
      {
        iface_ = iface;
        processMap_["validate_ping"] = validate_ping_Process;
        processMap_["validate_function_list"] = validate_function_list_Process;
        processMap_["NotifySdkStarted"] = NotifySdkStarted_Process;
        processMap_["NotifySdkStopped"] = NotifySdkStopped_Process;
        processMap_["UpdateCameraActive"] = UpdateCameraActive_Process;
        processMap_["UpdateExperienceTrackedObjectMetadata"] = UpdateExperienceTrackedObjectMetadata_Process;
        processMap_["UpdateExperienceTrackedObjectPoses"] = UpdateExperienceTrackedObjectPoses_Process;
        processMap_["SendExperienceEvent"] = SendExperienceEvent_Process;
        processMap_["RequestTakeSnapshot"] = RequestTakeSnapshot_Process;
        processMap_["RequestSetVideoRecording"] = RequestSetVideoRecording_Process;
        processMap_["RequestSetAutoSnapshot"] = RequestSetAutoSnapshot_Process;
        processMap_["RequestSetVideoStreaming"] = RequestSetVideoStreaming_Process;
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

      public void NotifySdkStarted_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        NotifySdkStarted_args args = new NotifySdkStarted_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        try
        {
          iface_.NotifySdkStarted(args.ExpData);
        }
        catch (TTransportException)
        {
          throw;
        }
        catch (Exception ex)
        {
          Console.Error.WriteLine("Error occurred in processor:");
          Console.Error.WriteLine(ex.ToString());
        }
      }

      public void NotifySdkStopped_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        NotifySdkStopped_args args = new NotifySdkStopped_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        try
        {
          iface_.NotifySdkStopped();
        }
        catch (TTransportException)
        {
          throw;
        }
        catch (Exception ex)
        {
          Console.Error.WriteLine("Error occurred in processor:");
          Console.Error.WriteLine(ex.ToString());
        }
      }

      public void UpdateCameraActive_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        UpdateCameraActive_args args = new UpdateCameraActive_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        try
        {
          iface_.UpdateCameraActive(args.CameraId, args.Active);
        }
        catch (TTransportException)
        {
          throw;
        }
        catch (Exception ex)
        {
          Console.Error.WriteLine("Error occurred in processor:");
          Console.Error.WriteLine(ex.ToString());
        }
      }

      public void UpdateExperienceTrackedObjectMetadata_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        UpdateExperienceTrackedObjectMetadata_args args = new UpdateExperienceTrackedObjectMetadata_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        try
        {
          iface_.UpdateExperienceTrackedObjectMetadata(args.TrackedObjects);
        }
        catch (TTransportException)
        {
          throw;
        }
        catch (Exception ex)
        {
          Console.Error.WriteLine("Error occurred in processor:");
          Console.Error.WriteLine(ex.ToString());
        }
      }

      public void UpdateExperienceTrackedObjectPoses_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        UpdateExperienceTrackedObjectPoses_args args = new UpdateExperienceTrackedObjectPoses_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        try
        {
          iface_.UpdateExperienceTrackedObjectPoses(args.Poses);
        }
        catch (TTransportException)
        {
          throw;
        }
        catch (Exception ex)
        {
          Console.Error.WriteLine("Error occurred in processor:");
          Console.Error.WriteLine(ex.ToString());
        }
      }

      public void SendExperienceEvent_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        SendExperienceEvent_args args = new SendExperienceEvent_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        try
        {
          iface_.SendExperienceEvent(args.EventId);
        }
        catch (TTransportException)
        {
          throw;
        }
        catch (Exception ex)
        {
          Console.Error.WriteLine("Error occurred in processor:");
          Console.Error.WriteLine(ex.ToString());
        }
      }

      public void RequestTakeSnapshot_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        RequestTakeSnapshot_args args = new RequestTakeSnapshot_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        try
        {
          iface_.RequestTakeSnapshot(args.CameraId);
        }
        catch (TTransportException)
        {
          throw;
        }
        catch (Exception ex)
        {
          Console.Error.WriteLine("Error occurred in processor:");
          Console.Error.WriteLine(ex.ToString());
        }
      }

      public void RequestSetVideoRecording_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        RequestSetVideoRecording_args args = new RequestSetVideoRecording_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        try
        {
          iface_.RequestSetVideoRecording(args.CameraId, args.EnableVideoRecording);
        }
        catch (TTransportException)
        {
          throw;
        }
        catch (Exception ex)
        {
          Console.Error.WriteLine("Error occurred in processor:");
          Console.Error.WriteLine(ex.ToString());
        }
      }

      public void RequestSetAutoSnapshot_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        RequestSetAutoSnapshot_args args = new RequestSetAutoSnapshot_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        try
        {
          iface_.RequestSetAutoSnapshot(args.CameraId, args.EnableAutoSnapshot);
        }
        catch (TTransportException)
        {
          throw;
        }
        catch (Exception ex)
        {
          Console.Error.WriteLine("Error occurred in processor:");
          Console.Error.WriteLine(ex.ToString());
        }
      }

      public void RequestSetVideoStreaming_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        RequestSetVideoStreaming_args args = new RequestSetVideoStreaming_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        try
        {
          iface_.RequestSetVideoStreaming(args.CameraId, args.EnableVideoStreaming);
        }
        catch (TTransportException)
        {
          throw;
        }
        catch (Exception ex)
        {
          Console.Error.WriteLine("Error occurred in processor:");
          Console.Error.WriteLine(ex.ToString());
        }
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
                    TList _list16 = iprot.ReadListBegin();
                    for( int _i17 = 0; _i17 < _list16.Count; ++_i17)
                    {
                      string _elem18;
                      _elem18 = iprot.ReadString();
                      Success.Add(_elem18);
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
                foreach (string _iter19 in Success)
                {
                  oprot.WriteString(_iter19);
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
    public partial class NotifySdkStarted_args : TBase
    {
      private BlueprintReality.MixCast.Data.ExperienceMetadata _expData;

      public BlueprintReality.MixCast.Data.ExperienceMetadata ExpData
      {
        get
        {
          return _expData;
        }
        set
        {
          __isset.expData = true;
          this._expData = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool expData;
      }

      public NotifySdkStarted_args() {
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
                if (field.Type == TType.Struct) {
                  ExpData = new BlueprintReality.MixCast.Data.ExperienceMetadata();
                  ExpData.Read(iprot);
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
          TStruct struc = new TStruct("NotifySdkStarted_args");
          oprot.WriteStructBegin(struc);
          TField field = new TField();
          if (ExpData != null && __isset.expData) {
            field.Name = "expData";
            field.Type = TType.Struct;
            field.ID = 1;
            oprot.WriteFieldBegin(field);
            ExpData.Write(oprot);
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
        StringBuilder __sb = new StringBuilder("NotifySdkStarted_args(");
        bool __first = true;
        if (ExpData != null && __isset.expData) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("ExpData: ");
          __sb.Append(ExpData== null ? "<null>" : ExpData.ToString());
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class NotifySdkStopped_args : TBase
    {

      public NotifySdkStopped_args() {
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
          TStruct struc = new TStruct("NotifySdkStopped_args");
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
        StringBuilder __sb = new StringBuilder("NotifySdkStopped_args(");
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class UpdateCameraActive_args : TBase
    {
      private string _cameraId;
      private bool _active;

      public string CameraId
      {
        get
        {
          return _cameraId;
        }
        set
        {
          __isset.cameraId = true;
          this._cameraId = value;
        }
      }

      public bool Active
      {
        get
        {
          return _active;
        }
        set
        {
          __isset.active = true;
          this._active = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool cameraId;
        public bool active;
      }

      public UpdateCameraActive_args() {
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
                  CameraId = iprot.ReadString();
                } else { 
                  TProtocolUtil.Skip(iprot, field.Type);
                }
                break;
              case 2:
                if (field.Type == TType.Bool) {
                  Active = iprot.ReadBool();
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
          TStruct struc = new TStruct("UpdateCameraActive_args");
          oprot.WriteStructBegin(struc);
          TField field = new TField();
          if (CameraId != null && __isset.cameraId) {
            field.Name = "cameraId";
            field.Type = TType.String;
            field.ID = 1;
            oprot.WriteFieldBegin(field);
            oprot.WriteString(CameraId);
            oprot.WriteFieldEnd();
          }
          if (__isset.active) {
            field.Name = "active";
            field.Type = TType.Bool;
            field.ID = 2;
            oprot.WriteFieldBegin(field);
            oprot.WriteBool(Active);
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
        StringBuilder __sb = new StringBuilder("UpdateCameraActive_args(");
        bool __first = true;
        if (CameraId != null && __isset.cameraId) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("CameraId: ");
          __sb.Append(CameraId);
        }
        if (__isset.active) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("Active: ");
          __sb.Append(Active);
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class UpdateExperienceTrackedObjectMetadata_args : TBase
    {
      private List<BlueprintReality.MixCast.Data.TrackedObject> _trackedObjects;

      public List<BlueprintReality.MixCast.Data.TrackedObject> TrackedObjects
      {
        get
        {
          return _trackedObjects;
        }
        set
        {
          __isset.trackedObjects = true;
          this._trackedObjects = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool trackedObjects;
      }

      public UpdateExperienceTrackedObjectMetadata_args() {
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
                if (field.Type == TType.List) {
                  {
                    TrackedObjects = new List<BlueprintReality.MixCast.Data.TrackedObject>();
                    TList _list20 = iprot.ReadListBegin();
                    for( int _i21 = 0; _i21 < _list20.Count; ++_i21)
                    {
                      BlueprintReality.MixCast.Data.TrackedObject _elem22;
                      _elem22 = new BlueprintReality.MixCast.Data.TrackedObject();
                      _elem22.Read(iprot);
                      TrackedObjects.Add(_elem22);
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
          TStruct struc = new TStruct("UpdateExperienceTrackedObjectMetadata_args");
          oprot.WriteStructBegin(struc);
          TField field = new TField();
          if (TrackedObjects != null && __isset.trackedObjects) {
            field.Name = "trackedObjects";
            field.Type = TType.List;
            field.ID = 1;
            oprot.WriteFieldBegin(field);
            {
              oprot.WriteListBegin(new TList(TType.Struct, TrackedObjects.Count));
              foreach (BlueprintReality.MixCast.Data.TrackedObject _iter23 in TrackedObjects)
              {
                _iter23.Write(oprot);
              }
              oprot.WriteListEnd();
            }
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
        StringBuilder __sb = new StringBuilder("UpdateExperienceTrackedObjectMetadata_args(");
        bool __first = true;
        if (TrackedObjects != null && __isset.trackedObjects) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("TrackedObjects: ");
          __sb.Append(TrackedObjects);
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class UpdateExperienceTrackedObjectPoses_args : TBase
    {
      private List<BlueprintReality.MixCast.Thrift.Pose> _poses;

      public List<BlueprintReality.MixCast.Thrift.Pose> Poses
      {
        get
        {
          return _poses;
        }
        set
        {
          __isset.poses = true;
          this._poses = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool poses;
      }

      public UpdateExperienceTrackedObjectPoses_args() {
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
                if (field.Type == TType.List) {
                  {
                    Poses = new List<BlueprintReality.MixCast.Thrift.Pose>();
                    TList _list24 = iprot.ReadListBegin();
                    for( int _i25 = 0; _i25 < _list24.Count; ++_i25)
                    {
                      BlueprintReality.MixCast.Thrift.Pose _elem26;
                      _elem26 = new BlueprintReality.MixCast.Thrift.Pose();
                      _elem26.Read(iprot);
                      Poses.Add(_elem26);
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
          TStruct struc = new TStruct("UpdateExperienceTrackedObjectPoses_args");
          oprot.WriteStructBegin(struc);
          TField field = new TField();
          if (Poses != null && __isset.poses) {
            field.Name = "poses";
            field.Type = TType.List;
            field.ID = 1;
            oprot.WriteFieldBegin(field);
            {
              oprot.WriteListBegin(new TList(TType.Struct, Poses.Count));
              foreach (BlueprintReality.MixCast.Thrift.Pose _iter27 in Poses)
              {
                _iter27.Write(oprot);
              }
              oprot.WriteListEnd();
            }
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
        StringBuilder __sb = new StringBuilder("UpdateExperienceTrackedObjectPoses_args(");
        bool __first = true;
        if (Poses != null && __isset.poses) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("Poses: ");
          __sb.Append(Poses);
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class SendExperienceEvent_args : TBase
    {
      private string _eventId;

      public string EventId
      {
        get
        {
          return _eventId;
        }
        set
        {
          __isset.eventId = true;
          this._eventId = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool eventId;
      }

      public SendExperienceEvent_args() {
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
                  EventId = iprot.ReadString();
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
          TStruct struc = new TStruct("SendExperienceEvent_args");
          oprot.WriteStructBegin(struc);
          TField field = new TField();
          if (EventId != null && __isset.eventId) {
            field.Name = "eventId";
            field.Type = TType.String;
            field.ID = 1;
            oprot.WriteFieldBegin(field);
            oprot.WriteString(EventId);
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
        StringBuilder __sb = new StringBuilder("SendExperienceEvent_args(");
        bool __first = true;
        if (EventId != null && __isset.eventId) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("EventId: ");
          __sb.Append(EventId);
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class RequestTakeSnapshot_args : TBase
    {
      private string _cameraId;

      public string CameraId
      {
        get
        {
          return _cameraId;
        }
        set
        {
          __isset.cameraId = true;
          this._cameraId = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool cameraId;
      }

      public RequestTakeSnapshot_args() {
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
                  CameraId = iprot.ReadString();
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
          TStruct struc = new TStruct("RequestTakeSnapshot_args");
          oprot.WriteStructBegin(struc);
          TField field = new TField();
          if (CameraId != null && __isset.cameraId) {
            field.Name = "cameraId";
            field.Type = TType.String;
            field.ID = 1;
            oprot.WriteFieldBegin(field);
            oprot.WriteString(CameraId);
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
        StringBuilder __sb = new StringBuilder("RequestTakeSnapshot_args(");
        bool __first = true;
        if (CameraId != null && __isset.cameraId) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("CameraId: ");
          __sb.Append(CameraId);
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class RequestSetVideoRecording_args : TBase
    {
      private string _cameraId;
      private bool _enableVideoRecording;

      public string CameraId
      {
        get
        {
          return _cameraId;
        }
        set
        {
          __isset.cameraId = true;
          this._cameraId = value;
        }
      }

      public bool EnableVideoRecording
      {
        get
        {
          return _enableVideoRecording;
        }
        set
        {
          __isset.enableVideoRecording = true;
          this._enableVideoRecording = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool cameraId;
        public bool enableVideoRecording;
      }

      public RequestSetVideoRecording_args() {
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
                  CameraId = iprot.ReadString();
                } else { 
                  TProtocolUtil.Skip(iprot, field.Type);
                }
                break;
              case 2:
                if (field.Type == TType.Bool) {
                  EnableVideoRecording = iprot.ReadBool();
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
          TStruct struc = new TStruct("RequestSetVideoRecording_args");
          oprot.WriteStructBegin(struc);
          TField field = new TField();
          if (CameraId != null && __isset.cameraId) {
            field.Name = "cameraId";
            field.Type = TType.String;
            field.ID = 1;
            oprot.WriteFieldBegin(field);
            oprot.WriteString(CameraId);
            oprot.WriteFieldEnd();
          }
          if (__isset.enableVideoRecording) {
            field.Name = "enableVideoRecording";
            field.Type = TType.Bool;
            field.ID = 2;
            oprot.WriteFieldBegin(field);
            oprot.WriteBool(EnableVideoRecording);
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
        StringBuilder __sb = new StringBuilder("RequestSetVideoRecording_args(");
        bool __first = true;
        if (CameraId != null && __isset.cameraId) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("CameraId: ");
          __sb.Append(CameraId);
        }
        if (__isset.enableVideoRecording) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("EnableVideoRecording: ");
          __sb.Append(EnableVideoRecording);
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class RequestSetAutoSnapshot_args : TBase
    {
      private string _cameraId;
      private bool _enableAutoSnapshot;

      public string CameraId
      {
        get
        {
          return _cameraId;
        }
        set
        {
          __isset.cameraId = true;
          this._cameraId = value;
        }
      }

      public bool EnableAutoSnapshot
      {
        get
        {
          return _enableAutoSnapshot;
        }
        set
        {
          __isset.enableAutoSnapshot = true;
          this._enableAutoSnapshot = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool cameraId;
        public bool enableAutoSnapshot;
      }

      public RequestSetAutoSnapshot_args() {
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
                  CameraId = iprot.ReadString();
                } else { 
                  TProtocolUtil.Skip(iprot, field.Type);
                }
                break;
              case 2:
                if (field.Type == TType.Bool) {
                  EnableAutoSnapshot = iprot.ReadBool();
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
          TStruct struc = new TStruct("RequestSetAutoSnapshot_args");
          oprot.WriteStructBegin(struc);
          TField field = new TField();
          if (CameraId != null && __isset.cameraId) {
            field.Name = "cameraId";
            field.Type = TType.String;
            field.ID = 1;
            oprot.WriteFieldBegin(field);
            oprot.WriteString(CameraId);
            oprot.WriteFieldEnd();
          }
          if (__isset.enableAutoSnapshot) {
            field.Name = "enableAutoSnapshot";
            field.Type = TType.Bool;
            field.ID = 2;
            oprot.WriteFieldBegin(field);
            oprot.WriteBool(EnableAutoSnapshot);
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
        StringBuilder __sb = new StringBuilder("RequestSetAutoSnapshot_args(");
        bool __first = true;
        if (CameraId != null && __isset.cameraId) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("CameraId: ");
          __sb.Append(CameraId);
        }
        if (__isset.enableAutoSnapshot) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("EnableAutoSnapshot: ");
          __sb.Append(EnableAutoSnapshot);
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class RequestSetVideoStreaming_args : TBase
    {
      private string _cameraId;
      private bool _enableVideoStreaming;

      public string CameraId
      {
        get
        {
          return _cameraId;
        }
        set
        {
          __isset.cameraId = true;
          this._cameraId = value;
        }
      }

      public bool EnableVideoStreaming
      {
        get
        {
          return _enableVideoStreaming;
        }
        set
        {
          __isset.enableVideoStreaming = true;
          this._enableVideoStreaming = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool cameraId;
        public bool enableVideoStreaming;
      }

      public RequestSetVideoStreaming_args() {
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
                  CameraId = iprot.ReadString();
                } else { 
                  TProtocolUtil.Skip(iprot, field.Type);
                }
                break;
              case 2:
                if (field.Type == TType.Bool) {
                  EnableVideoStreaming = iprot.ReadBool();
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
          TStruct struc = new TStruct("RequestSetVideoStreaming_args");
          oprot.WriteStructBegin(struc);
          TField field = new TField();
          if (CameraId != null && __isset.cameraId) {
            field.Name = "cameraId";
            field.Type = TType.String;
            field.ID = 1;
            oprot.WriteFieldBegin(field);
            oprot.WriteString(CameraId);
            oprot.WriteFieldEnd();
          }
          if (__isset.enableVideoStreaming) {
            field.Name = "enableVideoStreaming";
            field.Type = TType.Bool;
            field.ID = 2;
            oprot.WriteFieldBegin(field);
            oprot.WriteBool(EnableVideoStreaming);
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
        StringBuilder __sb = new StringBuilder("RequestSetVideoStreaming_args(");
        bool __first = true;
        if (CameraId != null && __isset.cameraId) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("CameraId: ");
          __sb.Append(CameraId);
        }
        if (__isset.enableVideoStreaming) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("EnableVideoStreaming: ");
          __sb.Append(EnableVideoStreaming);
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }

  }
}
#endif
