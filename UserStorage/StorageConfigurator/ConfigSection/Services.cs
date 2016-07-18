using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageConfigurator.ConfigSection
{
    public class Services : ConfigurationElement
    {
        [ConfigurationProperty("masterCount", DefaultValue = 0, IsKey = false, IsRequired = true)]
        public int MasterCount
        {
            get { return ((int)(base["masterCount"])); }
            set { base["masterCount"] = value; }
        }

        [ConfigurationProperty("slaveCount", DefaultValue = 0, IsKey = false, IsRequired = true)]
        public int SlaveCount
        {
            get { return ((int)(base["slaveCount"])); }
            set { base["slaveCount"] = value; }
        }

    }
}
