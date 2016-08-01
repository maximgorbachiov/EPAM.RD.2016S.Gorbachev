using System.Configuration;

namespace StorageConfigurator.ConfigSection.ConfigElements
{
    public class EndPointElement : ConfigurationElement
    {
        [ConfigurationProperty("port", DefaultValue = 0, IsKey = false, IsRequired = true)]
        public int Port
        {
            get { return (int)base["port"]; }
            set { base["port"] = value; }
        }

        [ConfigurationProperty("ipaddress", DefaultValue = "127.0.0.1", IsKey = false, IsRequired = false)]
        public string IpAddress
        {
            get { return (string)base["ipaddress"]; }
            set { base["ipaddress"] = value; }
        }
    }
}
