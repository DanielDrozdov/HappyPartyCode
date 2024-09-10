using System;
using UnityEngine;

namespace UI.BaseElements.Timers
{
    public class ScoreResultsClosingCountdownTimer : CountdownTimer, IScoreResultsClosingCountdownTimer
    {
        [SerializeField] 
        private GameObject _timerBaseText;

        public override void StartCountdown(float time, Action onCountdownEndAction)
        {
            base.StartCountdown(time, onCountdownEndAction);
            _timerBaseText.SetActive(true);
        }

        protected override void StopCountdownTimer()
        {
            base.StopCountdownTimer();
            _timerBaseText.SetActive(false);
        }
    }
}