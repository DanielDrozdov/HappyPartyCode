using Sirenix.OdinInspector;
using UI.BaseElements.Timers;
using UI.InGame.Input;
using UI.InGame.PlayersScoresPanel;
using UI.Screens;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(InGameUIScreensSwitcher))]
    public class NetworkSceneUICanvas : SceneUICanvas
    {
        [field: SerializeField, ReadOnly] 
        public CountdownTimer CountdownTimer { get; private set; }

        [field: SerializeField, ReadOnly] 
        public ScoreResultsClosingCountdownTimer ScoreResultsClosingCountdownTimer { get; private set; }
        
        [field: SerializeField, ReadOnly] 
        public InputPanelSwitcher InputPanelSwitcher { get; private set; }

        [field: SerializeField, ReadOnly] 
        public InGameUIScreensSwitcher GameScreensSwitcher { get; private set; }
        
        [field: SerializeField, ReadOnly] 
        public PlayersScoresPanel PlayersScoresPanel { get; private set; }



#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (CountdownTimer == null) CountdownTimer = GetComponentInChildren<CountdownTimer>();

            if (ScoreResultsClosingCountdownTimer == null) ScoreResultsClosingCountdownTimer = GetComponentInChildren<ScoreResultsClosingCountdownTimer>();
            
            if (InputPanelSwitcher == null) InputPanelSwitcher = GetComponentInChildren<InputPanelSwitcher>();

            if (GameScreensSwitcher == null) GameScreensSwitcher = GetComponentInChildren<InGameUIScreensSwitcher>();
            
            if (PlayersScoresPanel == null) PlayersScoresPanel = GetComponentInChildren<PlayersScoresPanel>();
        }
#endif
    }
}