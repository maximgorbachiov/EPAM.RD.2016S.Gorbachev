using System.Configuration;

namespace StorageConfigurator.ConfigSection
{
    public class ServicesInfoSection : ConfigurationSection
    {
        [ConfigurationProperty("Generator")]
        public TypeInfoElement Generator => (TypeInfoElement)base["Generator"];

        [ConfigurationProperty("Repository")]
        public TypeInfoElement Repository => (TypeInfoElement)base["Repository"];

        [ConfigurationProperty("Validator")]
        public TypeInfoElement Validator => (TypeInfoElement)base["Validator"];
    }
}
