using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Infrastructure.Network;
using UnityEngine;
using Zenject;

namespace Core.MainMenu.Podium
{
    public class PlayersPodium : MonoBehaviour, IPlayersPodium
    {
        [SerializeField] 
        private Pedestal[] _pedestals;

        private Dictionary<int, Pedestal> _pedestalsByNumberDict;
        private Dictionary<PlayerRef, PlayerOnPedestalData> _playersOnPedestalsDict;
        private INetworkConnectorCallbacksObserver _runnerCallbacks;

        private void Awake()
        {
            SortPedestalsByNumber();
            _runnerCallbacks.OnSetNewRoomConnection += SubscribeOnNetworkEvents;
        }

        private void OnDestroy()
        {
            if (_runnerCallbacks == null) return;

            UnsubscribeFromNetworkEvents();
        }

        [Inject]
        private void Construct(INetworkConnectorCallbacksObserver runnerCallbacks)
        {
            _runnerCallbacks = runnerCallbacks;
        }

        public Pedestal GetPedestalByNumber(int number)
        {
            return _pedestalsByNumberDict.GetValueOrDefault(number);
        }

        public void ReservePedestalForPlayer(int pedestalNumber, NetworkObject player)
        {
            if (_playersOnPedestalsDict.ContainsKey(player.InputAuthority))
            {
                Debug.LogError("Pedestal is taken");
                return;
            }
            
            _playersOnPedestalsDict.Add(player.InputAuthority, new PlayerOnPedestalData(player, pedestalNumber));
        }

        public void FreePedestalFromPlayer(PlayerRef player)
        {
            if (!_playersOnPedestalsDict.ContainsKey(player))
            {
                return;
            }

            _playersOnPedestalsDict.Remove(player);

            if (_playersOnPedestalsDict.Count > 1)
            {
                SortPlayersByPedestals();
            }
        }

        private void SortPlayersByPedestals()
        {
            Dictionary<int, PlayerOnPedestalData> playersOnPedestalsData = _playersOnPedestalsDict.Values
                .ToDictionary(x => x.PedestalNumber, x => x);
            
            for (int pedestalNumber = 1; pedestalNumber < _pedestals.Length; pedestalNumber++)
            {
                if (!playersOnPedestalsData.ContainsKey(pedestalNumber))
                {
                    if (!playersOnPedestalsData.ContainsKey(pedestalNumber + 1))
                    {
                        break;
                    }
                    
                    PlaceNextPlayerOnCurrentPedestal(playersOnPedestalsData, pedestalNumber);
                }
            }
        }
        
        private void SubscribeOnNetworkEvents(NetworkRunner runner)
        {
            _runnerCallbacks.NetworkCallbacksEvents.OnNetworkShutdown += ClearPodiumReservedPedestalsDict;
        }
        
        private void UnsubscribeFromNetworkEvents()
        {
            _runnerCallbacks.OnSetNewRoomConnection -= SubscribeOnNetworkEvents;

            if (_runnerCallbacks.NetworkCallbacksEvents != null)
            {
                _runnerCallbacks.NetworkCallbacksEvents.OnNetworkShutdown -= ClearPodiumReservedPedestalsDict;
            }
        }

        private void ClearPodiumReservedPedestalsDict()
        {
            _playersOnPedestalsDict.Clear();
        }

        private void PlaceNextPlayerOnCurrentPedestal(Dictionary<int, PlayerOnPedestalData> playersOnPedestalsData, int currentPedestalNumber)
        {
            int nextPedestalNumber = currentPedestalNumber + 1;
            NetworkObject player = playersOnPedestalsData[nextPedestalNumber].Player;
            player.transform.position = GetPedestalByNumber(currentPedestalNumber).PlayerSpawnPos;
            _playersOnPedestalsDict.Remove(player.InputAuthority);
            ReservePedestalForPlayer(currentPedestalNumber, player);
        }

        private void SortPedestalsByNumber()
        {
            _pedestalsByNumberDict = new Dictionary<int, Pedestal>();
            _playersOnPedestalsDict = new Dictionary<PlayerRef, PlayerOnPedestalData>();

            foreach (Pedestal pedestal in _pedestals)
            {
                _pedestalsByNumberDict.Add(pedestal.Number, pedestal);
            }
        }
    }
}
