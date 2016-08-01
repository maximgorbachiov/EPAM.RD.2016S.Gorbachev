using System.Configuration;
using StorageConfigurator.ConfigSection.ConfigCollections;

namespace StorageConfigurator.ConfigSection.ConfigSections
{
    public class SlavesIpSection : ConfigurationSection
    {
        [ConfigurationProperty("SlavesEndPoints")]
        public EndPointCollection EndPoints => (EndPointCollection)base["SlavesEndPoints"];
    }
}
