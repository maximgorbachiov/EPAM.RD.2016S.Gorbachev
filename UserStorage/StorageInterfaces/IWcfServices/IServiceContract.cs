using System.Collections.Generic;
using System.ServiceModel;
using StorageInterfaces.CommunicationEntities.WcfEntities;

namespace StorageInterfaces.IWcfServices
{
    [ServiceContract]
    public interface IServiceContract
    {
        [OperationContract]
        int AddUser(User userData);

        [OperationContract]
        void DeleteUser(int id);

        [OperationContract]
        List<int> SearchBy(User searchingUser);
    }
}
