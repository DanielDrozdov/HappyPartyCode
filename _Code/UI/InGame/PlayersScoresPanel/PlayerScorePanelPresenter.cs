using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.InGame.PlayersScoresPanel
{
    [RequireComponent(typeof(PlayerScorePanelView))]
    public class PlayerScorePanelPresenter : MonoBehaviour
    {
        [SerializeField, ReadOnly] 
        private PlayerScorePanelView _view;

        private string _nickname;

        public void Activate(string nickname, int score)
        {
            _nickname = nickname;
            _view.UpdatePlayerData(nickname, score);
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void UpdateScore(int score)
        {
            _view.UpdatePlayerData(_nickname, score);
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_view == null) TryGetComponent(out _view);
        }
#endif
    }
}