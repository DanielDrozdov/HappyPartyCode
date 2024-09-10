using Fusion;
using UnityEngine;

namespace UI.Player
{
    public class PodiumPlayerKickButtonPresenter : NetworkBehaviour
    {
        [SerializeField, Sirenix.OdinInspector.ReadOnly] 
        private PodiumPlayerKickButtonView _view;
        
        public override void Spawned()
        {
            if (HasInputAuthority || !HasStateAuthority) return;

            _view.SwitchButton(true);
        }

        public void KickCurrentPlayer()
        {
            Runner.Disconnect(Object.InputAuthority);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_view == null) TryGetComponent(out _view);
        }
#endif
    }
}