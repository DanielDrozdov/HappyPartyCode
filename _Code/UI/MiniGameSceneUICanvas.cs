using Sirenix.OdinInspector;
using UI.InGame.PlayersScoresPanel;
using UI.InGame.Timers;
using UnityEngine;

namespace UI
{
    public class MiniGameSceneUICanvas : NetworkSceneUICanvas
    {
        [field: SerializeField, ReadOnly] 
        public MiniGameDurationCountdownTimer GameDurationCountdownTimer { get; private set; }
        
        public PlayersMiniGameScoresPanel MiniGamePlayersScoresPanel  => PlayersScoresPanel as PlayersMiniGameScoresPanel;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (GameDurationCountdownTimer == null) GameDurationCountdownTimer = GetComponentInChildren<MiniGameDurationCountdownTimer>();
        }
#endif
    }
}