using System.Collections.Generic;
using Core.MiniGames.AncientLasers.Laser;
using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.MiniGames.AncientLasers.Arena
{
    public class AncientArenaLasersPool : NetworkBehaviour
    {
        [SerializeField, Sirenix.OdinInspector.ReadOnly]
        private AncientLaser[] _ancientLasers;

        private List<AncientLaser> _freeLasers;
        private Dictionary<int, AncientLaser> _lasersByIdDict;

        private void Awake()
        {
            FillLasersByIdDictionary();
            _freeLasers = new List<AncientLaser>(_ancientLasers);
        }
        
        public int GetFreeLaserId()
        {
            int randomFreeLaserIndex = Random.Range(0, _freeLasers.Count);
            return _freeLasers[randomFreeLaserIndex].Id;
        }
        
        public AncientLaser GetLaserFromPool(int id)
        {
            AncientLaser laser = _lasersByIdDict[id];
            _freeLasers.Remove(laser);
            return laser;
        }

        public void ReturnLaserToPool(AncientLaser laser)
        {
            _freeLasers.Add(laser);
        }

        private void FillLasersByIdDictionary()
        {
            _lasersByIdDict = new Dictionary<int, AncientLaser>();
            
            for (int i = 0; i < _ancientLasers.Length; i++)
            {
                AncientLaser laser = _ancientLasers[i];
                laser.SetLasersPool(this);
                _lasersByIdDict.Add(laser.Id, laser);
            }
        }

#if UNITY_EDITOR
        [Button("Regenerate Lasers Pool")]
        private void RegenerateLasersPool()
        {
            _ancientLasers = GetComponentsInChildren<AncientLaser>();
        }
#endif
    }
}