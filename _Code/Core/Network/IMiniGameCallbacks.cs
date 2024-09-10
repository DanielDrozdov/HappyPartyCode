using System;

namespace Core.Network
{
    public interface IMiniGameCallbacks
    {
        event Action OnMiniGameStarted;
        event Action OnMiniGameEnded;
    }
}