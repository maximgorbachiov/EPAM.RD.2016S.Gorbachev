using StorageInterfaces.CommunicationEntities.WcfEntities;

namespace StorageInterfaces.IValidators
{
    public interface IValidator
    {
        bool Validate(User user);
    }
}
