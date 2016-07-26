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

            for (int i = 0; i < 20; i++)
            {
                try
                {
                    configurator.MasterStorage.AddUser(new User());
                }
                catch (ObjectDisposedException oDEx)
                {
                    LogService.Service.TraceInfo(oDEx.Message);
                    LogService.Service.TraceInfo(oDEx.InnerException.Message);
                    LogService.Service.TraceInfo(oDEx.ObjectName);
                    LogService.Service.TraceInfo(oDEx.StackTrace);
                    throw;
                }
                catch (NullReferenceException nREx)
                {
                    LogService.Service.TraceInfo(nREx.Message);
                    LogService.Service.TraceInfo(nREx.StackTrace);
                    throw;
                }
            }

            Console.ReadKey();
            configurator.Save();
            Console.ReadKey();
        }
    }
}
