using System.Configuration;

namespace StorageConfigurator.ConfigSection
{
    [ConfigurationCollection(typeof(SlaveServiceElement), AddItemName = "SlaveService")]
    public class SlaveServicesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SlaveServiceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SlaveServiceElement)(element)).Port;
        }

        public SlaveServiceElement this[int idx] => (SlaveServiceElement)BaseGet(idx);

        [ConfigurationProperty("type", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string SlaveServiceType
        {
            get { return ((string)(base["type"])); }
            set { base["type"] = value; }
        }
    }
}
