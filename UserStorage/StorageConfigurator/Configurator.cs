using StorageConfigurator.ConfigSection;
using System;
using System.Collections.Generic;
using System.Configuration;
using StorageInterfaces.IStorages;
using StorageInterfaces.IRepositories;
using StorageInterfaces.IGenerators;
using StorageInterfaces.IValidators;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using Storage.Services;
using StorageInterfaces.CommunicationEntities;
using StorageInterfaces.IServices;
using StorageLib.Services;
using StorageLib.Storages;

namespace StorageConfigurator
{
    public class Configurator : IConfigurator
    {
        private const string FILENAME = "fileName";

        private IMasterStorage masterStorage;
        private readonly List<SlaveStorage> storages = new List<SlaveStorage>();
        private AppDomain masterDomain;
        private readonly List<AppDomain> slaveDomains = new List<AppDomain>();

        public IMasterStorage MasterStorage => masterStorage;
        public List<SlaveStorage> SlaveStorages => storages; 

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
                var slaveData = new SlaveConnectionData { MasterEndPoint = masterEndPoint, SlaveEndPoint = slaveEndPoint };
                ISlaveStorage slaveStorage = CreateSlaveStorage(slave.ServiceType, infoSection, slaveData);
                slaveStorage.UpdateByMasterCommand();
                storages.Add((SlaveStorage)slaveStorage);
            }

            var data = new MasterConnectionData { MasterEndPoint = masterEndPoint, SlavesEndPoints = slavesEndPoints };
            CreateMasterStorage(master?.ServiceType, master?.HostAddress, infoSection, data);
            masterStorage.Load();
        }

        public void Save()
        {
            masterStorage.Save();
        }

        private void CreateMasterStorage(string masterType, string serviceAddress, ServicesInfoSection info, MasterConnectionData data)
        {
            masterDomain = AppDomain.CreateDomain("Master");

            var repository = DependencyCreater.CreateDependency<IRepository>(info.Repository.Type, FILENAME);
            var generator = DependencyCreater.CreateDependency<IGenerator>(info.Generator.Type);
            var validator = DependencyCreater.CreateDependency<IValidator>(info.Validator.Type);

            var type = Type.GetType(masterType);

            if (type == null)
            {
                throw new ConfigurationErrorsException($"Invalid type of { masterDomain.FriendlyName } domain");
            }

            var master = (IMasterStorage)masterDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName, true, BindingFlags.CreateInstance, null, 
                new object[] { validator, generator, repository, data }, CultureInfo.InvariantCulture, null);

            var address = new Uri(serviceAddress);

            using (var host = (ServiceHost)masterDomain.CreateInstanceAndUnwrap(typeof(ServiceHost).Assembly.FullName, typeof(ServiceHost).FullName, true, BindingFlags.CreateInstance, null,
                new object[] { typeof(WcfService), address }, CultureInfo.InvariantCulture, null))
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

        private ISlaveStorage CreateSlaveStorage(string slaveType, ServicesInfoSection info, SlaveConnectionData data)
        {
            var repository = DependencyCreater.CreateDependency<IRepository>(info.Repository.Type, FILENAME);
            var slaveDomain = AppDomain.CreateDomain($"Slave №{ slaveDomains.Count }");
            slaveDomains.Add(slaveDomain);

            var type = Type.GetType(slaveType);

            if (type == null)
            {
                throw new ConfigurationErrorsException($"Invalid type of { slaveDomain.FriendlyName } domain");
            }

            return (ISlaveStorage)slaveDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName, true, BindingFlags.CreateInstance, null,
                new object[] { repository, data }, CultureInfo.InvariantCulture, null);
        }
    }
}
