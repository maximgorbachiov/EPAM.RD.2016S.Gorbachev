using System;
using StorageLib.Entities;

namespace StorageLib.EventArguments
{
    public class AddEventArgs : EventArgs
    {
        public User User { get; set; }
    }
}
