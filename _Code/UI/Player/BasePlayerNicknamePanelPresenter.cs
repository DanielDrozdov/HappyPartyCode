using Fusion;
using UnityEngine;

namespace UI.Player
{
    public class BasePlayerNicknamePanelPresenter : NetworkBehaviour
    {
        [SerializeField, Sirenix.OdinInspector.ReadOnly] 
        protected PlayerNicknamePanelView _view;
        
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_view == null) TryGetComponent(out _view);
        }
#endif
    }
}