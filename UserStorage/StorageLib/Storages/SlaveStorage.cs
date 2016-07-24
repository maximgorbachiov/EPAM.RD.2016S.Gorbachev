using System;
using System.Collections.Generic;
using System.Linq;
using StorageInterfaces.IStorages;
using StorageInterfaces.Entities;
using StorageInterfaces.EventArguments;
using System.Net;
using StorageInterfaces.ISerializers;
using StorageInterfaces.CommunicationEntities;
using StorageLib.Serializers;
using System.Net.Sockets;
using Storage;

namespace StorageLib.Storages
{
    public class SlaveStorage : MarshalByRefObject, ISlaveStorage
    {
        private IPEndPoint slaveEndPoint;
        private IPEndPoint masterEndPoint;
        private List<User> users = new List<User>();

        public List<User> Users => users;

        public SlaveStorage(IPEndPoint slaveEndPoint, IPEndPoint masterEndPoint)
        {
            this.slaveEndPoint = slaveEndPoint;
            this.masterEndPoint = masterEndPoint;
        }

        public int AddUser(User user)
        {
            throw new NotSupportedException("This method can't be called from slave. Try to use master for it");
        }

        public void DeleteUser(int id)
        {
            throw new NotSupportedException("This method can't be called from slave. Try to use master for it");
        }

        public List<int> SearchBy(IComparer<User> comparator, User searchingUser)
        {
            if (comparator == null)
            {
                throw new NullReferenceException(nameof(comparator));
            }

            if (searchingUser == null)
            {
                throw new NullReferenceException(nameof(searchingUser));
            }

            return Users.Where(user => comparator.Compare(user, searchingUser) == 0)
                .Select(user => user.Id).ToList();
        }

        async public void NotifyMasterAboutSlaveCreate()
        {
            using (var slave = new TcpClient())
            {
                ISerializer<ServiceCommands> commandSerializer = new BsonSerializer<ServiceCommands>();
                ISerializer<UsersCollection> collectionSerializer = new BsonSerializer<UsersCollection>();
                await slave.ConnectAsync(masterEndPoint.Address, masterEndPoint.Port);
                NetworkStream stream = slave.GetStream();

                byte[] data = commandSerializer.Serialize(ServiceCommands.IS_CREATED);
                await stream.WriteAsync(data, 0, data.Length);

                using (var client = new Client(slave))
                {
                    data = await client.ProcessAsync();
                }
                users = collectionSerializer.Deserialize(data).Users;
            }
        }

        async public void UpdateByMasterCommand()
        {
            var listener = new TcpListener(slaveEndPoint);
            listener.Start();

            while(true)
            {
                var client = await listener.AcceptTcpClientAsync();
                var tcpStream = client.GetStream();
            }
        }

        protected virtual void AddEventHandler(object sender, AddEventArgs e)
        {
            if (e.User != null)
            {
                Users.Add(e.User);
            }
        }

        protected virtual void DeleteEventHandler(object sender, DeleteEventArgs e)
        {
            User user;
            if ((user = Users.FirstOrDefault(u => u.Id == e.Id)) != null)
            {
                Users.Remove(user);
            }
        }
    }
}
