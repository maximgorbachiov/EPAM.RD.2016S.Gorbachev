using System;
using StorageConfigurator;
using StorageInterfaces.Entities;
using StorageLib.Services;

namespace UserStorageConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Configurator configurator = new Configurator();
            configurator.Load();

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    Console.ReadLine();
                    //configurator.MasterService.AddUser(new User());
                }
                catch (ObjectDisposedException oDEx)
                {
                    LogService.Service.TraceInfo(oDEx.Message);
                }
                catch (NullReferenceException nREx)
                {
                    LogService.Service.TraceInfo(nREx.Message);
                }
            }

            Console.ReadKey();
            //configurator.Save();
            //Console.ReadKey();
        }
    }
}
