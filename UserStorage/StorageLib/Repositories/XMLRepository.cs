using StorageInterfaces.Entities;
using StorageInterfaces.IRepositories;
using System;
using System.IO;
using System.Xml.Serialization;

namespace StorageLib.Repositories
{
    [Serializable]
    public class XMLRepository : IRepository
    {
        private readonly string fileName;

        public XMLRepository(string fileName)
        {
            this.fileName = fileName;
        }

        public ServiceState Load()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(ServiceState));
            using (var xmlFile = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                if (xmlFile.Length > 0)
                {
                    return (ServiceState)formatter.Deserialize(xmlFile);
                }
            }
            return new ServiceState();
        }

        public void Save(ServiceState state)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(ServiceState));
            using (var xmlFile = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(xmlFile, state);
            }
        }
    }
}
