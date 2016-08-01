using System.Configuration;
using StorageConfigurator.ConfigSection.ConfigCollections;

namespace StorageConfigurator.ConfigSection.ConfigSections
{
    public class ServicesSection : ConfigurationSection
    {
        [ConfigurationProperty("Services")]
        public ServicesCollection Services => (ServicesCollection)base["Services"];
    }
}
