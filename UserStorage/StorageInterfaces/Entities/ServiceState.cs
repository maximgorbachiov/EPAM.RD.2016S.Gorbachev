using System;
using System.Collections.Generic;

namespace StorageInterfaces.Entities
{
    [Serializable]
    public class ServiceState
    {
        public List<User> Users { get; set; }
        public int LastId { get; set; }
    }
}
