using System;
using StorageConfigurator;
using StorageLib.Services;

namespace UserStorageConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Configurator configurator = new Configurator();
            configurator.Load();

            try
            {
                Console.ReadLine();
                configurator.Save();
            }
            catch (ObjectDisposedException objectDisposedException)
            {
                LogService.Service.TraceInfo(objectDisposedException.Message);
            }
            catch (NullReferenceException nullReferenceException)
            {
                LogService.Service.TraceInfo(nullReferenceException.Message);
            }
            Console.ReadLine();
        }
    }
}
