using System.Configuration;

namespace StorageConfigurator.ConfigSection
{
    public class ServicesSection : ConfigurationSection
    {        
        [ConfigurationProperty("Services")]
        public ServicesCollection Services => (ServicesCollection)base["Services"];
    }
}
