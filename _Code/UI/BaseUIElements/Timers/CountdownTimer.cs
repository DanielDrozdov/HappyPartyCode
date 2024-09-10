using System;
using TMPro;
using UnityEngine;
using UpdateSys;

namespace UI.BaseElements.Timers
{
    public class CountdownTimer : MonoBehaviour, IUpdatable, ICountdownTimer
    {
        [SerializeField] 
        private TMP_Text _timer;

        private float _remainderTime;
        private float _lastSavedNumber;
        private bool _isActivated;
        private Action _onCountdownEndAction;

        public bool IsActivated => _isActivated;

        private void OnDestroy()
        {
            this.StopUpdate();
        }

        public void OnSystemUpdate(float deltaTime)
        {
            _remainderTime -= deltaTime;

            UpdateTimerText();
            
            if (_remainderTime <= 0)
            {
                _onCountdownEndAction?.Invoke();
                StopCountdownTimer();
            }
        }

        public virtual void StartCountdown(float time, Action onCountdownEndAction)
        {
            _isActivated = true;
            _lastSavedNumber = -1;
            _remainderTime = time;
            _onCountdownEndAction = onCountdownEndAction;
            _timer.gameObject.SetActive(true);
            this.StartUpdate();
        }

        public void BreakCountdown()
        {
            if (!_isActivated) return;
            
            StopCountdownTimer();
        }

        protected virtual void StopCountdownTimer()
        {
            _timer.gameObject.SetActive(false);
            _isActivated = false;
            this.StopUpdate();   
        }

        private void UpdateTimerText()
        {
            float ceilNumber = Mathf.Ceil(_remainderTime);

            if (ceilNumber != _lastSavedNumber)
            {
                _lastSavedNumber = ceilNumber;
                _timer.text = ceilNumber.ToString();
            }
        }
    }
}
