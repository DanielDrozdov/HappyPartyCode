using System;
using System.Collections.Generic;
using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;
using UpdateSys;

namespace Core.MiniGames.AncientLasers.Laser
{
    public class AncientLaserPlayersTrigger : NetworkBehaviour, IFixedUpdatable
    {
        [SerializeField] 
        private LayerMask _playersLayerMask;
        
        [SerializeField, Title("Box YZ size")] 
        private float _boxTriggerYSize;

        [SerializeField] 
        private float _boxTriggerZSize;

        private List<Collider> _hitPlayers;
        private Transform _transform;
        private Collider[] _overlapResults;
        private Vector3 _boxOverlapSize;
        private float _boxTriggerXSize = 5f;

        public event Action<Collider> OnCaughtPlayer;
        
        public override void Spawned()
        {
            if (!Object.HasStateAuthority) return;

            _hitPlayers = new List<Collider>();
            _overlapResults = new Collider[4];
            _transform = transform;
            this.StartFixedUpdate();
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            this.StopFixedUpdate();
        }

        public void Switch(bool value)
        {
            gameObject.SetActive(value);
            
            if (value)
            {
                this.StartFixedUpdate();
            }
            else
            {
                _hitPlayers.Clear();
                this.StopFixedUpdate();
            }
        }

        public void SetXSize(float xSize)
        {
            _boxOverlapSize = new Vector3(xSize, _boxTriggerYSize,_boxTriggerZSize);
        }
        
        private void FindPlayers()
        {
            int resultsCount = Physics.OverlapBoxNonAlloc(transform.position, _boxOverlapSize, _overlapResults, _transform.rotation, _playersLayerMask);

            for (int i = 0; i < resultsCount; i++)
            {
                CatchPlayer(_overlapResults[i]);
            }
        }

        private void CatchPlayer(Collider hitPlayerCollider)
        {
            if (_hitPlayers.Contains(hitPlayerCollider)) return;
            
            _hitPlayers.Add(hitPlayerCollider);
            OnCaughtPlayer?.Invoke(hitPlayerCollider);
        }
        
        public void OnSystemFixedUpdate(float deltaTime)
        {
            FindPlayers();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, transform.rotation * _boxOverlapSize);
        }
#endif
    }
}
