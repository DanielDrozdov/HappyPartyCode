using UnityEngine;

namespace Core.MiniGames
{
    public class DeactivatedPlayersPlatform : MonoBehaviour, IDeactivatedPlayersPlatform
    {
        public Vector3 GetPlatformRandomPosition() => transform.position + _yOffset + GetRandomXZPosition();

        private Vector3 GetRandomXZPosition() => new Vector3(Random.Range(-_randomXZOffsetPos, _randomXZOffsetPos), 0,
            Random.Range(-_randomXZOffsetPos, _randomXZOffsetPos));
        
        private readonly Vector3 _yOffset = new Vector3(0, 0.5f, 0);
        private readonly float _randomXZOffsetPos = 2f;

    }
}
