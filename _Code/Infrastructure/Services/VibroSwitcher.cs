using Data.Save;
using UniRx;

namespace Infrastructure.Services
{
    public class VibroSwitcher
    {
        private CompositeDisposable _compositeDisposable = new ();
        
        public VibroSwitcher()
        {
            
        }
        
        private void UpdateVibroState(bool vibroDisabled) => Taptic.tapticOn = !vibroDisabled;
    }
}