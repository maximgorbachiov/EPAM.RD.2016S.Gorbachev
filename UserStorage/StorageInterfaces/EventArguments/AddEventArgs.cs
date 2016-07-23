using StorageInterfaces.Entities;
using System;

namespace StorageInterfaces.EventArguments
{
    public class AddEventArgs : EventArgs
    {
        public User User { get; set; }
    }
}
