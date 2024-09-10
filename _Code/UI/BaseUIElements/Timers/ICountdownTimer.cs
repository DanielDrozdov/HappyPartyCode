using System;

namespace UI.BaseElements.Timers
{
    public interface ICountdownTimer
    {
        void StartCountdown(float time, Action onCountdownEndAction);
        void BreakCountdown();
        bool IsActivated { get; }
    }
}