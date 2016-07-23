using Storage.Interfaces.Entities.UserInfo;
using Storage.Interfaces.Interfaces;

namespace ValidatorProject
{
    public class Validator : IValidator
    {
        public bool IsValid(User user)
        {
            return (user.Gender == Gender.Female);
        }
    }
}
