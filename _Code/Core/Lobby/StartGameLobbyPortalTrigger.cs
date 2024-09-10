using System;
using System.Collections.Generic;
using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;
using UpdateSys;
using Units = Sirenix.OdinInspector.Units;

namespace Core.Lobby
{
    public class StartGameLobbyPortalTrigger : NetworkBehaviour, IFixedUpdatable
    {
        [SerializeField, MinValue(0), Sirenix.OdinInspector.Unit(Units.Meter)] 
        private float _triggerRadius;

        [SerializeField] 
        private LayerMask _playerLayerMask;
        
        private Collider[] _sphereOverlapPlayersResult;
        private List<Collider> _playersInZone;
        private List<Collider> _playersInZoneOnLastIteration;
        private Transform _transform;
        private int _lastPlayersInZoneCount;
        private int _collectionsSize = 4;

        public event Action<int> OnPlayersCountInZoneUpdated;

        private void Awake()
        {
            _sphereOverlapPlayersResult = new Collider[_collectionsSize];
            _playersInZone = new List<Collider>(_collectionsSize);
            _playersInZoneOnLastIteration = new List<Collider>(_collectionsSize);
            _transform = transform;
        }

        public override void Spawned()
        {
            if (!Object.HasStateAuthority) return;
            
            this.StartFixedUpdate();
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            this.StopFixedUpdate();
        }

        public void OnSystemFixedUpdate(float deltaTime)
        {
            FindPlayers();
            CheckIfPlayersInZoneListChanged();
        }

        private void CheckIfPlayersInZoneListChanged()
        {
            if (_playersInZone.Count != _lastPlayersInZoneCount)
            {
                OnPlayersCountInZoneUpdated?.Invoke(_playersInZone.Count);
                _lastPlayersInZoneCount = _playersInZone.Count;
            }
        }

        private void FindPlayers()
        {
            FillListWithPlayersOnLastIteration();
            int foundColliders = Physics.OverlapSphereNonAlloc(_transform.position, _triggerRadius, _sphereOverlapPlayersResult, _playerLayerMask);

            for (int i = 0; i < foundColliders; i++)
            {
                Collider player = _sphereOverlapPlayersResult[i];
                
                if (!_playersInZone.Contains(player))
                {
                    _playersInZone.Add(player);
                }
                else
                {
                    _playersInZoneOnLastIteration.Remove(player);
                }
            }

            for (int i = 0; i < _playersInZoneOnLastIteration.Count; i++)
            {
                _playersInZone.Remove(_playersInZoneOnLastIteration[i]);
            }
        }

        private void FillListWithPlayersOnLastIteration()
        {
            _playersInZoneOnLastIteration.Clear();
            
            foreach (Collider player in _playersInZone)
            {
                _playersInZoneOnLastIteration.Add(player);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _triggerRadius);
        }
    }
}
