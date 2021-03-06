﻿using System.Configuration;
using StorageConfigurator.ConfigSection.ConfigElements;

namespace StorageConfigurator.ConfigSection.ConfigCollections
{
    [ConfigurationCollection(typeof(ServiceElement), AddItemName = "Service")]
    public class ServicesCollection : ConfigurationElementCollection
    {
        public ServiceElement this[int idx] => (ServiceElement)BaseGet(idx);

        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceElement)element).Port;
        }
    }
}
