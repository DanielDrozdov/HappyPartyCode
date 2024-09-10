using System.Collections.Generic;
using Core.Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.MiniGames.ScorePodium
{
    public class ScorePlayersPodium : MonoBehaviour
    {
        [SerializeField, ReadOnly] 
        private ScorePlayerPedestal[] _scorePlayersPedestals;

        private Dictionary<int, ScorePlayerPedestal> _playersPodiumsByNumberPlacesDict;

        private Vector3 _yOffset = Vector3.up * 0.1f;
        
        private void Awake()
        {
            InitializePlayersPodiumsDict();
        }

        public void PlacePlayerOnScorePedestal(PlayerMovement playerMovement, int playerPlace)
        {
            ScorePlayerPedestal scorePedestal = _playersPodiumsByNumberPlacesDict[playerPlace];
            playerMovement.Teleport(scorePedestal.PlayerSpawnPoint.position + _yOffset, scorePedestal.PlayerSpawnPoint.rotation);
        }

        public void SetPlayerScoreToPedestal(int playerPlace, int playerScore)
        {
            _playersPodiumsByNumberPlacesDict[playerPlace].SetScore(playerScore);
        }
        
        private void InitializePlayersPodiumsDict()
        {
            _playersPodiumsByNumberPlacesDict = new Dictionary<int, ScorePlayerPedestal>();

            for (int i = 0; i < _scorePlayersPedestals.Length; i++)
            {
                ScorePlayerPedestal scorePlayerPedestal = _scorePlayersPedestals[i];
                _playersPodiumsByNumberPlacesDict.Add(scorePlayerPedestal.PlaceNumber, scorePlayerPedestal);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_scorePlayersPedestals == null) _scorePlayersPedestals = GetComponentsInChildren<ScorePlayerPedestal>();
        }
#endif
    }
}