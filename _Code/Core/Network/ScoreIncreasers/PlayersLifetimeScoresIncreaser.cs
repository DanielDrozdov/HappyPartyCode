using System.Collections.ObjectModel;
using Core.Storages;
using Data.Levels.FallingFloorMiniGame;
using Fusion;
using UI.InGame.PlayersScoresPanel;
using UnityEngine;
using UpdateSys;
using Zenject;

namespace Core.Network.ScoreIncreasers
{
    public class PlayersLifetimeScoresIncreaser : NetworkBehaviour, IUpdatable
    {
        [SerializeField, Unit(Units.Seconds)] 
        private float _increaseIterationCallEverySeconds;

        [SerializeField] 
        private int _scoreIncreaseEveryIteration;

        private IPlayersMiniGameScoresStorage _playersMiniGameScoresStorage;
        private IMiniGamePlayersStorage _miniGamePlayersStorage;
        private IPlayersMiniGameScoresPanel _playersMiniGameScoresPanel;
        private IMiniGameCallbacks _miniGameCallbacks;
        private float _increaseIterationEverySecondsRemainder;

        [Inject]
        private void Construct(IPlayersMiniGameScoresStorage playersMiniGameScoresStorage, IMiniGamePlayersStorage miniGamePlayersStorage,
            IMiniGameCallbacks miniGameCallbacks, IPlayersMiniGameScoresPanel playersMiniGameScoresPanel)
        {
            _playersMiniGameScoresStorage = playersMiniGameScoresStorage;
            _miniGamePlayersStorage = miniGamePlayersStorage;
            _playersMiniGameScoresPanel = playersMiniGameScoresPanel;
            _miniGameCallbacks = miniGameCallbacks;
        }

        public void OnSystemUpdate(float deltaTime)
        {
            _increaseIterationEverySecondsRemainder -= deltaTime;

            if (_increaseIterationEverySecondsRemainder <= 0)
            {
                _increaseIterationEverySecondsRemainder = _increaseIterationCallEverySeconds;
                IncreaseScoresForActivePlayers();
            }
        }

        public override void Spawned()
        {
            if (Runner.IsServer)
            {
                _miniGameCallbacks.OnMiniGameStarted += StartIncreaser;
                _miniGameCallbacks.OnMiniGameEnded += StopIncreaser;
            }
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            if (!Runner.IsServer) return;
            
            this.StopUpdate();
        }

        private void StartIncreaser()
        {
            if (!Runner.IsServer) return;    
            
            _increaseIterationEverySecondsRemainder = _increaseIterationCallEverySeconds;
            this.StartUpdate();
        }

        private void StopIncreaser()
        {
            if (!Runner.IsServer) return;    
            
            this.StopUpdate();
        }

        private void IncreaseScoresForActivePlayers()
        {
            ReadOnlyCollection<PlayerRef> activeLivePlayers = _miniGamePlayersStorage.ActiveLivePlayers;
            
            PlayerMiniGameScoreTransfer[] playersMiniGameScoreTransfers = new PlayerMiniGameScoreTransfer[activeLivePlayers.Count];
            
            for (int i = 0; i < playersMiniGameScoreTransfers.Length; i++)
            {
                PlayerRef playerRef = activeLivePlayers[i];
                PlayerMiniGameScoreTransfer scoreTransfer = new PlayerMiniGameScoreTransfer((byte)playerRef.PlayerId, _scoreIncreaseEveryIteration);
                playersMiniGameScoreTransfers[i] = scoreTransfer;
            }

            RPC_IncreasePlayersScores(playersMiniGameScoreTransfers);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_IncreasePlayersScores(PlayerMiniGameScoreTransfer[] playersMiniGameScoreTransfers)
        {
            for (int i = 0; i < playersMiniGameScoreTransfers.Length; i++)
            {
                PlayerMiniGameScoreTransfer scoreTransfer = playersMiniGameScoreTransfers[i];
                _playersMiniGameScoresStorage.AddMiniGameScoreToPlayer(scoreTransfer.PlayerId, scoreTransfer.Score);
            }
            
            _playersMiniGameScoresPanel.UpdateMiniGamePlayersScores();
        }
    }
}
