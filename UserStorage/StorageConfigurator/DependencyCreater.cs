using System;
using System.Configuration;
using System.Linq;

namespace StorageConfigurator
{
    public static class DependencyCreater
    {
        public static T CreateDependency<T>(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type?.GetInterface(typeof(T).Name) == null || type.GetConstructor(new Type[] { }) == null)
                throw new ConfigurationErrorsException("Unable to create.");
            return (T)Activator.CreateInstance(type);
        }

        public static T CreateDependency<T>(string typeName, params object[] constructorParams)
        {
            var type = Type.GetType(typeName);

            if (type?.GetInterface(typeof(T).Name) == null || type.GetConstructor(constructorParams.Select(t => t.GetType()).ToArray()) == null)
                throw new ConfigurationErrorsException("Unable to create repository.");

            return (T)Activator.CreateInstance(type, constructorParams);
        }
    }
}
