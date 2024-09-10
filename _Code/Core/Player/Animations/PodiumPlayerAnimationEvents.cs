using System;
using UnityEngine;

namespace Core.Player.Animations
{
    public class PodiumPlayerAnimationEvents : MonoBehaviour
    {
        public event Action OnEmotionReactionEnded; 
        
        private void OnEmotionReactionEnded_AnimationEvent()
        {
            OnEmotionReactionEnded?.Invoke();
        }
    }
}
