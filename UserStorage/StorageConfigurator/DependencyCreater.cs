using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using StorageInterfaces.Entities;
using StorageInterfaces.IFactories;

namespace StorageConfigurator
{
    [Serializable]
    public class DependencyCreater : IFactory
    {
        private readonly Dictionary<Type, TypeEntity> dependencies; 

        public DependencyCreater(Dictionary<Type, TypeEntity> dependencies)
        {
            this.dependencies = dependencies;
        }

        /*public T CreateDependency<T>()
        {
            var type = Type.GetType(typeName);
            if (type?.GetInterface(typeof(T).Name) == null || type.GetConstructor(new Type[] { }) == null)
                throw new ConfigurationErrorsException("Unable to create.");
            return (T)Activator.CreateInstance(type);
        }*/

        public T CreateDependency<T>()
        {
            var dependency = dependencies[typeof(T)];
            var type = Type.GetType(dependency.Type);

            if (type?.GetInterface(typeof(T).Name) == null || type.GetConstructor(dependency.Parameters.Select(t => t.GetType()).ToArray()) == null)
            {
                throw new ConfigurationErrorsException($"Unable to create { typeof(T).FullName }.");
            }

            return (T)Activator.CreateInstance(type, dependency.Parameters);
        }
    }
}
