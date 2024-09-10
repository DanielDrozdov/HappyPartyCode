using Core.MiniGames.AncientLasers.Laser;
using Data.Levels.AncientLasers;
using Fusion;
using UnityEngine;
using UpdateSys;
using Zenject;

namespace Core.MiniGames.AncientLasers.Arena
{
    [RequireComponent(typeof(AncientLasersArenaBorderWarnActivator))]
    [RequireComponent(typeof(AncientLasersArenaBorderLasersSpawner))]
    public class AncientLasersArenaBorder : NetworkBehaviour, IUpdatable
    {
        [SerializeField] 
        private Transform _oppositeWallTrm;

        [SerializeField] 
        private Transform _leftCornerPoint;
        
        [SerializeField] 
        private Transform _rightCornerPoint;
        
        [SerializeField, Sirenix.OdinInspector.ReadOnly]
        private AncientLasersArenaBorderWarnActivator _warnActivator;
        
        [SerializeField, Sirenix.OdinInspector.ReadOnly]
        private AncientLasersArenaBorderLasersSpawner _lasersSpawner;
        
        private AncientLasersArenaSettings _arenaSettings;
        private int _lasersCountToSpawnRemainder;
        private float _timeRemainderToSpawnNextLaser;
        private readonly float _laserStartForwardOffset = 0.3f;
        private readonly Vector3 _laserStartYOffset = Vector3.up * 1.3f; 

        [Inject]
        private void Construct(AncientLasersArenaSettings arenaSettings)
        {
            _arenaSettings = arenaSettings;
        }

        public void OnSystemUpdate(float deltaTime)
        {
            _timeRemainderToSpawnNextLaser -= deltaTime;
            
            if (_timeRemainderToSpawnNextLaser <= 0)
            {
                ActivateLaser();
            }
        }

        public void ActivateLasers(int lasersCountToSpawn)
        {
            _lasersCountToSpawnRemainder = lasersCountToSpawn;
            _warnActivator.ActivateWarnState(OnLasersSpawningWarnEnded);
        }

        private void OnLasersSpawningWarnEnded()
        {
            if (!Runner.IsServer) return;
            
            ActivateLaser();
            
            if (_lasersCountToSpawnRemainder >= 1)
            {
                this.StartUpdate();
            }
        }

        private void ActivateLaser()
        {
            _lasersCountToSpawnRemainder--;
            _timeRemainderToSpawnNextLaser = _arenaSettings.SpawnTimeDelayBetweenMultiplyLasers;
            float borderWidth = Vector3.Distance(_leftCornerPoint.position, _rightCornerPoint.position);

            AncientLaser ancientLaser = _lasersSpawner.SpawnLaser();
            Vector3 startLaserPos = transform.position + transform.forward * _laserStartForwardOffset + _laserStartYOffset;
            ancientLaser.ActivateLaserInDirection(borderWidth, startLaserPos, transform.rotation,_oppositeWallTrm.position);
            
            if (_lasersCountToSpawnRemainder == 0)
            {
                this.StopUpdate();
            }
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (_warnActivator == null) TryGetComponent(out _warnActivator);

            if (_lasersSpawner == null) TryGetComponent(out _lasersSpawner);
        }
#endif
    }
}
