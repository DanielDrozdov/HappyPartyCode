using System;
using Core.Network;
using Core.Storages;
using Data.Levels.AncientLasers;
using Fusion;
using Sirenix.OdinInspector;
using UI.InGame.Timers;
using UnityEngine;
using UpdateSys;
using Random = UnityEngine.Random;

namespace Core.MiniGames.AncientLasers.Arena
{
    public class AncientArenaLasersThrower : MiniGameStarter, IUpdatable
    {
        [SerializeField, MinValue(0)] 
        private float _timeToSpawnNewLasersPair;

        [SerializeField, Space(15)] 
        private AncientLasersCountThrowChance[] _ancientLasersCountThrowChances;
        
        [SerializeField, Sirenix.OdinInspector.ReadOnly, Space(15)] 
        private AncientLasersArenaBorder[] _arenaBorders;

        private AncientLasersThrowCountRandomizer _lasersThrowCountRandomizer;
        private AncientLasersArenaSettings _ancientLasersArenaSettings;
        private IMiniGamePlayersStorage _playersStorage;
        private IMiniGameDurationCountdownTimer _miniGameDurationCountdownTimer;
        private float _timeToSpawnNewLasersPairRemainder;

        private void Awake()
        {
            _lasersThrowCountRandomizer = new AncientLasersThrowCountRandomizer(_ancientLasersCountThrowChances);
            _timeToSpawnNewLasersPairRemainder = _timeToSpawnNewLasersPair;
        }
        
        public void Inject(AncientLasersArenaSettings ancientLasersArenaSettings, IMiniGamePlayersStorage playersStorage, 
            IMiniGameDurationCountdownTimer miniGameDurationCountdownTimer)
        {
            _ancientLasersArenaSettings = ancientLasersArenaSettings;
            _miniGameDurationCountdownTimer = miniGameDurationCountdownTimer;
            _playersStorage = playersStorage;
        }

        public void OnSystemUpdate(float deltaTime)
        {
            if (_playersStorage.ActiveLivePlayers.Count == 0)
            {
                EndMiniGame();
                return;
            }

            CountToNextLaserThrow(deltaTime);
        }

        public override void StartMiniGame()
        {
            base.StartMiniGame();
            this.StartUpdate();

            RPC_StartMiniGameDurationTimer();
        }

        protected override void EndMiniGame()
        {
            base.EndMiniGame();
            this.StopUpdate();
            
            _miniGameDurationCountdownTimer.BreakCountdown();
        }

        private void CountToNextLaserThrow(float deltaTime)
        {
            _timeToSpawnNewLasersPairRemainder -= deltaTime;

            if (_timeToSpawnNewLasersPairRemainder <= 0)
            {
                ThrowLasers();
            }
        }

        private void ThrowLasers()
        {
            _timeToSpawnNewLasersPairRemainder = _timeToSpawnNewLasersPair;
            int randomBorderIndex = Random.Range(0, _arenaBorders.Length);
            int lasersCount = _lasersThrowCountRandomizer.GetRandomLasersCountToThrow();
            _arenaBorders[randomBorderIndex].ActivateLasers(lasersCount);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_StartMiniGameDurationTimer()
        {
            Action endTimerAction = Runner.IsServer ? EndMiniGame : null;

            if (Runner.IsClient)
            {
                OnMiniGameEnded += _miniGameDurationCountdownTimer.BreakCountdown;
            }
            
            _miniGameDurationCountdownTimer.StartCountdown(_ancientLasersArenaSettings.MiniGamePlayingTime, endTimerAction);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_arenaBorders.Length == 0) _arenaBorders = GetComponentsInChildren<AncientLasersArenaBorder>();
        }
#endif
    }
}
