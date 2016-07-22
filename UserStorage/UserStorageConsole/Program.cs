using System;
using StorageConfigurator;

namespace UserStorageConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurator configurator = new Configurator();
            configurator.Load();
            configurator.Save();
            Console.ReadKey();
        }
    }
}
