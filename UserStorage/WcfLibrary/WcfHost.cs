using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using StorageInterfaces.IServices;
using StorageInterfaces.IWcfServices;
using StorageLib.Services;
using WcfLibrary.Interfaces;
using WcfLibrary.WcfServices;

namespace WcfLibrary
{
    public class WcfHost : MarshalByRefObject, IWcfHost
    {
        private readonly ServiceHost host;

        public WcfHost(IService service, string address)
        {
            var uri = new Uri(address);
            var wcfService = new WcfService(service);

            host = new ServiceHost(wcfService, uri);

            var smb = new ServiceMetadataBehavior
            {
                HttpGetEnabled = true,
                MetadataExporter = { PolicyVersion = PolicyVersion.Policy15 }
            };

            host.Description.Behaviors.Add(smb);
        }

        public void OpenWcfService()
        {
            ((IWcfLoader)host.SingletonInstance).Load();
            ((IWcfListener)host.SingletonInstance)?.UpdateByCommand();
            host.Open();
            LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } wcf service was opened");
        }

        public void CloseWcfService()
        {
            host.Close();
            (host.SingletonInstance as IWcfLoader)?.Save();
            ((IDisposable)host).Dispose();
        }
    }
}
