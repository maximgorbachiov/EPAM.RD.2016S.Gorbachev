using System.Linq;
using StorageInterfaces.CommunicationEntities.WcfEntities;
using StorageInterfaces.Entities;

namespace StorageInterfaces.Mappers
{
    public static class UserMapper
    {
        public static SavedUser ToSavedUser(this User user)
        {
            return new SavedUser
            {
                Id = user.Id,
                Name = user.Name ?? "",
                SecondName = user.SecondName ?? "",
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Visas = user.Visas?.Select(visa => visa.ToSavedVisa()).ToArray() ?? new SavedCountryVisa[] { }
            };
        }

        public static User ToUser(this SavedUser user)
        {
            return new User
            {
                Id = user.Id,
                Name = user.Name ?? "",
                SecondName = user.SecondName ?? "",
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Visas = user.Visas?.Select(visa => visa.ToVisa()).ToArray() ?? new CountryVisa[] { }
            };
        }
    }
}
