using System;
using StorageInterfaces.IServices;
using System.ServiceModel;
using WcfLibrary.WcfServices;
using System.ServiceModel.Description;
using StorageInterfaces.IWcfServices;
using StorageLib.Services;
using WcfLibrary.Interfaces;
using StorageInterfaces.INetworkConnections;

namespace WcfLibrary
{
    public class WcfHost : MarshalByRefObject, IWcfHost
    {
        private ServiceHost host;

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
            (host.SingletonInstance as IWcfLoader).Load();
            (host.SingletonInstance as INetworkUpdater)?.UpdateByCommand();
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
