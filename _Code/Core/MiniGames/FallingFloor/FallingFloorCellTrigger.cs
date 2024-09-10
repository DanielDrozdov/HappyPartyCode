using System;
using UnityEngine;

namespace Core.MiniGames.FallingFloor
{
    public class FallingFloorCellTrigger : MonoBehaviour
    {
        public event Action OnEnteredLavaZone;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out LavaZone lavaZone))
            {
                OnEnteredLavaZone?.Invoke();
            }
        }
    }
}