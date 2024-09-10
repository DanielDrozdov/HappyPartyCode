using Data.Save;
using UniRx;
using UnityEngine;

namespace Infrastructure.Services
{
    public class AudioSwitcher
    {
        private CompositeDisposable _compositeDisposable = new ();
        
        public AudioSwitcher()
        {
            
        }
        
        private void UpdateAudioState(bool audioDisabled) => AudioListener.volume = audioDisabled ? 0 : 1;
    }
}