using System;
using Fusion;

namespace Infrastructure.Network
{
    public interface INetworkConnectorCallbacksObserver
    {
        INetworkCallbacksEvents NetworkCallbacksEvents { get; }
        event Action<NetworkRunner> OnSetNewRoomConnection;
    }
}