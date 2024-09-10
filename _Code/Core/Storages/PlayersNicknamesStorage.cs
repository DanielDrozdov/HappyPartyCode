using System.Collections.Generic;
using Fusion;
using Infrastructure.Network;

public class PlayersNicknamesStorage : IPlayersNicknamesStorageSetter, IPlayersNicknamesStorage
{
    private Dictionary<PlayerRef, string> _playersNicknames = new ();

    public PlayersNicknamesStorage(INetworkConnectorCallbacksObserver networkConnectorCallbacksObserver)
    {
        networkConnectorCallbacksObserver.OnSetNewRoomConnection += ClearPlayersNicknames;
    }

    public void SetPlayerNickname(PlayerRef playerRef, string nickname)
    {
        _playersNicknames.TryAdd(playerRef, nickname);
    }

    public string GetPlayerNickname(PlayerRef playerRef)
    {
        return _playersNicknames.GetValueOrDefault(playerRef);
    }

    private void ClearPlayersNicknames(NetworkRunner runner)
    {
        _playersNicknames.Clear();
    }
}
