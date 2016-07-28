using StorageInterfaces.CommunicationEntities.WcfEntities;
using StorageInterfaces.Entities;

namespace StorageInterfaces.Mappers
{
    public static class UserMapper
    {
        public static User ToUser(this UserDataContract userDataContract)
        {
            return new User
            {
                Id = userDataContract.Id,
                Name = userDataContract.Name,
                SecondName = userDataContract.SecondName,
                DateOfBirth = userDataContract.DateOfBirth,
                Gender = userDataContract.Gender,
                Visas = userDataContract.Visas
            };
        }

        public static UserDataContract ToUserDataContract(this User user)
        {
            return new UserDataContract
            {
                Id = user.Id,
                Name = user.Name,
                SecondName = user.SecondName,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Visas = user.Visas
            };
        }
    }
}
