using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageLib.Entities
{
    public struct CountryVisa
    {
        public string country;
        public DateTime start;
        public DateTime end;

        public CountryVisa(string country, DateTime start, DateTime end)
        {
            this.country = country;
            this.start = start;
            this.end = end;
        }
    }
}
