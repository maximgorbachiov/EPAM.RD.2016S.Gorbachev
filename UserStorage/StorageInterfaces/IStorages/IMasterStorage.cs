using StorageInterfaces.EventArguments;
using System;

namespace StorageInterfaces.IStorages
{
    public interface IMasterStorage : IStorage
    {
        event EventHandler<AddEventArgs> OnAddUser;
        event EventHandler<DeleteEventArgs> OnDeleteUser;

        void Load();
        void Save();
    }
}
