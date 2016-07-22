using System.Configuration;

namespace StorageConfigurator.ConfigSection
{
    public class SlaveServiceElement : ConfigurationElement
    {
        [ConfigurationProperty("port", DefaultValue = 0, IsKey = false, IsRequired = true)]
        public int Port
        {
            get { return ((int)(base["port"])); }
            set { base["port"] = value; }
        }
    }
}
