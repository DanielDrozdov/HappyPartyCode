using System;

namespace UI.System
{
    public interface ISceneTransition
    {
        void StartFadeInTransition(Action onTransitionCompleted = null);
        void StartFadeOutTransition(Action onTransitionCompleted = null);
    }
}