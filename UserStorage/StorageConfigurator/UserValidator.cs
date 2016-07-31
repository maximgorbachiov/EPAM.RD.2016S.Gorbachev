using System;
using StorageInterfaces.Entities;
using StorageInterfaces.IValidators;

namespace StorageConfigurator
{
    [Serializable]
    internal class UserValidator : IValidator
    {
        public bool Validate(User user)
        {
            return user.Gender == Gender.Man || user.Gender == Gender.Woman;
        }
    }
}