using Core.Storages;
using Data.Levels.AncientLasers;
using UI.InGame.Timers;
using UnityEngine;
using Zenject;

namespace Core.MiniGames.AncientLasers.Arena
{
    public class AncientArenaThrowerInject : MonoBehaviour
    {
        [SerializeField] 
        private AncientArenaLasersThrower _thrower;
        
        [Inject]
        private void Construct(AncientLasersArenaSettings ancientLasersArenaSettings, IMiniGamePlayersStorage playersStorage, 
            IMiniGameDurationCountdownTimer miniGameDurationCountdownTimer)
        {
            _thrower.Inject(ancientLasersArenaSettings, playersStorage, miniGameDurationCountdownTimer);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_thrower == null) TryGetComponent(out _thrower);
        }
#endif
    }
}