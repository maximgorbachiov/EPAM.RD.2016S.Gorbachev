using System.Configuration;
using System.Net;

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

        [ConfigurationProperty("ipaddress", DefaultValue = default(IPAddress), IsKey = false, IsRequired = false)]
        public IPAddress IPAddress
        {
            get { return ((IPAddress)(base["ipaddress"])); }
            set { base["ipaddress"] = value; }
        }
    }
}
