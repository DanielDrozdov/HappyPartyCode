using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Input
{
    public class JumpButton : MonoBehaviour
    {
        [SerializeField, ReadOnly] 
        private Button _button;

        public static event Action OnPressed; 
        
        private void Awake()
        {
            _button.onClick.AddListener(Jump_OnButtonClick);
        }

        private void Jump_OnButtonClick()
        {
            OnPressed?.Invoke();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_button == null) TryGetComponent(out _button);
        }
#endif
    }
}
