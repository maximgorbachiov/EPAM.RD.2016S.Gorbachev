using System;
using ClientApplication.WcfServices;
using StorageInterfaces.CommunicationEntities.WcfEntities;

namespace ClientApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IServiceContract service = new ServiceContractClient();
            int id = 0;

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    Console.WriteLine($"Add user №{i}");
                    Console.ReadLine();
                    id = service.AddUser(new User {Name = $"Maxim {i}"});
                    service.DeleteUser(5);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            var result = service.SearchBy(new User { Id = 3, Name = "Maxim 1" });

            for (int i = 0; i < result?.Length; i++)
            {
                Console.WriteLine(result[i]);
            }

            Console.ReadLine();
        }
    }
}
