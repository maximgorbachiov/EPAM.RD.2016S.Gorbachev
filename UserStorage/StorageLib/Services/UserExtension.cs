using System.Collections.Generic;
using System.Linq;
using StorageInterfaces.CommunicationEntities.WcfEntities;

namespace StorageLib.Services
{
    public static class UserExtension
    {
        public static List<User> SearchUsersById(this User searchingUser, List<User> users)
        {
            return users.Where(user => user.Id == searchingUser.Id).ToList();
        }

        public static List<User> SearchUsersByName(this User searchingUser, List<User> users)
        {
            return users.Where(user => user.Name == searchingUser.Name).ToList();
        }

        public static List<User> SearchUsersBySurname(this User searchingUser, List<User> users)
        {
            return users.Where(user => user.SecondName == searchingUser.SecondName).ToList();
        }

        public static List<User> SearchUsersByDateOfBirth(this User searchingUser, List<User> users)
        {
            return users.Where(user => user.DateOfBirth == searchingUser.DateOfBirth).ToList();
        }

        public static List<User> SearchUsersByGender(this User searchingUser, List<User> users)
        {
            return users.Where(user => user.Gender == searchingUser.Gender).ToList();
        }
    }
}
