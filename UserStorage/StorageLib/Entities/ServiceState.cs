using System;
using System.Collections.Generic;

namespace StorageLib.Entities
{
    [Serializable]
    public class ServiceState
    {
        public List<User> Users { get; set; }
        public int LastId { get; set; }
    }
}
