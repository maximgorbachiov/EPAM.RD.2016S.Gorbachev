using System.Configuration;

namespace StorageConfigurator.ConfigSection
{
    public class ServicesSection : ConfigurationSection
    {
        [ConfigurationProperty("MasterService")]
        public MasterServiceElement MasterService => (MasterServiceElement)base["MasterService"];

        [ConfigurationProperty("SlaveServices")]
        public SlaveServicesCollection SlaveServices => (SlaveServicesCollection)base["SlaveServices"];
    }
}
