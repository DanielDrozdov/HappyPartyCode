using Core.MiniGames.AncientLasers.Laser;
using Fusion;
using UnityEngine;

namespace Core.MiniGames.AncientLasers.Arena
{
    public class AncientLasersArenaBorderLasersSpawner : NetworkBehaviour
    {
        [SerializeField, Sirenix.OdinInspector.ReadOnly]
        private AncientArenaLasersPool _lasersPool;
        
        public AncientLaser SpawnLaser()
        {
            int freeLaserId = _lasersPool.GetFreeLaserId();
            return  _lasersPool.GetLaserFromPool(freeLaserId);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_lasersPool == null) _lasersPool = FindObjectOfType<AncientArenaLasersPool>();
        }
#endif
    }
}