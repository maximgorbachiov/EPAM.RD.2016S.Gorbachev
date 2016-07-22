using FibonachyGenerator.Generators;
using FibonachyGenerator.Interfaces;
using StorageConfigurator.ConfigSection;
using StorageLib.Interfaces;
using StorageLib.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using Storage.Storages;
using StorageLib.Services;

namespace StorageConfigurator
{
    public class Configurator : IConfigurator
    {
        private const string FILENAME = "fileName";

        private IStorage logService;
        private readonly List<SlaveStorage> storages = new List<SlaveStorage>();

        public IStorage LogService => logService;
        public List<SlaveStorage> SlaveStorages => storages; 

        public void Load()
        {
            var servicesSection = (ServicesSection)ConfigurationManager.GetSection("Services");
            var servicesInfoSection = (ServicesInfoSection)ConfigurationManager.GetSection("ServicesInfo");

            if (servicesSection == null)
            {
                throw new NullReferenceException("Unable to read services section from config.");
            }

            if (servicesInfoSection == null)
            {
                throw new NullReferenceException("Unable to read service info section from config.");
            }

            IRepository repository = new XMLRepository(ConfigurationManager.AppSettings[FILENAME]);
            IGenerator generator = new IdGenerator();
            IValidator validator = new UserValidator();

            IMasterStorage masterStorage = new MasterStorage(validator, generator, repository);
            masterStorage.Load();

            foreach (var slaveServiceElement in servicesSection.SlaveServices)
            {
                IStorage slaveStorage = new SlaveStorage(masterStorage);
                storages.Add((SlaveStorage)slaveStorage);
            }

            logService = new LogService(masterStorage, "boolSwitch", "trace");
        }

        public void Save()
        {
            ((IMasterStorage)logService).Save();
        }
    }
}
