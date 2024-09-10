using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UpdateSys;

namespace UI.MainMenu.Room
{
    public class RoomUIErrorDisplay : MonoBehaviour, IUpdatable
    {
        [SerializeField, Unit(Units.Second)] 
        private float _activatedTime;

        [SerializeField, ReadOnly] 
        private TMP_Text _text;
        
        private float _activatedTimeBalance;

        public void OnSystemUpdate(float deltaTime)
        {
            _activatedTimeBalance -= Time.deltaTime;

            if (_activatedTimeBalance <= 0)
            {
                _text.text = string.Empty;
                gameObject.SetActive(false);
                this.StopUpdate();
            }
        }

        private void OnDestroy()
        {
            this.StopUpdate();
        }

        public void Display(string message)
        {
            _activatedTimeBalance = _activatedTime;
            _text.text = message;
            gameObject.SetActive(true);
            this.StartUpdate();
        }
        
#if UNITY_EDITOR

        private void OnValidate()
        {
            if (_text == null) TryGetComponent(out _text);
        }
#endif
    }
}
