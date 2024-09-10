using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Player
{
    public class PodiumPlayerKickButtonView : MonoBehaviour
    {
        [SerializeField] 
        private Button _button;
        
        [SerializeField, ReadOnly] 
        private PodiumPlayerKickButtonPresenter _presenter;

        private void Awake()
        {
            _button.onClick.AddListener(_presenter.KickCurrentPlayer);
        }

        public void SwitchButton(bool state)
        {
            _button.gameObject.SetActive(state);
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_presenter == null) TryGetComponent(out _presenter);
        }
#endif
    }
}
