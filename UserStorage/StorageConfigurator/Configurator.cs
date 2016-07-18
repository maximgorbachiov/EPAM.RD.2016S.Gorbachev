using FibonachyGenerator.Generators;
using FibonachyGenerator.Interfaces;
using Storage;
using StorageConfigurator.ConfigSection;
using StorageLib.Entities;
using StorageLib.Interfaces;
using StorageLib.Repositories;
using StorageLib.Strategies;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageConfigurator
{
    public class Configurator : IConfigurator
    {
        private IGeneratorId generator;
        private IRepository repository;
        private IStorage logService;
        private List<StorageLib.Storage> storages = new List<StorageLib.Storage>();

        public const string FILENAME = "fileName";

        public IStorage Load()
        {
            CustomSection servicesSection = (CustomSection)ConfigurationManager.GetSection("Services");

            if (servicesSection == null)
            {
                throw new NullReferenceException("Unable to read section from config.");
            }

            if (servicesSection.ServiceInfo.MasterCount != 1)
            {
                throw new ConfigurationErrorsException("Count of masters must be one.");
            }

            int[] idArray = new int[3];
            string fileName = ConfigurationManager.AppSettings[FILENAME];

            repository = new XMLRepository(fileName);
            generator = new IdGenerator();

            ServiceState state = repository.Load();

            while (generator.Current < state.LastId)
            {
                generator.MoveNext();
            }

            IValidator validator = new UserValidator();
            IStrategy masterStrategy = new MasterStrategy(state.Users, validator, generator);
            IStorage masterStorage = new StorageLib.Storage(masterStrategy);

            for (int i = 0; i < servicesSection.ServiceInfo.SlaveCount; i++)
            {
                IStrategy slaveStrategy = new SlaveStrategy(repository.Load().Users, (MasterStrategy)masterStrategy);
                IStorage slaveStorage = new StorageLib.Storage(slaveStrategy);
                storages.Add((StorageLib.Storage)slaveStorage);
            }

            logService = new LogService(masterStorage);

            return logService;
        }

        public void Save()
        {
            ServiceState state = new ServiceState() { Users = logService.Users, LastId = generator.Current };

            repository.Save(state);
        }
    }
}
