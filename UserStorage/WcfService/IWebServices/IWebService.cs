using System.Collections.Generic;
using System.ServiceModel;
using StorageInterfaces.Entities;

namespace WcfService.IWebServices
{ 
    [ServiceContract]
    public interface IWebService
    {
        [OperationContract]
        int AddUser(User user);

        [OperationContract]
        void DeleteUser(int id);

        [OperationContract]
        List<User> SearchBy(IComparer<User> comparer, User searchingUser);
    }
}
