using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGameMenu
{
    [RequireComponent(typeof(InGameMenuPresenter))]
    public class InGameMenuView : MonoBehaviour
    {
        [SerializeField] 
        private Button _returnToGameButton;

        [SerializeField] 
        private Button _leaveGameButton;

        [SerializeField, ReadOnly] 
        private InGameMenuPresenter _presenter;

        private void Awake()
        {
            _returnToGameButton.onClick.AddListener(ReturnToGame_OnClickButton);
            _leaveGameButton.onClick.AddListener(LeaveGame_OnClickButton);
        }

        private void ReturnToGame_OnClickButton()
        {
            _presenter.ReturnToGame();
        }

        private void LeaveGame_OnClickButton()
        {
            _presenter.LeaveGame();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_presenter == null) TryGetComponent(out _presenter);
        }
#endif
    }
}