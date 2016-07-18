using StorageLib.Entities;
using StorageLib.Interfaces;

namespace StorageConfigurator
{
    internal class UserValidator : IValidator
    {
        public bool Validate(User user)
        {
            return (user.Gender == Gender.Man || user.Gender == Gender.Woman);
        }
    }
}