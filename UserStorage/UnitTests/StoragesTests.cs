using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageConfigurator;
using StorageInterfaces.CommunicationEntities;
using StorageInterfaces.CommunicationEntities.WcfEntities;
using StorageInterfaces.Entities;
using StorageInterfaces.IFactories;
using StorageInterfaces.IGenerators;
using StorageInterfaces.INetworkConnections;
using StorageInterfaces.IRepositories;
using StorageInterfaces.IServices;
using StorageInterfaces.IValidators;
using StorageLib.Services;
using StorageLib.Storages;

namespace UnitTests
{
    [TestClass]
    public class StoragesTests
    {
        [TestMethod]
        public void AddUser_Master_Test()
        {
            var master = CreateMaster(new List<IPEndPoint>());
            int id = master.AddUser(new User());

            Assert.AreEqual(1, id);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void AddUser_Slave_Test()
        {
            var slaveEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10001);

            var slave = CreateSlave(slaveEndPoint);
            int id = slave.AddUser(new User());
        }

        [TestMethod]
        public void DeleteUser_Master_Test()
        {
            var master = CreateMaster(new List<IPEndPoint>());
            int id = master.AddUser(new User());
            master.DeleteUser(id);

            User user = new User { Id = id };

            var result = master.SearchBy(new Func<List<User>, List<User>>[] { user.SearchUsersById });

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void DeleteUser_Slave_Test()
        {
            var slaveEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10001);

            var master = CreateMaster(new List<IPEndPoint>());
            var slave = CreateSlave(slaveEndPoint);

            int id = master.AddUser(new User());
            slave.DeleteUser(id);
        }

        [TestMethod]
        public void SearchUserById_Master_Test()
        {
            var master = CreateMaster(new List<IPEndPoint>());
            int id = master.AddUser(new User());
            User user = new User { Id = id };

            var result = master.SearchBy(new Func<List<User>, List<User>>[] { user.SearchUsersById });

            Assert.AreEqual(1, id);
        }

        [TestMethod]
        public void SearchUserById_Slave_Test()
        {
            User user = new User();
            int[] result = new int[5];
            var slavesEndPoints = new List<IPEndPoint>();
            var slaveEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10001);

            slavesEndPoints.Add(slaveEndPoint);

            var slave = CreateSlave(slaveEndPoint);
            var master = CreateMaster(slavesEndPoints);
            
            for (int i = 0; i < 5; i++)
            {
                user = new User { Id = master.AddUser(new User()) };
                result[i] = master.SearchBy(new Func<List<User>, List<User>>[] { user.SearchUsersById })[0];
                Assert.AreEqual(user.Id, result[i]);
            }
        }

        [TestMethod]
        public void SearchUserByName_Master_Test()
        {
            var master = CreateMaster(new List<IPEndPoint>());
            int id1 = master.AddUser(new User { Name = "Maxim" });
            int id2 = master.AddUser(new User { Name = "Maxim" });
            User searchingUser = new User { Name = "Maxim" };

            var result = master.SearchBy(new Func<List<User>, List<User>>[] { searchingUser.SearchUsersByName });

            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void SearchUserByName_Slave_Test()
        {
            User user = new User { Name = "Maxim" };
            List<int> result = new List<int>();
            var slavesEndPoints = new List<IPEndPoint>();
            var slaveEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10001);

            slavesEndPoints.Add(slaveEndPoint);

            var slave = CreateSlave(slaveEndPoint);
            var master = CreateMaster(slavesEndPoints);

            for (int i = 0; i < 5; i++)
            {
                master.AddUser(new User { Name = "Maxim" });
            }

            result = master.SearchBy(new Func<List<User>, List<User>>[] { user.SearchUsersByName });
            Assert.AreEqual(5, result.Count);
        }

        private IService CreateMaster(List<IPEndPoint> ipEndPoints)
        {
            var masterParams = new Dictionary<Type, TypeEntity>
                {
                    {
                        typeof(IRepository),
                        new TypeEntity("StorageLib.Repositories.XMLRepository, Storage", "storage.txt")
                    },
                    {
                        typeof(IGenerator),
                        new TypeEntity("FibonachyGenerator.Generators.IdGenerator, FibonachyGenerator")
                    },
                    {
                        typeof(IValidator),
                        new TypeEntity("StorageConfigurator.UserValidator, StorageConfigurator")
                    },
                    {
                        typeof(INetworkNotificator),
                        new TypeEntity("StorageLib.NetworkClients.NetworkNotificator, Storage",
                            new ServicesIp { SlavesEndPoints = ipEndPoints })
                    }
                };

            IFactory factory = new DependencyCreater(masterParams);

            return new MasterService(factory);
        }

        private IService CreateSlave(IPEndPoint slaveEndPoint)
        {
            var slaveParams = new Dictionary<Type, TypeEntity>
                {
                    {
                        typeof(IRepository),
                        new TypeEntity("StorageLib.Repositories.XMLRepository, Storage", "storage.txt")
                    },
                    {
                        typeof(INetworkUpdater),
                        new TypeEntity("StorageLib.NetworkClients.NetworkUpdater, Storage", slaveEndPoint)
                    }
                };

            IFactory factory = new DependencyCreater(slaveParams);

            return new SlaveService(factory);
        }
    }
}
