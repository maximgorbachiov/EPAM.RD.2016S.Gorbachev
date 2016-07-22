using System.Configuration;

namespace StorageConfigurator.ConfigSection
{
    public class ServicesInfoSection : ConfigurationSection
    {
        [ConfigurationProperty("Generator")]
        public TypeInfo Generator => (TypeInfo)base["Generator"];

        [ConfigurationProperty("Repository")]
        public TypeInfo Repository => (TypeInfo)base["Repository"];

        [ConfigurationProperty("Validator")]
        public TypeInfo Validator => (TypeInfo)base["Validator"];
    }
}
