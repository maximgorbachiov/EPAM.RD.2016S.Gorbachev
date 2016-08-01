using System;
using StorageInterfaces.EventArgs;

namespace StorageInterfaces.INetworkConnections
{
    public interface INetworkUpdater
    {
        EventHandler<ReceiveMessageEventArg> OnMessageReceived { get; set; }

        void UpdateByCommand();
    }
}
