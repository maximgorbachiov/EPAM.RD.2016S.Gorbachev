using StorageInterfaces.Entities;

namespace StorageInterfaces.IValidators
{
    public interface IValidator
    {
        bool Validate(User user);
    }
}
