using StorageInterfaces.CommunicationEntities.WcfEntities;
using StorageInterfaces.Entities;

namespace StorageInterfaces.Mappers
{
    public static class VisasMapper
    {
        public static SavedCountryVisa ToSavedVisa(this CountryVisa visa)
        {
            return new SavedCountryVisa
            {
                Country = visa.Country ?? "",
                Start = visa.Start,
                End = visa.End
            };
        }

        public static CountryVisa ToVisa(this SavedCountryVisa visa)
        {
            return new CountryVisa
            {
                Country = visa.Country ?? "",
                Start = visa.Start,
                End = visa.End
            };
        }
    }
}
