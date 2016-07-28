using System;
using System.Collections.Generic;
using StorageInterfaces.CommunicationEntities.WcfEntities;
using WcfWebService.Interfaces.IWebContracts;

namespace WcfWebService
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы Service1.svc или Service1.svc.cs в обозревателе решений и начните отладку.
    public class WebService : IWebServiceContract
    {
        public string TestConnection()
        {
            return "OK";
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
