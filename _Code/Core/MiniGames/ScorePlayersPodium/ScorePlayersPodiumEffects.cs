using UnityEngine;

namespace Core.MainMenu.Podium
{
    public class ScorePlayersPodiumEffects : MonoBehaviour
    {
        [SerializeField] 
        private ParticleSystem[] _confettiEffects;

        public void PlayConfettiEffect()
        {
            for (int i = 0; i < _confettiEffects.Length; i++)
            {
                _confettiEffects[i].Play();
            }
        }
    }
}
