using System.Configuration;
using StorageConfigurator.ConfigSection.ConfigElements;

namespace StorageConfigurator.ConfigSection.ConfigCollections
{
    [ConfigurationCollection(typeof(EndPointElement), AddItemName = "EndPoint")]
    public class EndPointCollection : ConfigurationElementCollection
    {
        public EndPointElement this[int idx] => (EndPointElement)BaseGet(idx);

        protected override ConfigurationElement CreateNewElement()
        {
            return new EndPointElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EndPointElement)element).Port;
        }
    }
}
