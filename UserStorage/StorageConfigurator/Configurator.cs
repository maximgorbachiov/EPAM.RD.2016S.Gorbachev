using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Reflection;
using StorageConfigurator.ConfigSection.ConfigCollections;
using StorageConfigurator.ConfigSection.ConfigElements;
using StorageConfigurator.ConfigSection.ConfigSections;
using StorageInterfaces.CommunicationEntities;
using StorageInterfaces.Entities;
using StorageInterfaces.IGenerators;
using StorageInterfaces.INetworkConnections;
using StorageInterfaces.IRepositories;
using StorageInterfaces.IServices;
using StorageInterfaces.IValidators;
using StorageInterfaces.IWcfServices;
using WcfLibrary;

namespace StorageConfigurator
{
    public class Configurator : IConfigurator
    {
        private const string FILENAME = "fileName";

        private readonly List<IWcfHost> slaveHosts = new List<IWcfHost>();
        private IWcfHost masterHost;

        public void Load()
        {
            var servicesSection = (ServicesSection)ConfigurationManager.GetSection("ServicesSection");
            var dependenciesSection = (DependenciesSection)ConfigurationManager.GetSection("DependenciesSection");
            var slaves = new List<ServiceElement>();
            ServiceElement master = null;

            if (servicesSection == null)
            {
                throw new NullReferenceException("Unable to read services section from config.");
            }

            var servicesInfo = servicesSection.Services;

            if (dependenciesSection == null)
            {
                throw new NullReferenceException("Unable to read service dependencies section from config.");
            }

            var servicesIp = GetIpAddresses(servicesInfo, ref master, ref slaves);
            int j = 0;

            if (slaves != null)
            {
                foreach (var slave in slaves)
                {
                    j++;
                    var slaveEndPoint = new IPEndPoint(IPAddress.Parse(slave.IpAddress), slave.Port);

                    var slavesParams = new Dictionary<Type, TypeEntity>
                    {
                        { typeof(IRepository), new TypeEntity(dependenciesSection.Repository.Type, FILENAME) },
                        { typeof(INetworkUpdater), new TypeEntity(dependenciesSection.NetworkUpdater.Type, slaveEndPoint) }
                    };

                    IWcfHost slaveHost = CreateServiceHost($"Slave{j}", slave.ServiceType, slave.HostAddress, slavesParams);
                    slaveHosts.Add(slaveHost);
                    slaveHost.OpenWcfService();
                }
            }

            if (master != null)
            {
                var masterParams = new Dictionary<Type, TypeEntity>
                {
                    { typeof(IRepository), new TypeEntity(dependenciesSection.Repository.Type, FILENAME) },
                    { typeof(IGenerator), new TypeEntity(dependenciesSection.Generator.Type) },
                    { typeof(IValidator), new TypeEntity(dependenciesSection.Validator.Type) },
                    { typeof(INetworkNotificator), new TypeEntity(dependenciesSection.NetworkNotificator.Type, servicesIp) }
                };

                masterHost = CreateServiceHost("Master", master?.ServiceType, master?.HostAddress, masterParams);
                masterHost.OpenWcfService();
            }
        }

        public void Save()
        {
            masterHost?.CloseWcfService();
            
            foreach (var host in slaveHosts)
            {
                host.CloseWcfService();
            }
        }

        private IWcfHost CreateServiceHost(string hostName, string serviceType, string address, Dictionary<Type, TypeEntity> parameters)
        {
            var factory = new DependencyCreater(parameters);

            var domain = AppDomain.CreateDomain(hostName);

            var type = Type.GetType(serviceType);

            if (type == null)
            {
                throw new ConfigurationErrorsException($"Invalid type of { domain.FriendlyName } domain");
            }

            var service = (IService)domain.CreateInstanceAndUnwrap(
                type.Assembly.FullName,
                type.FullName,
                true,
                BindingFlags.CreateInstance,
                null, 
                new object[] { factory },
                CultureInfo.InvariantCulture,
                null);

            var host = (IWcfHost)domain.CreateInstanceAndUnwrap(
                typeof(WcfHost).Assembly.FullName,
                typeof(WcfHost).FullName,
                true,
                BindingFlags.CreateInstance,
                null,
                new object[] { service, address },
                CultureInfo.InvariantCulture,
                null);

            return host;
        }

        private ServicesIp GetIpAddresses(ServicesCollection collection, ref ServiceElement masterElement, ref List<ServiceElement> slavesElements)
        {
            var slaveEndPoints = new List<IPEndPoint>();

            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i].IsMaster)
                {
                    masterElement = collection[i];

                    var ipSection = (SlavesIpSection)ConfigurationManager.GetSection("SlavesIpSection");

                    for (int j = 0; j < ipSection.EndPoints.Count; j++)
                    {
                        slaveEndPoints.Add(new IPEndPoint(IPAddress.Parse(ipSection.EndPoints[j].IpAddress), ipSection.EndPoints[j].Port));
                    }
                }
                else
                {
                    slavesElements.Add(collection[i]);
                }
            }

            return new ServicesIp { SlavesEndPoints = slaveEndPoints };
        }
    }
}
