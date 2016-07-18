using System;
using StorageConfigurator;
using StorageLib.Interfaces;

namespace UserStorageConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurator configurator = new Configurator();
            IStorage service = configurator.Load();
            configurator.Save();
            Console.ReadKey();
        }
    }
}
