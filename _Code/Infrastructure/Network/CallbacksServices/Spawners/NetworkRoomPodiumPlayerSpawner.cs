using System.Collections.Generic;
using System.Linq;
using Core.MainMenu.Podium;
using Data.Network;
using Fusion;
using UnityEngine;
using Zenject;

namespace Infrastructure.Network.CallbacksServices.Spawners
{
    public class NetworkRoomPodiumPlayerSpawner : MonoBehaviour
    {
        private Dictionary<PlayerRef, NetworkObject> _playersDict = new();
        private GameObject _networkPodiumPlayerPrb;
        private IPlayersPodium _playersPodium;

        [Inject]
        private void Construct(IPlayersPodium playersPodium, NetworkRoomSettings networkRoomSettings)
        {
            _networkPodiumPlayerPrb = networkRoomSettings.PodiumPlayerPrb;
            _playersPodium = playersPodium;
        }

        public void SpawnPlayer(NetworkRunner runner, PlayerRef playerRef)
        {
            Pedestal pedestal = _playersPodium.GetPedestalByNumber(runner.ActivePlayers.Count());
            NetworkObject player = runner.Spawn(_networkPodiumPlayerPrb, pedestal.PlayerSpawnPos, pedestal.PlayerSpawnRotation, playerRef);
            _playersPodium.ReservePedestalForPlayer(pedestal.Number, player);
            _playersDict.Add(playerRef, player);
        }

        public void DespawnPlayer(NetworkRunner runner, PlayerRef playerRef)
        {
            NetworkObject player = _playersDict[playerRef];
            _playersPodium.FreePedestalFromPlayer(playerRef);
            _playersDict.Remove(playerRef);
            runner.Despawn(player);
        }
    }
}