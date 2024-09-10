using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace UI.InGame.PlayersScoresPanel
{
    [RequireComponent(typeof(PlayersScoresPanelActivator))]
    public class PlayersScoresPanel : MonoBehaviour, IPlayersScoresPanelDeactivator
    {
        [SerializeField, ReadOnly] 
        protected PlayersScoresPanelActivator _activator;
        
        protected Dictionary<PlayerRef, PlayerScorePanelPresenter> _playersPanelsDict;
        
        private void Awake()
        {
            _playersPanelsDict = new Dictionary<PlayerRef, PlayerScorePanelPresenter>();
            _activator.InitializeScoresPanel(_playersPanelsDict);
        }

        public void DeactivateScorePanels()
        {
            foreach (PlayerScorePanelPresenter playerScorePanel in _playersPanelsDict.Values)
            {
                playerScorePanel.Deactivate();
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_activator == null) TryGetComponent(out _activator);
        }
#endif
    }
}
