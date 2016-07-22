using System;

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
