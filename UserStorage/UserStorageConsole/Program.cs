using System;
using StorageConfigurator;
using StorageInterfaces.Entities;
using StorageLib.Services;

namespace UserStorageConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Configurator configurator = new Configurator();
            configurator.Load();

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    Console.ReadLine();
                    ////configurator.MasterService.AddUser(new User());
                }
                catch (ObjectDisposedException objectDisposedException)
                {
                    LogService.Service.TraceInfo(objectDisposedException.Message);
                }
                catch (NullReferenceException nullReferenceException)
                {
                    LogService.Service.TraceInfo(nullReferenceException.Message);
                }
            }

            Console.ReadKey();
            ////configurator.Save();
            ////Console.ReadKey();
        }
    }
}
