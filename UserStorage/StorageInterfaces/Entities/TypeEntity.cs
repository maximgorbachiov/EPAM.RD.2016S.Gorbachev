using System;

namespace StorageInterfaces.Entities
{
    [Serializable]
    public class TypeEntity
    {
        private readonly string type;
        private readonly object[] parameters;

        public TypeEntity(string type, params object[] parameters)
        {
            this.type = type;
            this.parameters = parameters ?? new object[] { };
        }

        public string Type => type;

        public object[] Parameters => parameters;
    }
}
