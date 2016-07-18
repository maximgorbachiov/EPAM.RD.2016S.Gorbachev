using StorageLib.Entities;

namespace StorageLib.Interfaces
{
    public interface IValidator
    {
        bool Validate(User user);
    }
}
