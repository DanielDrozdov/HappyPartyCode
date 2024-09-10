using UnityEngine;

namespace Core.MainMenu.Podium
{
    public class Pedestal : MonoBehaviour
    {
        [SerializeField] 
        private int _number;

        [SerializeField] 
        private Transform _playerSpawnPos;
        
        public int Number => _number;

        public Vector3 PlayerSpawnPos => _playerSpawnPos.position;

        public Quaternion PlayerSpawnRotation => _playerSpawnPos.rotation;
    }
}