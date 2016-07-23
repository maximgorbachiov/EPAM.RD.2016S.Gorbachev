using StorageInterfaces.Entities;
using StorageInterfaces.IValidators;
using System;

namespace StorageConfigurator
{
    [Serializable]
    internal class UserValidator : IValidator
    {
        public bool Validate(User user)
        {
            return (user.Gender == Gender.Man || user.Gender == Gender.Woman);
        }
    }
}