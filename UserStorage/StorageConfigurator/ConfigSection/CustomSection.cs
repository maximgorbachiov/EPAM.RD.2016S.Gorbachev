using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageConfigurator.ConfigSection
{
    public class CustomSection : ConfigurationSection
    {
        [ConfigurationProperty("ServiceInfo")]
        public Services ServiceInfo => (Services)base["ServiceInfo"];
    }
}
