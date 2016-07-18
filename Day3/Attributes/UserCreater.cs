using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Attributes
{
    public class UserCreater
    {
        public List<User> CreateUsualUsers()
        {
            var users = new List<User>();
            var attributes = typeof(User).GetCustomAttributes(typeof(InstantiateUserAttribute))
                .Select(attribute => (InstantiateUserAttribute)attribute);

            foreach (var attribute in attributes)
            {
                if (attribute.Id == null)
                {
                    attribute.Id = MatchParametr(typeof(User), "id");
                }

                var constructorInfo = typeof(User).GetConstructor(new[] { typeof(int) });
                if (constructorInfo != null)
                {
                    var user = (User)constructorInfo.Invoke(new object[] { attribute.Id.Value });
                    user.Name = attribute.FirstName;
                    user.Surname = attribute.LastName;
                    users.Add(user);
                }
            }

            return users;
        }

        public List<AdvancedUser> CreateAdvancedUsers()
        {
            var users = new List<AdvancedUser>();

            var assembly = Assembly.GetExecutingAssembly();
            var attributes = assembly.GetCustomAttributes(typeof(InstantiateAdvancedUserAttribute))
                .Select(attribute => (InstantiateAdvancedUserAttribute)attribute);

            foreach (var attribute in attributes)
            {
                if (attribute.Id == null)
                {
                    attribute.Id = MatchParametr(typeof(AdvancedUser), "id");
                }
                if (attribute.ExternalId == null)
                {
                    attribute.ExternalId = MatchParametr(typeof(AdvancedUser), "externalId");
                }

                var constructorInfo = typeof(AdvancedUser).GetConstructor(new[] { typeof(int), typeof(int) });
                if (constructorInfo != null)
                {
                    var user = (AdvancedUser)constructorInfo.Invoke(new object[] { attribute.Id.Value, attribute.ExternalId.Value });
                    user.Name = attribute.FirstName;
                    user.Surname = attribute.LastName;
                    users.Add(user);
                }
            }

            return users;
        }

        private int MatchParametr(Type type, string paramName)
        {
            var constructors = type.GetConstructors();
            var constructor = constructors.FirstOrDefault(c => c.GetCustomAttributes(typeof(MatchParameterWithPropertyAttribute)) != null);
            var attribute = constructor?.GetCustomAttributes(typeof(MatchParameterWithPropertyAttribute)).Select(ca => (MatchParameterWithPropertyAttribute)ca)
                    .FirstOrDefault(attr => attr.Parametr == paramName);

            if (attribute == null)
            {
                throw new InvalidOperationException();
            }

            var defaultValueAttr = (DefaultValueAttribute)(type.GetProperties().FirstOrDefault(prop => prop.Name == attribute.Property)
                .GetCustomAttribute(typeof(DefaultValueAttribute)));
            return (int)defaultValueAttr.Value;
        }
    }
}
