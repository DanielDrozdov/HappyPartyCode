using DG.Tweening;
using UnityEngine;

namespace Core.MiniGames.FallingFloor
{
    public class FallingFloorCellAnimation : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _shakeVector = new (0, 1, 1);
        
        [SerializeField]
        private int _vibratoPower = 35;
        
        [SerializeField]
        private float _shakeStrength = 2f;

        public void StartShakeRotation(float destructionTime, TweenCallback onComplete)
        {
            transform
                .DOShakeRotation(destructionTime, _shakeVector * _shakeStrength, _vibratoPower, 90, false)
                .OnComplete(onComplete);
        }
    }
}