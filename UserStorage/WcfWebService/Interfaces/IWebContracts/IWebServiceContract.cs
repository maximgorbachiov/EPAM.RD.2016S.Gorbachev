using System.Collections.Generic;
using System.ServiceModel;
using StorageInterfaces.CommunicationEntities.WcfEntities;

namespace WcfWebService.Interfaces.IWebContracts
{
    [ServiceContract]
    public interface IWebServiceContract
    {
        [OperationContract]
        string TestConnection();

        [OperationContract]
        int AddUser(UserDataContract user);

        [OperationContract]
        void DeleteUser(UserDataContract user);

        [OperationContract]
        List<UserDataContract> SearchBy(IComparer<UserDataContract> comparer, UserDataContract searchingUser);
    }
}
