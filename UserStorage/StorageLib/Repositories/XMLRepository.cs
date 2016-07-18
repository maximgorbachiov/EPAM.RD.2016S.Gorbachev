using System.IO;
using System.Xml.Serialization;
using StorageLib.Entities;
using StorageLib.Interfaces;

namespace StorageLib.Repositories
{
    public class XMLRepository : IRepository
    {
        private string fileName;

        public XMLRepository(string fileName)
        {
            this.fileName = fileName;
        }

        public ServiceState Load()
        {
            var formatter = new XmlSerializer(typeof(ServiceState));

            using (FileStream xmlFile = new FileStream(fileName, FileMode.OpenOrCreate))
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
            var formatter = new XmlSerializer(typeof(ServiceState));

            using (FileStream xmlFile = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(xmlFile, state);
            }
        }
    }
}
