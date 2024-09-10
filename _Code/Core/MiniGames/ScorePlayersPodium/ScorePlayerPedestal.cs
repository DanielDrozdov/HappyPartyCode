using TMPro;
using UnityEngine;

namespace Core.MiniGames.ScorePodium
{
    public class ScorePlayerPedestal : MonoBehaviour
    {
        [SerializeField] 
        private int _placeNumber;

        [SerializeField] 
        private Transform _playerSpawnPoint;

        [SerializeField] 
        private TMP_Text _scoreText;

        public int PlaceNumber => _placeNumber;

        public Transform PlayerSpawnPoint => _playerSpawnPoint;

        public void SetScore(int playerScore)
        {
            _scoreText.text = $"+{playerScore}";
        }
    }
}
