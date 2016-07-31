using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Reflection;
using StorageConfigurator.ConfigSection;
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
            var infoSection = (ServicesInfoSection)ConfigurationManager.GetSection("ServicesInfoSection");
            ServicesIp servicesIp;
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

            servicesIp = GetIpAddresses(servicesInfo, ref master, ref slaves);
            int j = 0;

            foreach (var slave in slaves)
            {
                j++;
                var slaveEndPoint = new IPEndPoint(IPAddress.Parse(slave.IpAddress), slave.Port);

                servicesIp.SlavesEndPoints.Add(slaveEndPoint);

                var slavesParams = new Dictionary<Type, TypeEntity>
                {
                    { typeof(IRepository), new TypeEntity(infoSection.Repository.Type, FILENAME) },
                    { typeof(INetworkUpdater), new TypeEntity(infoSection.NetworkUpdater.Type, slaveEndPoint) }
                };

                IWcfHost slaveHost = CreateServiceHost($"Slave{j}", slave.ServiceType, slave.HostAddress, infoSection, slavesParams);
                slaveHosts.Add(slaveHost);
                slaveHost.OpenWcfService();
            }

            var masterParams = new Dictionary<Type, TypeEntity>
            {
                { typeof(IRepository), new TypeEntity(infoSection.Repository.Type, FILENAME) },
                { typeof(IGenerator), new TypeEntity(infoSection.Generator.Type) },
                { typeof(IValidator), new TypeEntity(infoSection.Validator.Type) },
                { typeof(INetworkNotificator), new TypeEntity(infoSection.NetworkNotificator.Type, servicesIp) }
            };

            masterHost = CreateServiceHost("Master", master?.ServiceType, master?.HostAddress, infoSection, masterParams);
            masterHost.OpenWcfService();
        }

        public void Save()
        {
            masterHost.CloseWcfService();
            
            foreach (var host in slaveHosts)
            {
                host.CloseWcfService();
            }
        }

        private IWcfHost CreateServiceHost(string hostName, string serviceType, string address, ServicesInfoSection info, Dictionary<Type, TypeEntity> parameters)
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
            IPEndPoint masterEndPoint = null;
            List<IPEndPoint> slaveEndPoints = new List<IPEndPoint>();

            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i].IsMaster)
                {
                    masterEndPoint = new IPEndPoint(IPAddress.Parse(collection[i].IpAddress), collection[i].Port);
                    masterElement = collection[i];
                }
                else
                {
                    slavesElements.Add(collection[i]);
                }
            }

            return new ServicesIp { MasterEndPoint = masterEndPoint, SlavesEndPoints = slaveEndPoints };
        }

        /*private IWcfHost CreateSlaveHost(string slaveType, string address, ServicesInfoSection info, Dictionary<Type, TypeEntity> parameters)
        {
            var factory = new DependencyCreater(parameters);

            var slaveDomain = AppDomain.CreateDomain($"Slave №{ slaveHosts.Count }");

            var type = Type.GetType(slaveType);

            if (type == null)
            {
                throw new ConfigurationErrorsException($"Invalid type of { slaveDomain.FriendlyName } domain");
            }

            var slave = (IService)slaveDomain.CreateInstanceAndUnwrap(
                type.Assembly.FullName,
                type.FullName,
                true,
                BindingFlags.CreateInstance,
                null,
                new object[] { factory },
                CultureInfo.InvariantCulture,
                null);

            var host = (IWcfHost)slaveDomain.CreateInstanceAndUnwrap(
                typeof(WcfHost).Assembly.FullName,
                typeof(WcfHost).FullName,
                true,
                BindingFlags.CreateInstance,
                null,
                new object[] { slave, address },
                CultureInfo.InvariantCulture,
                null);

            return host;
        }*/
    }
}
