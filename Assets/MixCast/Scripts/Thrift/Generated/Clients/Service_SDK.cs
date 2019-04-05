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
  public partial class Service_SDK {
    public interface ISyncServer {
      void validate_ping();
      List<string> validate_function_list();
      void SetActivationState(bool active);
      void NotifyServiceStarted();
      void UpdateLicenseMetadata(BlueprintReality.MixCast.Data.License license);
      void UpdateTrackedObjectMetadata(List<BlueprintReality.MixCast.Data.TrackedObject> trackedObjects);
      void UpdateCameraMetadata(List<BlueprintReality.MixCast.Data.VirtualCamera> cameras);
      void UpdateViewfinderMetadata(List<BlueprintReality.MixCast.Data.Viewfinder> viewfinders);
      void UpdateDesktopMetadata(BlueprintReality.MixCast.Data.Desktop desktop);
      void ResetWorldOrientation();
      void ModifyWorldOrientation(double degrees);
      void SendExperienceCommand(string eventId);
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
      IAsyncResult Begin_SetActivationState(AsyncCallback callback, object state, bool active);
      void End_SetActivationState(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_NotifyServiceStarted(AsyncCallback callback, object state);
      void End_NotifyServiceStarted(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_UpdateLicenseMetadata(AsyncCallback callback, object state, BlueprintReality.MixCast.Data.License license);
      void End_UpdateLicenseMetadata(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_UpdateTrackedObjectMetadata(AsyncCallback callback, object state, List<BlueprintReality.MixCast.Data.TrackedObject> trackedObjects);
      void End_UpdateTrackedObjectMetadata(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_UpdateCameraMetadata(AsyncCallback callback, object state, List<BlueprintReality.MixCast.Data.VirtualCamera> cameras);
      void End_UpdateCameraMetadata(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_UpdateViewfinderMetadata(AsyncCallback callback, object state, List<BlueprintReality.MixCast.Data.Viewfinder> viewfinders);
      void End_UpdateViewfinderMetadata(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_UpdateDesktopMetadata(AsyncCallback callback, object state, BlueprintReality.MixCast.Data.Desktop desktop);
      void End_UpdateDesktopMetadata(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_ResetWorldOrientation(AsyncCallback callback, object state);
      void End_ResetWorldOrientation(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_ModifyWorldOrientation(AsyncCallback callback, object state, double degrees);
      void End_ModifyWorldOrientation(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_SendExperienceCommand(AsyncCallback callback, object state, string eventId);
      void End_SendExperienceCommand(IAsyncResult asyncResult);
      #endif
    }

    public interface ISyncClient {
      bool TrySetActivationState(bool active);
      bool TryNotifyServiceStarted();
      bool TryUpdateLicenseMetadata(BlueprintReality.MixCast.Data.License license);
      bool TryUpdateTrackedObjectMetadata(List<BlueprintReality.MixCast.Data.TrackedObject> trackedObjects);
      bool TryUpdateCameraMetadata(List<BlueprintReality.MixCast.Data.VirtualCamera> cameras);
      bool TryUpdateViewfinderMetadata(List<BlueprintReality.MixCast.Data.Viewfinder> viewfinders);
      bool TryUpdateDesktopMetadata(BlueprintReality.MixCast.Data.Desktop desktop);
      bool TryResetWorldOrientation();
      bool TryModifyWorldOrientation(double degrees);
      bool TrySendExperienceCommand(string eventId);
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
      IAsyncResult Begin_SetActivationState(AsyncCallback callback, object state, bool active);
      void End_SetActivationState(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_NotifyServiceStarted(AsyncCallback callback, object state);
      void End_NotifyServiceStarted(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_UpdateLicenseMetadata(AsyncCallback callback, object state, BlueprintReality.MixCast.Data.License license);
      void End_UpdateLicenseMetadata(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_UpdateTrackedObjectMetadata(AsyncCallback callback, object state, List<BlueprintReality.MixCast.Data.TrackedObject> trackedObjects);
      void End_UpdateTrackedObjectMetadata(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_UpdateCameraMetadata(AsyncCallback callback, object state, List<BlueprintReality.MixCast.Data.VirtualCamera> cameras);
      void End_UpdateCameraMetadata(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_UpdateViewfinderMetadata(AsyncCallback callback, object state, List<BlueprintReality.MixCast.Data.Viewfinder> viewfinders);
      void End_UpdateViewfinderMetadata(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_UpdateDesktopMetadata(AsyncCallback callback, object state, BlueprintReality.MixCast.Data.Desktop desktop);
      void End_UpdateDesktopMetadata(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_ResetWorldOrientation(AsyncCallback callback, object state);
      void End_ResetWorldOrientation(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_ModifyWorldOrientation(AsyncCallback callback, object state, double degrees);
      void End_ModifyWorldOrientation(IAsyncResult asyncResult);
      #endif
      #if SILVERLIGHT
      IAsyncResult Begin_SendExperienceCommand(AsyncCallback callback, object state, string eventId);
      void End_SendExperienceCommand(IAsyncResult asyncResult);
      #endif
    }

    public abstract class Handler : HandlerProxy, IFaceServer {
      public void validate_ping(){ }
      public List<string> validate_function_list(){ return new List<string>(){ "validate_ping", "validate_function_list", "SetActivationState", "NotifyServiceStarted", "UpdateLicenseMetadata", "UpdateTrackedObjectMetadata", "UpdateCameraMetadata", "UpdateViewfinderMetadata", "UpdateDesktopMetadata", "ResetWorldOrientation", "ModifyWorldOrientation", "SendExperienceCommand", }; }
      public abstract void SetActivationState(bool active);
      public abstract void NotifyServiceStarted();
      public abstract void UpdateLicenseMetadata(BlueprintReality.MixCast.Data.License license);
      public abstract void UpdateTrackedObjectMetadata(List<BlueprintReality.MixCast.Data.TrackedObject> trackedObjects);
      public abstract void UpdateCameraMetadata(List<BlueprintReality.MixCast.Data.VirtualCamera> cameras);
      public abstract void UpdateViewfinderMetadata(List<BlueprintReality.MixCast.Data.Viewfinder> viewfinders);
      public abstract void UpdateDesktopMetadata(BlueprintReality.MixCast.Data.Desktop desktop);
      public abstract void ResetWorldOrientation();
      public abstract void ModifyWorldOrientation(double degrees);
      public abstract void SendExperienceCommand(string eventId);
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
      public IAsyncResult Begin_SetActivationState(AsyncCallback callback, object state, bool active)
      {
        return send_SetActivationState(callback, state, active);
      }

      public void End_SetActivationState(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
      }

      #endif

      public bool TrySetActivationState(bool active){ try { SetActivationState(active); return true; } catch { return false; } }

      protected void SetActivationState(bool active)
      {
        #if !SILVERLIGHT
        send_SetActivationState(active);

        #else
        var asyncResult = Begin_SetActivationState(null, null, active);

        #endif
      }
      #if SILVERLIGHT
      protected IAsyncResult send_SetActivationState(AsyncCallback callback, object state, bool active)
      #else
      protected void send_SetActivationState(bool active)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("SetActivationState", TMessageType.Oneway, seqid_));
        SetActivationState_args args = new SetActivationState_args();
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
      public IAsyncResult Begin_NotifyServiceStarted(AsyncCallback callback, object state)
      {
        return send_NotifyServiceStarted(callback, state);
      }

      public void End_NotifyServiceStarted(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
      }

      #endif

      public bool TryNotifyServiceStarted(){ try { NotifyServiceStarted(); return true; } catch { return false; } }

      protected void NotifyServiceStarted()
      {
        #if !SILVERLIGHT
        send_NotifyServiceStarted();

        #else
        var asyncResult = Begin_NotifyServiceStarted(null, null);

        #endif
      }
      #if SILVERLIGHT
      protected IAsyncResult send_NotifyServiceStarted(AsyncCallback callback, object state)
      #else
      protected void send_NotifyServiceStarted()
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("NotifyServiceStarted", TMessageType.Oneway, seqid_));
        NotifyServiceStarted_args args = new NotifyServiceStarted_args();
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_UpdateLicenseMetadata(AsyncCallback callback, object state, BlueprintReality.MixCast.Data.License license)
      {
        return send_UpdateLicenseMetadata(callback, state, license);
      }

      public void End_UpdateLicenseMetadata(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
      }

      #endif

      public bool TryUpdateLicenseMetadata(BlueprintReality.MixCast.Data.License license){ try { UpdateLicenseMetadata(license); return true; } catch { return false; } }

      protected void UpdateLicenseMetadata(BlueprintReality.MixCast.Data.License license)
      {
        #if !SILVERLIGHT
        send_UpdateLicenseMetadata(license);

        #else
        var asyncResult = Begin_UpdateLicenseMetadata(null, null, license);

        #endif
      }
      #if SILVERLIGHT
      protected IAsyncResult send_UpdateLicenseMetadata(AsyncCallback callback, object state, BlueprintReality.MixCast.Data.License license)
      #else
      protected void send_UpdateLicenseMetadata(BlueprintReality.MixCast.Data.License license)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("UpdateLicenseMetadata", TMessageType.Oneway, seqid_));
        UpdateLicenseMetadata_args args = new UpdateLicenseMetadata_args();
        args.License = license;
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_UpdateTrackedObjectMetadata(AsyncCallback callback, object state, List<BlueprintReality.MixCast.Data.TrackedObject> trackedObjects)
      {
        return send_UpdateTrackedObjectMetadata(callback, state, trackedObjects);
      }

      public void End_UpdateTrackedObjectMetadata(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
      }

      #endif

      public bool TryUpdateTrackedObjectMetadata(List<BlueprintReality.MixCast.Data.TrackedObject> trackedObjects){ try { UpdateTrackedObjectMetadata(trackedObjects); return true; } catch { return false; } }

      protected void UpdateTrackedObjectMetadata(List<BlueprintReality.MixCast.Data.TrackedObject> trackedObjects)
      {
        #if !SILVERLIGHT
        send_UpdateTrackedObjectMetadata(trackedObjects);

        #else
        var asyncResult = Begin_UpdateTrackedObjectMetadata(null, null, trackedObjects);

        #endif
      }
      #if SILVERLIGHT
      protected IAsyncResult send_UpdateTrackedObjectMetadata(AsyncCallback callback, object state, List<BlueprintReality.MixCast.Data.TrackedObject> trackedObjects)
      #else
      protected void send_UpdateTrackedObjectMetadata(List<BlueprintReality.MixCast.Data.TrackedObject> trackedObjects)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("UpdateTrackedObjectMetadata", TMessageType.Oneway, seqid_));
        UpdateTrackedObjectMetadata_args args = new UpdateTrackedObjectMetadata_args();
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
      public IAsyncResult Begin_UpdateCameraMetadata(AsyncCallback callback, object state, List<BlueprintReality.MixCast.Data.VirtualCamera> cameras)
      {
        return send_UpdateCameraMetadata(callback, state, cameras);
      }

      public void End_UpdateCameraMetadata(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
      }

      #endif

      public bool TryUpdateCameraMetadata(List<BlueprintReality.MixCast.Data.VirtualCamera> cameras){ try { UpdateCameraMetadata(cameras); return true; } catch { return false; } }

      protected void UpdateCameraMetadata(List<BlueprintReality.MixCast.Data.VirtualCamera> cameras)
      {
        #if !SILVERLIGHT
        send_UpdateCameraMetadata(cameras);

        #else
        var asyncResult = Begin_UpdateCameraMetadata(null, null, cameras);

        #endif
      }
      #if SILVERLIGHT
      protected IAsyncResult send_UpdateCameraMetadata(AsyncCallback callback, object state, List<BlueprintReality.MixCast.Data.VirtualCamera> cameras)
      #else
      protected void send_UpdateCameraMetadata(List<BlueprintReality.MixCast.Data.VirtualCamera> cameras)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("UpdateCameraMetadata", TMessageType.Oneway, seqid_));
        UpdateCameraMetadata_args args = new UpdateCameraMetadata_args();
        args.Cameras = cameras;
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_UpdateViewfinderMetadata(AsyncCallback callback, object state, List<BlueprintReality.MixCast.Data.Viewfinder> viewfinders)
      {
        return send_UpdateViewfinderMetadata(callback, state, viewfinders);
      }

      public void End_UpdateViewfinderMetadata(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
      }

      #endif

      public bool TryUpdateViewfinderMetadata(List<BlueprintReality.MixCast.Data.Viewfinder> viewfinders){ try { UpdateViewfinderMetadata(viewfinders); return true; } catch { return false; } }

      protected void UpdateViewfinderMetadata(List<BlueprintReality.MixCast.Data.Viewfinder> viewfinders)
      {
        #if !SILVERLIGHT
        send_UpdateViewfinderMetadata(viewfinders);

        #else
        var asyncResult = Begin_UpdateViewfinderMetadata(null, null, viewfinders);

        #endif
      }
      #if SILVERLIGHT
      protected IAsyncResult send_UpdateViewfinderMetadata(AsyncCallback callback, object state, List<BlueprintReality.MixCast.Data.Viewfinder> viewfinders)
      #else
      protected void send_UpdateViewfinderMetadata(List<BlueprintReality.MixCast.Data.Viewfinder> viewfinders)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("UpdateViewfinderMetadata", TMessageType.Oneway, seqid_));
        UpdateViewfinderMetadata_args args = new UpdateViewfinderMetadata_args();
        args.Viewfinders = viewfinders;
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_UpdateDesktopMetadata(AsyncCallback callback, object state, BlueprintReality.MixCast.Data.Desktop desktop)
      {
        return send_UpdateDesktopMetadata(callback, state, desktop);
      }

      public void End_UpdateDesktopMetadata(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
      }

      #endif

      public bool TryUpdateDesktopMetadata(BlueprintReality.MixCast.Data.Desktop desktop){ try { UpdateDesktopMetadata(desktop); return true; } catch { return false; } }

      protected void UpdateDesktopMetadata(BlueprintReality.MixCast.Data.Desktop desktop)
      {
        #if !SILVERLIGHT
        send_UpdateDesktopMetadata(desktop);

        #else
        var asyncResult = Begin_UpdateDesktopMetadata(null, null, desktop);

        #endif
      }
      #if SILVERLIGHT
      protected IAsyncResult send_UpdateDesktopMetadata(AsyncCallback callback, object state, BlueprintReality.MixCast.Data.Desktop desktop)
      #else
      protected void send_UpdateDesktopMetadata(BlueprintReality.MixCast.Data.Desktop desktop)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("UpdateDesktopMetadata", TMessageType.Oneway, seqid_));
        UpdateDesktopMetadata_args args = new UpdateDesktopMetadata_args();
        args.Desktop = desktop;
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_ResetWorldOrientation(AsyncCallback callback, object state)
      {
        return send_ResetWorldOrientation(callback, state);
      }

      public void End_ResetWorldOrientation(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
      }

      #endif

      public bool TryResetWorldOrientation(){ try { ResetWorldOrientation(); return true; } catch { return false; } }

      protected void ResetWorldOrientation()
      {
        #if !SILVERLIGHT
        send_ResetWorldOrientation();

        #else
        var asyncResult = Begin_ResetWorldOrientation(null, null);

        #endif
      }
      #if SILVERLIGHT
      protected IAsyncResult send_ResetWorldOrientation(AsyncCallback callback, object state)
      #else
      protected void send_ResetWorldOrientation()
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("ResetWorldOrientation", TMessageType.Oneway, seqid_));
        ResetWorldOrientation_args args = new ResetWorldOrientation_args();
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_ModifyWorldOrientation(AsyncCallback callback, object state, double degrees)
      {
        return send_ModifyWorldOrientation(callback, state, degrees);
      }

      public void End_ModifyWorldOrientation(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
      }

      #endif

      public bool TryModifyWorldOrientation(double degrees){ try { ModifyWorldOrientation(degrees); return true; } catch { return false; } }

      protected void ModifyWorldOrientation(double degrees)
      {
        #if !SILVERLIGHT
        send_ModifyWorldOrientation(degrees);

        #else
        var asyncResult = Begin_ModifyWorldOrientation(null, null, degrees);

        #endif
      }
      #if SILVERLIGHT
      protected IAsyncResult send_ModifyWorldOrientation(AsyncCallback callback, object state, double degrees)
      #else
      protected void send_ModifyWorldOrientation(double degrees)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("ModifyWorldOrientation", TMessageType.Oneway, seqid_));
        ModifyWorldOrientation_args args = new ModifyWorldOrientation_args();
        args.Degrees = degrees;
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_SendExperienceCommand(AsyncCallback callback, object state, string eventId)
      {
        return send_SendExperienceCommand(callback, state, eventId);
      }

      public void End_SendExperienceCommand(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
      }

      #endif

      public bool TrySendExperienceCommand(string eventId){ try { SendExperienceCommand(eventId); return true; } catch { return false; } }

      protected void SendExperienceCommand(string eventId)
      {
        #if !SILVERLIGHT
        send_SendExperienceCommand(eventId);

        #else
        var asyncResult = Begin_SendExperienceCommand(null, null, eventId);

        #endif
      }
      #if SILVERLIGHT
      protected IAsyncResult send_SendExperienceCommand(AsyncCallback callback, object state, string eventId)
      #else
      protected void send_SendExperienceCommand(string eventId)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("SendExperienceCommand", TMessageType.Oneway, seqid_));
        SendExperienceCommand_args args = new SendExperienceCommand_args();
        args.EventId = eventId;
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
        processMap_["SetActivationState"] = SetActivationState_Process;
        processMap_["NotifyServiceStarted"] = NotifyServiceStarted_Process;
        processMap_["UpdateLicenseMetadata"] = UpdateLicenseMetadata_Process;
        processMap_["UpdateTrackedObjectMetadata"] = UpdateTrackedObjectMetadata_Process;
        processMap_["UpdateCameraMetadata"] = UpdateCameraMetadata_Process;
        processMap_["UpdateViewfinderMetadata"] = UpdateViewfinderMetadata_Process;
        processMap_["UpdateDesktopMetadata"] = UpdateDesktopMetadata_Process;
        processMap_["ResetWorldOrientation"] = ResetWorldOrientation_Process;
        processMap_["ModifyWorldOrientation"] = ModifyWorldOrientation_Process;
        processMap_["SendExperienceCommand"] = SendExperienceCommand_Process;
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

      public void SetActivationState_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        SetActivationState_args args = new SetActivationState_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        try
        {
          iface_.SetActivationState(args.Active);
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

      public void NotifyServiceStarted_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        NotifyServiceStarted_args args = new NotifyServiceStarted_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        try
        {
          iface_.NotifyServiceStarted();
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

      public void UpdateLicenseMetadata_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        UpdateLicenseMetadata_args args = new UpdateLicenseMetadata_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        try
        {
          iface_.UpdateLicenseMetadata(args.License);
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

      public void UpdateTrackedObjectMetadata_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        UpdateTrackedObjectMetadata_args args = new UpdateTrackedObjectMetadata_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        try
        {
          iface_.UpdateTrackedObjectMetadata(args.TrackedObjects);
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

      public void UpdateCameraMetadata_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        UpdateCameraMetadata_args args = new UpdateCameraMetadata_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        try
        {
          iface_.UpdateCameraMetadata(args.Cameras);
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

      public void UpdateViewfinderMetadata_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        UpdateViewfinderMetadata_args args = new UpdateViewfinderMetadata_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        try
        {
          iface_.UpdateViewfinderMetadata(args.Viewfinders);
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

      public void UpdateDesktopMetadata_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        UpdateDesktopMetadata_args args = new UpdateDesktopMetadata_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        try
        {
          iface_.UpdateDesktopMetadata(args.Desktop);
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

      public void ResetWorldOrientation_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        ResetWorldOrientation_args args = new ResetWorldOrientation_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        try
        {
          iface_.ResetWorldOrientation();
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

      public void ModifyWorldOrientation_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        ModifyWorldOrientation_args args = new ModifyWorldOrientation_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        try
        {
          iface_.ModifyWorldOrientation(args.Degrees);
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

      public void SendExperienceCommand_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        SendExperienceCommand_args args = new SendExperienceCommand_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        try
        {
          iface_.SendExperienceCommand(args.EventId);
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
    public partial class SetActivationState_args : TBase
    {
      private bool _active;

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
        public bool active;
      }

      public SetActivationState_args() {
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
          TStruct struc = new TStruct("SetActivationState_args");
          oprot.WriteStructBegin(struc);
          TField field = new TField();
          if (__isset.active) {
            field.Name = "active";
            field.Type = TType.Bool;
            field.ID = 1;
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
        StringBuilder __sb = new StringBuilder("SetActivationState_args(");
        bool __first = true;
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
    public partial class NotifyServiceStarted_args : TBase
    {

      public NotifyServiceStarted_args() {
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
          TStruct struc = new TStruct("NotifyServiceStarted_args");
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
        StringBuilder __sb = new StringBuilder("NotifyServiceStarted_args(");
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class UpdateLicenseMetadata_args : TBase
    {
      private BlueprintReality.MixCast.Data.License _license;

      public BlueprintReality.MixCast.Data.License License
      {
        get
        {
          return _license;
        }
        set
        {
          __isset.license = true;
          this._license = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool license;
      }

      public UpdateLicenseMetadata_args() {
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
                  License = new BlueprintReality.MixCast.Data.License();
                  License.Read(iprot);
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
          TStruct struc = new TStruct("UpdateLicenseMetadata_args");
          oprot.WriteStructBegin(struc);
          TField field = new TField();
          if (License != null && __isset.license) {
            field.Name = "license";
            field.Type = TType.Struct;
            field.ID = 1;
            oprot.WriteFieldBegin(field);
            License.Write(oprot);
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
        StringBuilder __sb = new StringBuilder("UpdateLicenseMetadata_args(");
        bool __first = true;
        if (License != null && __isset.license) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("License: ");
          __sb.Append(License== null ? "<null>" : License.ToString());
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class UpdateTrackedObjectMetadata_args : TBase
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

      public UpdateTrackedObjectMetadata_args() {
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
                    TList _list4 = iprot.ReadListBegin();
                    for( int _i5 = 0; _i5 < _list4.Count; ++_i5)
                    {
                      BlueprintReality.MixCast.Data.TrackedObject _elem6;
                      _elem6 = new BlueprintReality.MixCast.Data.TrackedObject();
                      _elem6.Read(iprot);
                      TrackedObjects.Add(_elem6);
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
          TStruct struc = new TStruct("UpdateTrackedObjectMetadata_args");
          oprot.WriteStructBegin(struc);
          TField field = new TField();
          if (TrackedObjects != null && __isset.trackedObjects) {
            field.Name = "trackedObjects";
            field.Type = TType.List;
            field.ID = 1;
            oprot.WriteFieldBegin(field);
            {
              oprot.WriteListBegin(new TList(TType.Struct, TrackedObjects.Count));
              foreach (BlueprintReality.MixCast.Data.TrackedObject _iter7 in TrackedObjects)
              {
                _iter7.Write(oprot);
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
        StringBuilder __sb = new StringBuilder("UpdateTrackedObjectMetadata_args(");
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
    public partial class UpdateCameraMetadata_args : TBase
    {
      private List<BlueprintReality.MixCast.Data.VirtualCamera> _cameras;

      public List<BlueprintReality.MixCast.Data.VirtualCamera> Cameras
      {
        get
        {
          return _cameras;
        }
        set
        {
          __isset.cameras = true;
          this._cameras = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool cameras;
      }

      public UpdateCameraMetadata_args() {
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
                    Cameras = new List<BlueprintReality.MixCast.Data.VirtualCamera>();
                    TList _list8 = iprot.ReadListBegin();
                    for( int _i9 = 0; _i9 < _list8.Count; ++_i9)
                    {
                      BlueprintReality.MixCast.Data.VirtualCamera _elem10;
                      _elem10 = new BlueprintReality.MixCast.Data.VirtualCamera();
                      _elem10.Read(iprot);
                      Cameras.Add(_elem10);
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
          TStruct struc = new TStruct("UpdateCameraMetadata_args");
          oprot.WriteStructBegin(struc);
          TField field = new TField();
          if (Cameras != null && __isset.cameras) {
            field.Name = "cameras";
            field.Type = TType.List;
            field.ID = 1;
            oprot.WriteFieldBegin(field);
            {
              oprot.WriteListBegin(new TList(TType.Struct, Cameras.Count));
              foreach (BlueprintReality.MixCast.Data.VirtualCamera _iter11 in Cameras)
              {
                _iter11.Write(oprot);
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
        StringBuilder __sb = new StringBuilder("UpdateCameraMetadata_args(");
        bool __first = true;
        if (Cameras != null && __isset.cameras) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("Cameras: ");
          __sb.Append(Cameras);
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class UpdateViewfinderMetadata_args : TBase
    {
      private List<BlueprintReality.MixCast.Data.Viewfinder> _viewfinders;

      public List<BlueprintReality.MixCast.Data.Viewfinder> Viewfinders
      {
        get
        {
          return _viewfinders;
        }
        set
        {
          __isset.viewfinders = true;
          this._viewfinders = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool viewfinders;
      }

      public UpdateViewfinderMetadata_args() {
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
                    Viewfinders = new List<BlueprintReality.MixCast.Data.Viewfinder>();
                    TList _list12 = iprot.ReadListBegin();
                    for( int _i13 = 0; _i13 < _list12.Count; ++_i13)
                    {
                      BlueprintReality.MixCast.Data.Viewfinder _elem14;
                      _elem14 = new BlueprintReality.MixCast.Data.Viewfinder();
                      _elem14.Read(iprot);
                      Viewfinders.Add(_elem14);
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
          TStruct struc = new TStruct("UpdateViewfinderMetadata_args");
          oprot.WriteStructBegin(struc);
          TField field = new TField();
          if (Viewfinders != null && __isset.viewfinders) {
            field.Name = "viewfinders";
            field.Type = TType.List;
            field.ID = 1;
            oprot.WriteFieldBegin(field);
            {
              oprot.WriteListBegin(new TList(TType.Struct, Viewfinders.Count));
              foreach (BlueprintReality.MixCast.Data.Viewfinder _iter15 in Viewfinders)
              {
                _iter15.Write(oprot);
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
        StringBuilder __sb = new StringBuilder("UpdateViewfinderMetadata_args(");
        bool __first = true;
        if (Viewfinders != null && __isset.viewfinders) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("Viewfinders: ");
          __sb.Append(Viewfinders);
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class UpdateDesktopMetadata_args : TBase
    {
      private BlueprintReality.MixCast.Data.Desktop _desktop;

      public BlueprintReality.MixCast.Data.Desktop Desktop
      {
        get
        {
          return _desktop;
        }
        set
        {
          __isset.desktop = true;
          this._desktop = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool desktop;
      }

      public UpdateDesktopMetadata_args() {
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
                  Desktop = new BlueprintReality.MixCast.Data.Desktop();
                  Desktop.Read(iprot);
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
          TStruct struc = new TStruct("UpdateDesktopMetadata_args");
          oprot.WriteStructBegin(struc);
          TField field = new TField();
          if (Desktop != null && __isset.desktop) {
            field.Name = "desktop";
            field.Type = TType.Struct;
            field.ID = 1;
            oprot.WriteFieldBegin(field);
            Desktop.Write(oprot);
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
        StringBuilder __sb = new StringBuilder("UpdateDesktopMetadata_args(");
        bool __first = true;
        if (Desktop != null && __isset.desktop) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("Desktop: ");
          __sb.Append(Desktop== null ? "<null>" : Desktop.ToString());
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class ResetWorldOrientation_args : TBase
    {

      public ResetWorldOrientation_args() {
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
          TStruct struc = new TStruct("ResetWorldOrientation_args");
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
        StringBuilder __sb = new StringBuilder("ResetWorldOrientation_args(");
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class ModifyWorldOrientation_args : TBase
    {
      private double _degrees;

      public double Degrees
      {
        get
        {
          return _degrees;
        }
        set
        {
          __isset.degrees = true;
          this._degrees = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool degrees;
      }

      public ModifyWorldOrientation_args() {
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
                if (field.Type == TType.Double) {
                  Degrees = iprot.ReadDouble();
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
          TStruct struc = new TStruct("ModifyWorldOrientation_args");
          oprot.WriteStructBegin(struc);
          TField field = new TField();
          if (__isset.degrees) {
            field.Name = "degrees";
            field.Type = TType.Double;
            field.ID = 1;
            oprot.WriteFieldBegin(field);
            oprot.WriteDouble(Degrees);
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
        StringBuilder __sb = new StringBuilder("ModifyWorldOrientation_args(");
        bool __first = true;
        if (__isset.degrees) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("Degrees: ");
          __sb.Append(Degrees);
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class SendExperienceCommand_args : TBase
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

      public SendExperienceCommand_args() {
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
          TStruct struc = new TStruct("SendExperienceCommand_args");
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
        StringBuilder __sb = new StringBuilder("SendExperienceCommand_args(");
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

  }
}
#endif
