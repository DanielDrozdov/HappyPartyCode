using System.Collections.Generic;
using System.Collections.ObjectModel;
using Fusion;
using Infrastructure.Network;

namespace Core.Storages
{
    public class MiniGamePlayersStorage : IMiniGamePlayersStorage
    {
        private List<PlayerRef> _activeLivePlayers;
        private ReadOnlyCollection<PlayerRef> _readOnlyActiveLivePlayers;

        public ReadOnlyCollection<PlayerRef> ActiveLivePlayers => _readOnlyActiveLivePlayers;

        public MiniGamePlayersStorage(INetworkActiveRunnerInfoProvider networkActiveRunnerInfoProvider, INetworkConnectorCallbacksObserver networkConnectorCallbacksObserver)
        {
            _activeLivePlayers = new(networkActiveRunnerInfoProvider.ActiveSessionPlayers);
            _readOnlyActiveLivePlayers = new (_activeLivePlayers);
            networkConnectorCallbacksObserver.NetworkCallbacksEvents.OnNetworkPlayerLeft += OnPlayerLeft;
        }

        public void SetPlayerAsInactive(PlayerRef player)
        {
            _activeLivePlayers.Remove(player);
        }

        private void OnPlayerLeft(PlayerRef player)
        {
            _activeLivePlayers.Remove(player);
        }
    }
}
