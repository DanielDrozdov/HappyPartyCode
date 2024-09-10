using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Core.MainMenu.Podium;
using Core.MiniGames.ScorePodium;
using Core.Player;
using Core.Player.Animations;
using Fusion;
using Infrastructure.Services.Input;
using Odin = Sirenix.OdinInspector;
using UI.InGame.Input;
using UI.InGame.PlayersScoresPanel;
using UnityEngine;
using Zenject;

namespace Core.Network.SceneHandlers
{
    public abstract class BaseGameResultsDisplayer : NetworkBehaviour
    {
        [SerializeField, Odin.ReadOnly, Odin.FoldoutGroup("Components")]
        private SceneNetworkPlayersSpawner _networkPlayersSpawner;
        
        [SerializeField, Odin.ReadOnly, Odin.FoldoutGroup("Components")] 
        private ScorePlayersPodium _scorePlayersPodium;
        
        [SerializeField, Odin.ReadOnly, Odin.FoldoutGroup("Components")] 
        private ScorePlayersPodiumEffects _scorePodiumEffects;
        
        [SerializeField, Odin.ReadOnly, Odin.FoldoutGroup("Components")] 
        private ScorePodiumCamera _scorePodiumCamera;
        
        private IApplicationInputBlocker _applicationInputBlocker;
        private IInputPanelSwitcher _inputPanelSwitcher;
        private IPlayersScoresPanelDeactivator _playersScoresPanelDeactivator;

        [Inject]
        private void BaseConstruct(IApplicationInputBlocker applicationInputBlocker, IInputPanelSwitcher inputPanelSwitcher,
            IPlayersScoresPanelDeactivator playersScoresPanelDeactivator)
        {
            _inputPanelSwitcher = inputPanelSwitcher;
            _applicationInputBlocker = applicationInputBlocker;
            _playersScoresPanelDeactivator = playersScoresPanelDeactivator;
        }

        protected abstract ReadOnlyDictionary<PlayerRef, int> GetPlayersScores();
        
        protected void DisplayResults()
        {
            PreparePlayerForFinalResults();
            SetUpPodiumAndPlayers();
        }

        protected virtual void SetUpPodiumAndPlayers()
        {
            ReadOnlyDictionary<PlayerRef, int> playersScores = GetPlayersScores();
            List<PlayerRef> sortedPlayersByScore = SortPlayersByScore(playersScores);
            SetScoresForPlayersPlaces(sortedPlayersByScore, playersScores);
            _scorePodiumCamera.StartAnimationLookTransferToPlayersScorePodium(OnScorePodiumLookTransferCompleted);

            if (Runner.IsServer)
            {
                PlacePlayersOnScorePodium(sortedPlayersByScore);
                PlayPlayersStateEmotions(sortedPlayersByScore);
            }
        }
        
        protected virtual void OnScorePodiumLookTransferCompleted()
        {
            _scorePodiumEffects.PlayConfettiEffect();
        }
        
        private void PreparePlayerForFinalResults()
        {
            _inputPanelSwitcher.Hide();
            _applicationInputBlocker.BlockUserInput();
            _playersScoresPanelDeactivator.DeactivateScorePanels();
        }

        #region Players Emotion callers

        private void PlayPlayersStateEmotions(List<PlayerRef> sortedPlayersByScore)
        {
            ReadOnlyCollection<NetworkObject> players = _networkPlayersSpawner.ReadOnlyActivePlayersObjectsList;

            for (int i = 0; i < players.Count; i++)
            {
                PlayPlayerStateEmotion(players[i], sortedPlayersByScore);
            }
        }

        private void PlayPlayerStateEmotion(NetworkObject player, List<PlayerRef> sortedPlayersByScore)
        {
            PlayerScorePodiumEmotions playerEmotions = player.GetComponentInChildren<PlayerScorePodiumEmotions>();
            int playerPlace = sortedPlayersByScore.IndexOf(player.InputAuthority) + 1;

            Action playStateEmotionAction = playerPlace == 1 ? playerEmotions.PlayVictoryEmotion : playerEmotions.PlayLossEmotion;
            playStateEmotionAction.Invoke();
        }

        #endregion

        #region Podium placers

        private void PlacePlayersOnScorePodium(List<PlayerRef> sortedPlayersByScore)
        {
            ReadOnlyCollection<NetworkObject> players = _networkPlayersSpawner.ReadOnlyActivePlayersObjectsList;

            for (int i = 0; i < players.Count; i++)
            {
                PlacePlayerOnScorePodium(sortedPlayersByScore, players[i]);
            }
        }

        private void PlacePlayerOnScorePodium(List<PlayerRef> sortedPlayersByScore, NetworkObject player)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            int playerPlace = sortedPlayersByScore.IndexOf(player.InputAuthority) + 1;
            _scorePlayersPodium.PlacePlayerOnScorePedestal(playerMovement, playerPlace);
        }

        #endregion
        
        #region Place score setters
        
        private void SetScoresForPlayersPlaces(List<PlayerRef> sortedPlayersByScore, ReadOnlyDictionary<PlayerRef, int> playersScores)
        {
            for (int i = 0; i < sortedPlayersByScore.Count; i++)
            {
                SetScoreForPlayerPlace(sortedPlayersByScore, playersScores, sortedPlayersByScore[i]);
            }
        }

        private void SetScoreForPlayerPlace(List<PlayerRef> sortedPlayersByScore, ReadOnlyDictionary<PlayerRef, int> playersScores, PlayerRef player)
        {
            int playerPlace = sortedPlayersByScore.IndexOf(player) + 1;
            int playerScore = playersScores[player];
            _scorePlayersPodium.SetPlayerScoreToPedestal(playerPlace, playerScore);
        }

        #endregion
        
        private List<PlayerRef> SortPlayersByScore(ReadOnlyDictionary<PlayerRef, int> playersScoresDict)
        {
            return playersScoresDict
                .OrderByDescending(x => x.Value)
                .Select(x => x.Key)
                .ToList();
        }
        
#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (_scorePodiumEffects == null) _scorePodiumEffects = FindObjectOfType<ScorePlayersPodiumEffects>();
            
            if (_scorePlayersPodium == null) _scorePlayersPodium = FindObjectOfType<ScorePlayersPodium>();
            
            if (_scorePodiumCamera == null) _scorePodiumCamera = FindObjectOfType<ScorePodiumCamera>();

            if (_networkPlayersSpawner == null) TryGetComponent(out _networkPlayersSpawner);
        }
#endif
    }
}