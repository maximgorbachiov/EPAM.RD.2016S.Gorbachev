using StorageConfigurator.ConfigSection;
using System;
using System.Collections.Generic;
using System.Configuration;
using StorageLib.Services;
using StorageInterfaces.IStorages;
using StorageInterfaces.IRepositories;
using StorageInterfaces.IGenerators;
using StorageInterfaces.IValidators;
using System.Globalization;
using System.Net;
using System.Reflection;
using StorageLib.Storages;

namespace StorageConfigurator
{
    public class Configurator : IConfigurator
    {
        private const string FILENAME = "fileName";

        private IStorage logService;
        private readonly List<SlaveStorage> storages = new List<SlaveStorage>();
        private AppDomain masterDomain;
        private readonly List<AppDomain> slaveDomains = new List<AppDomain>();

        public IStorage LogService => logService;
        public List<SlaveStorage> SlaveStorages => storages; 

        public void Load()
        {
            var servicesSection = (ServicesSection)ConfigurationManager.GetSection("Services");
            var infoSection = (ServicesInfoSection)ConfigurationManager.GetSection("ServicesInfo");

            if (servicesSection == null)
            {
                throw new NullReferenceException("Unable to read services section from config.");
            }

            if (infoSection == null)
            {
                throw new NullReferenceException("Unable to read service info section from config.");
            }

            IMasterStorage masterStorage = CreateMasterStorage(servicesSection.MasterService, infoSection);
            masterStorage.Load();
            //masterStorage.InitializeServices();
            ((MasterStorage)masterStorage).MasterEndPoint = new IPEndPoint(servicesSection.MasterService.IPAddress, servicesSection.MasterService.Port);

            for (int i = 0; i < servicesSection.SlaveServices.Count; i++)
            {
                ISlaveStorage slaveStorage = CreateSlaveStorage(servicesSection.SlaveServices[i], infoSection, 
                    ((MasterStorage)masterStorage).MasterEndPoint, new IPEndPoint(servicesSection.SlaveServices[i].IPAddress, servicesSection.SlaveServices[i].Port),
                    servicesSection.SlaveServices.SlaveServiceType);
                //slaveStorage.NotifyMasterAboutSlaveCreate();
                slaveStorage.UpdateByMasterCommand();
                storages.Add((SlaveStorage)slaveStorage);
            }

            logService = new LogService(masterStorage, "boolSwitch", "trace");
        }

        public void Save()
        {
            ((IMasterStorage)logService).Save();
        }

        private IMasterStorage CreateMasterStorage(MasterServiceElement masterInfo, ServicesInfoSection info)
        {
            masterDomain = AppDomain.CreateDomain("Master");

            var repository = DependencyCreater.CreateDependency<IRepository>(info.Repository.Type, "IRepository", FILENAME);
            var generator = DependencyCreater.CreateDependency<IGenerator>(info.Generator.Type, "IGenerator");
            var validator = DependencyCreater.CreateDependency<IValidator>(info.Validator.Type, "IValidator");

            string[] strings = GetAssemblyAndType(masterInfo.MasterServiceType);
            string assembly = strings[1], type = strings[0]; 
            
            return (IMasterStorage)masterDomain.CreateInstanceAndUnwrap(assembly, type, true, BindingFlags.CreateInstance, null, 
                new object[] { validator, generator, repository }, CultureInfo.InvariantCulture, null);
        }

        private ISlaveStorage CreateSlaveStorage(SlaveServiceElement slaveInfo, ServicesInfoSection info, IPEndPoint masterEndPoint, IPEndPoint slaveEndPoint, string assemblyAndType)
        {
            var repository = DependencyCreater.CreateDependency<IRepository>(info.Repository.Type, "IRepository", FILENAME);
            var slaveDomain = AppDomain.CreateDomain("Slave " + "№" + slaveDomains.Count);
            slaveDomains.Add(slaveDomain);

            string[] strings = GetAssemblyAndType(assemblyAndType);
            string assembly = strings[1], type = strings[0];

            return (ISlaveStorage)masterDomain.CreateInstanceAndUnwrap(assembly, type, true, BindingFlags.CreateInstance, null,
                new object[] { repository, masterEndPoint, slaveEndPoint }, CultureInfo.InvariantCulture, null);
        }

        private string[] GetAssemblyAndType(string assemblyAndType)
        {
            if (assemblyAndType == null)
            {
                throw new NullReferenceException(nameof(assemblyAndType));
            }

            string[] strings = assemblyAndType.Split(' ');
            string[] result = new string[2];
            int j = 0;

            for(int i = 0; i < strings.Length; i++)
            {
                if (strings[i] != " ")
                {
                    result[j] = strings[i];
                    j++;
                }
            }

            result[0] = result[0].Remove(result[0].Length - 1, 1);

            return result;
        }
    }
}
