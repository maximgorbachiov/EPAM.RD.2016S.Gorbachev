using System.Configuration;

namespace StorageConfigurator.ConfigSection
{
    public class ServiceElement : ConfigurationElement
    {
        [ConfigurationProperty("port", DefaultValue = 0, IsKey = false, IsRequired = true)]
        public int Port
        {
            get { return ((int)(base["port"])); }
            set { base["port"] = value; }
        }

        [ConfigurationProperty("ipaddress", DefaultValue = "127.0.0.1", IsKey = false, IsRequired = false)]
        public string IpAddress
        {
            get { return ((string)(base["ipaddress"])); }
            set { base["ipaddress"] = value; }
        }

        [ConfigurationProperty("isMaster", DefaultValue = false, IsKey = false, IsRequired = true)]
        public bool IsMaster
        {
            get { return ((bool)(base["isMaster"])); }
            set { base["isMaster"] = value; }
        }

        [ConfigurationProperty("hostaddress", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string HostAddress
        {
            get { return ((string)(base["hostaddress"])); }
            set { base["hostaddress"] = value; }
        }

        [ConfigurationProperty("type", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string ServiceType
        {
            get { return ((string)(base["type"])); }
            set { base["type"] = value; }
        }
    }
}
