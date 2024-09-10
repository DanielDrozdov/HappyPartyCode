using Data.Save;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu.Room
{
    public class RoomConnectionPanelView : MonoBehaviour
    {
        [SerializeField] 
        private Button _connectToRoomButton;
        
        [SerializeField] 
        private Button _createRoomButton;

        [SerializeField] 
        private TMP_InputField _nicknameInputField;

        [SerializeField] 
        private TMP_InputField _roomNameInputField;

        [SerializeField, ReadOnly] 
        private RoomUIErrorDisplay _errorDisplay;
        
        [SerializeField, ReadOnly] 
        private RoomConnectionPanelPresenter _presenter;
        
        private void Awake()
        {
            _connectToRoomButton.onClick.AddListener(ConnectToRoom_OnClickButton);
            _createRoomButton.onClick.AddListener(CreateRoom_OnClickButton);
        }

        public void DisplayConnectionError(string errorMessage)
        {
            _errorDisplay.Display(errorMessage);
        }

        public void UpdateNickname(string nickname)
        {
            _nicknameInputField.text = nickname;
        }
        
        private void ConnectToRoom_OnClickButton()
        {
            if (!IsNicknameValid()) return;

            _presenter.SaveNickname(_nicknameInputField.text);
            _presenter.ConnectToRoom(_roomNameInputField.text);
        }

        private void CreateRoom_OnClickButton()
        {
            if (!IsNicknameValid()) return;

            if (!IsRoomNameValid()) return;

            _presenter.SaveNickname(_nicknameInputField.text);
            _presenter.CreateRoom(_roomNameInputField.text);
        }

        private bool IsNicknameValid()
        {
            if (string.IsNullOrWhiteSpace(_nicknameInputField.text))
            {
                DisplayConnectionError("Nickname is empty");
                return false;
            }

            return true;
        }
        
        private bool IsRoomNameValid()
        {
            if (string.IsNullOrWhiteSpace(_roomNameInputField.text))
            {
                DisplayConnectionError("Can't create a room with an empty name");
                return false;
            }

            return true;
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (_presenter == null) TryGetComponent(out _presenter);

            if (_errorDisplay == null) _errorDisplay = GetComponentInChildren<RoomUIErrorDisplay>();
        }
#endif
    }
}
