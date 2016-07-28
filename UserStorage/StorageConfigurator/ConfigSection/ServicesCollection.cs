using System.Configuration;

namespace StorageConfigurator.ConfigSection
{
    [ConfigurationCollection(typeof(ServiceElement), AddItemName = "Service")]
    public class ServicesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceElement)(element)).Port;
        }

        public ServiceElement this[int idx] => (ServiceElement)BaseGet(idx);
    }
}
