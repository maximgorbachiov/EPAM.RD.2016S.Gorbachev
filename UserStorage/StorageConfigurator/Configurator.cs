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
using StorageInterfaces.CommunicationEntities;
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
            var servicesSection = (ServicesSection)ConfigurationManager.GetSection("Services");
            var infoSection = (ServicesInfoSection)ConfigurationManager.GetSection("ServicesInfo");
            var masterInfo = servicesSection.MasterService;
            var slavesInfo = servicesSection.SlaveServices;
            var slavesEndPoints = new List<IPEndPoint>();
            var masterEndPoint = new IPEndPoint(IPAddress.Parse(masterInfo.IPAddress), masterInfo.Port);

            if (servicesSection == null)
            {
                throw new NullReferenceException("Unable to read services section from config.");
            }

            if (infoSection == null)
            {
                throw new NullReferenceException("Unable to read service info section from config.");
            }

            for (int i = 0; i < slavesInfo.Count; i++)
            {
                var slaveEndPoint = new IPEndPoint(IPAddress.Parse(slavesInfo[i].IPAddress), slavesInfo[i].Port);
                var slaveData = new SlaveConnectionData { MasterEndPoint = masterEndPoint, SlaveEndPoint = slaveEndPoint };
                slavesEndPoints.Add(slaveEndPoint);
                ISlaveStorage slaveStorage = CreateSlaveStorage(infoSection, slaveData, slavesInfo.SlaveServiceType);
                slaveStorage.UpdateByMasterCommand();
                storages.Add((SlaveStorage)slaveStorage);
            }

            var data = new MasterConnectionData { MasterEndPoint = masterEndPoint, SlavesEndPoints = slavesEndPoints };
            masterStorage = CreateMasterStorage(masterInfo, infoSection, data);
            masterStorage.Load();
        }

        public void Save()
        {
            masterStorage.Save();
        }

        private IMasterStorage CreateMasterStorage(MasterServiceElement masterInfo, ServicesInfoSection info, MasterConnectionData data)
        {
            masterDomain = AppDomain.CreateDomain("Master");

            var repository = DependencyCreater.CreateDependency<IRepository>(info.Repository.Type, FILENAME);
            var generator = DependencyCreater.CreateDependency<IGenerator>(info.Generator.Type);
            var validator = DependencyCreater.CreateDependency<IValidator>(info.Validator.Type);

            var type = Type.GetType(masterInfo.MasterServiceType);

            if (type == null)
            {
                throw new ConfigurationErrorsException($"Invalid type { type } of { masterDomain.FriendlyName } domain");
            }

            return (IMasterStorage)masterDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName, true, BindingFlags.CreateInstance, null, 
                new object[] { validator, generator, repository, data }, CultureInfo.InvariantCulture, null);
        }

        private ISlaveStorage CreateSlaveStorage(ServicesInfoSection info, SlaveConnectionData data, string assemblyAndType)
        {
            var repository = DependencyCreater.CreateDependency<IRepository>(info.Repository.Type, FILENAME);
            var slaveDomain = AppDomain.CreateDomain($"Slave №{ slaveDomains.Count }");
            slaveDomains.Add(slaveDomain);

            var type = Type.GetType(assemblyAndType);

            if (type == null)
            {
                throw new ConfigurationErrorsException($"Invalid type { type } of { slaveDomain.FriendlyName } domain");
            }

            return (ISlaveStorage)slaveDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName, true, BindingFlags.CreateInstance, null,
                new object[] { repository, data }, CultureInfo.InvariantCulture, null);
        }
    }
}
