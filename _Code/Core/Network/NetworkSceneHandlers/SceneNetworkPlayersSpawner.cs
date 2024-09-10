using System.Collections.Generic;
using System.Collections.ObjectModel;
using Core.MiniGames;
using Infrastructure.Network;
using UnityEngine;
using Zenject;
using Fusion;
using Odin = Sirenix.OdinInspector;
using Random = UnityEngine.Random;

namespace Core.Network.SceneHandlers
{
    public class SceneNetworkPlayersSpawner : NetworkBehaviour, IPlayerLeft
    {
        [SerializeField, Odin.ReadOnly] 
        private PlayerSpawnPoint[] _playersSpawnPoints;
        
        private GameObject _playerPrb;
        private List<PlayerSpawnPoint> _activePlayersSpawnPoints;
        private List<NetworkObject> _activePlayersObjectsList;

        public ReadOnlyCollection<NetworkObject> ReadOnlyActivePlayersObjectsList { get; private set; }

        [Inject]
        private void Construct(INetworkSceneSettingsProvider sceneSettingsProvider)
        {
            if (sceneSettingsProvider.CurrentSceneSettings != null)
            {
                _playerPrb = sceneSettingsProvider.CurrentSceneSettings.PlayerPrb;
            }
        }

        private void Awake()
        {
            _activePlayersSpawnPoints = new List<PlayerSpawnPoint>(_playersSpawnPoints);
            _activePlayersObjectsList = new List<NetworkObject>();
            ReadOnlyActivePlayersObjectsList = new ReadOnlyCollection<NetworkObject>(_activePlayersObjectsList);
        }

        public void PlayerLeft(PlayerRef player)
        {
            for (int i = 0; i < _activePlayersObjectsList.Count; i++)
            {
                if (_activePlayersObjectsList[i].InputAuthority == player)
                {
                    _activePlayersObjectsList.RemoveAt(i);
                    break;
                }
            }
        }

        public void SpawnPlayers()
        {
            IEnumerable<PlayerRef> players = Runner.ActivePlayers;

            foreach (PlayerRef playerRef in players)
            {
                NetworkObject spawnedPlayer = SpawnPlayer(playerRef);
                _activePlayersObjectsList.Add(spawnedPlayer);
            }
        }

        private NetworkObject SpawnPlayer(PlayerRef playerRef)
        {
            Transform spawnPoint = GetRandomPlayerSpawnPoint().transform;
            return Runner.Spawn(_playerPrb, spawnPoint.position, spawnPoint.rotation, playerRef);
        }

        private PlayerSpawnPoint GetRandomPlayerSpawnPoint()
        {
            int randomIndex = Random.Range(0, _activePlayersSpawnPoints.Count);
            PlayerSpawnPoint playerSpawnPoint = _activePlayersSpawnPoints[randomIndex];
            _activePlayersSpawnPoints.RemoveAt(randomIndex);
            return playerSpawnPoint;
        }

#if UNITY_EDITOR

        [Odin.Button("Recreate spawn points list")]
        private void RecreatePlayersSpawnPointsList()
        {
            _playersSpawnPoints = FindObjectsOfType<PlayerSpawnPoint>();
        }
#endif
    }
}
