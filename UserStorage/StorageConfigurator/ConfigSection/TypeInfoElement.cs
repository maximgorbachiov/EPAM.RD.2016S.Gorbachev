using System.Configuration;

namespace StorageConfigurator.ConfigSection
{
    public class TypeInfoElement : ConfigurationElement
    {
        [ConfigurationProperty("type", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Type
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }
    }
}
