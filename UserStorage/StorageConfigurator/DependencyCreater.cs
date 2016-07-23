using System;
using System.Collections.Generic;
using System.Configuration;

namespace StorageConfigurator
{
    public static class DependencyCreater
    {
        public static T CreateDependency<T>(string typeName, string dependencyName)
        {
            var type = Type.GetType(typeName);
            if (type?.GetInterface(dependencyName) == null || type.GetConstructor(new Type[] { }) == null)
                throw new ConfigurationErrorsException("Unable to create.");
            return (T)Activator.CreateInstance(type);
        }

        public static T CreateDependency<T>(string typeName, string dependencyName, params object[] constructorParams)
        {
            var constructorParamsTypes = new List<Type>();

            for (int i = 0; i < constructorParams.Length; i++)
            {
                constructorParamsTypes.Add(constructorParams[i].GetType());
            }

            var type = Type.GetType(typeName);
            if (type?.GetInterface(dependencyName) == null || type.GetConstructor(constructorParamsTypes.ToArray()) == null)
                throw new ConfigurationErrorsException("Unable to create repository.");
            return (T)Activator.CreateInstance(type, constructorParams);
        }
    }
}
