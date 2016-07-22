using System.IO;
using System.Xml.Serialization;
using StorageLib.Entities;
using StorageLib.Interfaces;

namespace StorageLib.Repositories
{
    public class XMLRepository : IRepository
    {
        private readonly string fileName;
        private readonly XmlSerializer formatter = new XmlSerializer(typeof(ServiceState));

        public XMLRepository(string fileName)
        {
            this.fileName = fileName;
        }

        public ServiceState Load()
        {
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
            using (var xmlFile = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(xmlFile, state);
            }
        }
    }
}
