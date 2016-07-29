using StorageConfigurator.ConfigSection;
using System;
using System.Collections.Generic;
using System.Configuration;
using StorageInterfaces.IRepositories;
using StorageInterfaces.IGenerators;
using StorageInterfaces.IValidators;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using StorageInterfaces.CommunicationEntities;
using StorageInterfaces.Entities;
using StorageInterfaces.INetworkConnections;
using StorageInterfaces.IServices;
using StorageInterfaces.IWcfServices;
using StorageLib.Services;
using StorageLib.Storages;

namespace StorageConfigurator
{
    public class Configurator : IConfigurator
    {
        private const string FILENAME = "fileName";

        private IService masterService;
        private readonly List<SlaveService> services = new List<SlaveService>();
        private AppDomain masterDomain;
        private readonly List<AppDomain> slaveDomains = new List<AppDomain>();

        public IService MasterService => masterService;
        public List<SlaveService> SlaveServices => services; 

        public void Load()
        {
            var servicesSection = (ServicesSection)ConfigurationManager.GetSection("ServicesSection");
            var infoSection = (ServicesInfoSection)ConfigurationManager.GetSection("ServicesInfoSection");
            IPEndPoint masterEndPoint = null;
            var slavesEndPoints = new List<IPEndPoint>();
            var slaves = new List<ServiceElement>();
            ServiceElement master = null;

            if (servicesSection == null)
            {
                throw new NullReferenceException("Unable to read services section from config.");
            }
            var servicesInfo = servicesSection.Services;

            if (infoSection == null)
            {
                throw new NullReferenceException("Unable to read service info section from config.");
            }

            for (int i = 0; i < servicesInfo.Count; i++)
            {
                if (servicesInfo[i].IsMaster)
                {
                    masterEndPoint = new IPEndPoint(IPAddress.Parse(servicesInfo[i].IpAddress), servicesInfo[i].Port);
                    master = servicesInfo[i];
                }
                else
                {
                    slaves.Add(servicesInfo[i]);
                }
            }

            foreach (var slave in slaves)
            {
                var slaveEndPoint = new IPEndPoint(IPAddress.Parse(slave.IpAddress), slave.Port);
                slavesEndPoints.Add(slaveEndPoint);

                IService slaveService = CreateSlaveService(slave.ServiceType, infoSection, slaveEndPoint);
                ((IListener)slaveService).UpdateByMasterCommand();
                services.Add((SlaveService)slaveService);
            }

            var data = new MasterConnectionData { MasterEndPoint = masterEndPoint, SlavesEndPoints = slavesEndPoints };
            CreateMasterService(master?.ServiceType, master?.HostAddress, infoSection, data);
            ((ILoader)masterService).Load();
        }

        public void Save()
        {
            ((ILoader)masterService).Save();
        }

        private void CreateMasterService(string masterType, string serviceAddress, ServicesInfoSection info, MasterConnectionData data)
        {
            var masterParams = new Dictionary<Type, TypeEntity>
            {
                { typeof (IRepository), new TypeEntity(info.Repository.Type, FILENAME) },
                { typeof (IGenerator), new TypeEntity(info.Generator.Type) },
                { typeof (IValidator), new TypeEntity(info.Validator.Type) },
                { typeof (INetworkNotificator), new TypeEntity(info.NetworkNotificator.Type, data) }
            };

            var factory = new DependencyCreater(masterParams);

            masterDomain = AppDomain.CreateDomain("Master");

            var type = Type.GetType(masterType);

            if (type == null)
            {
                throw new ConfigurationErrorsException($"Invalid type of { masterDomain.FriendlyName } domain");
            }

            var master = (IService)masterDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName, true, BindingFlags.CreateInstance, null, 
                new object[] { factory }, CultureInfo.InvariantCulture, null);

            var address = new Uri(serviceAddress);
            var wcfService = new WcfService(master);

            using (var host = (ServiceHost)masterDomain.CreateInstanceAndUnwrap(typeof(ServiceHost).Assembly.FullName, typeof(ServiceHost).FullName, true, BindingFlags.CreateInstance, null,
                new object[] { wcfService, address }, CultureInfo.InvariantCulture, null))
            {
                // Enable metadata publishing.
                var smb = new ServiceMetadataBehavior
                {
                    HttpGetEnabled = true,
                    MetadataExporter = {PolicyVersion = PolicyVersion.Policy15}
                };
                host.Description.Behaviors.Add(smb);
                ((IInitializeServiceContract)host.SingletonInstance).Initialize(master);
                host.Open();

                Console.WriteLine("The service is ready at {0}", address);
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();

                // Close the ServiceHost.
                host.Close();
            }
        }

        private IService CreateSlaveService(string slaveType, ServicesInfoSection info, IPEndPoint slaveEndPoint)
        {
            var slavesParams = new Dictionary<Type, TypeEntity>
            {
                { typeof (IRepository), new TypeEntity(info.Repository.Type, FILENAME) },
                { typeof (INetworkUpdater), new TypeEntity(info.NetworkUpdater.Type, slaveEndPoint) }
            };
            var factory = new DependencyCreater(slavesParams);

            var slaveDomain = AppDomain.CreateDomain($"Slave №{ slaveDomains.Count }");
            slaveDomains.Add(slaveDomain);

            var type = Type.GetType(slaveType);

            if (type == null)
            {
                throw new ConfigurationErrorsException($"Invalid type of { slaveDomain.FriendlyName } domain");
            }

            return (IService)slaveDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName, true, BindingFlags.CreateInstance, null,
                new object[] { factory }, CultureInfo.InvariantCulture, null);
        }
    }
}
