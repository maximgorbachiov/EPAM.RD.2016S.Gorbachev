using System;
using System.Collections.Generic;
using StorageInterfaces.Entities;
using WcfService.IWebServices;

namespace WcfService.WebServices
{
    public class WebService : IWebService
    {
        public WebService()
        {
            
        }

        public int AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public List<User> SearchBy(IComparer<User> comparer, User searchingUser)
        {
            throw new NotImplementedException();
        }
    }
}
