using System.Collections.Generic;
using System.ServiceModel;
using StorageInterfaces.CommunicationEntities.WcfEntities;
using StorageInterfaces.Entities;


namespace StorageInterfaces.IServices
{
    [ServiceContract]
    public interface IServiceContract
    {
        [OperationContract]
        int AddUser(UserDataContract userData);

        [OperationContract]
        void DeleteUser(int id);

        [OperationContract]
        List<int> SearchBy(IComparer<User> comparer, UserDataContract searchingUser);
    }
}
