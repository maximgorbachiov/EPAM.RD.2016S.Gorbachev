using System;
using System.Collections.Generic;
using StorageInterfaces.CommunicationEntities.WcfEntities;
using WcfWebService.Interfaces.IWebContracts;

namespace WcfWebService.Services.WebServices
{
    public class WebService : IWebServiceContract
    {
        public string TestConnection()
        {
            throw new NotImplementedException();
        }

        public int AddUser(UserDataContract user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(UserDataContract user)
        {
            throw new NotImplementedException();
        }

        public List<UserDataContract> SearchBy(IComparer<UserDataContract> comparer, UserDataContract searchingUser)
        {
            throw new NotImplementedException();
        }
    }
}