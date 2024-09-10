using System;
using Sirenix.OdinInspector;
using UI.Screens;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.InGame
{
    public class MenuPanelButtonSwitcher : MonoBehaviour
    {
        [SerializeField, ReadOnly] 
        private Button _button;
        
        private IInGameUIScreensSwitcher _inGameUIScreensSwitcher;

        [Inject]
        private void Construct(IInGameUIScreensSwitcher inGameUIScreensSwitcher)
        {
            _inGameUIScreensSwitcher = inGameUIScreensSwitcher;
        }

        private void Awake()
        {
            _button.onClick.AddListener(SwitchOnMenu_OnClickButton);
        }

        private void SwitchOnMenu_OnClickButton()
        {
            _inGameUIScreensSwitcher.OpenMenuScreen();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_button == null) TryGetComponent(out _button);
        }
#endif
    }
}
