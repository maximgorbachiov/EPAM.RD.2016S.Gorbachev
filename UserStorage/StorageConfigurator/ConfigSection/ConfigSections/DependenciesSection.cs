using System.Configuration;
using StorageConfigurator.ConfigSection.ConfigElements;

namespace StorageConfigurator.ConfigSection.ConfigSections
{
    public class DependenciesSection : ConfigurationSection
    {
        [ConfigurationProperty("Generator")]
        public TypeInfoElement Generator => (TypeInfoElement)base["Generator"];

        [ConfigurationProperty("Repository")]
        public TypeInfoElement Repository => (TypeInfoElement)base["Repository"];

        [ConfigurationProperty("Validator")]
        public TypeInfoElement Validator => (TypeInfoElement)base["Validator"];

        [ConfigurationProperty("NetworkNotificator")]
        public TypeInfoElement NetworkNotificator => (TypeInfoElement)base["NetworkNotificator"];

        [ConfigurationProperty("NetworkUpdater")]
        public TypeInfoElement NetworkUpdater => (TypeInfoElement)base["NetworkUpdater"];
    }
}
