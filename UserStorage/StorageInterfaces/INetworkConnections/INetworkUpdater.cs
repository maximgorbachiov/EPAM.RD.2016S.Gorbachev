using System;
using StorageInterfaces.EventArgs;

namespace StorageInterfaces.INetworkConnections
{
    public interface INetworkUpdater
    {
        EventHandler<AddEventArg> OnAdd { get; set; }

        EventHandler<DeleteEventArg> OnDelete { get; set; }

        void UpdateByCommand();
    }
}
