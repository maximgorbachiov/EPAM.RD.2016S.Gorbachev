using System;
using StorageLib.EventArguments;

namespace StorageLib.Interfaces
{
    public interface IMasterStorage : IStorage
    {
        event EventHandler<AddEventArgs> OnAddUser;
        event EventHandler<DeleteEventArgs> OnDeleteUser;

        void Load();
        void Save();
    }
}
