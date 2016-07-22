using System.Configuration;

namespace StorageConfigurator.ConfigSection
{
    public class MasterServiceElement : ConfigurationElement
    {
        [ConfigurationProperty("type", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string MasterServiceType
        {
            get { return ((string)(base["type"])); }
            set { base["type"] = value; }
        }

        [ConfigurationProperty("port", DefaultValue = 0, IsKey = true, IsRequired = true)]
        public int Port
        {
            get { return ((int)(base["port"])); }
            set { base["port"] = value; }
        }
    }
}
