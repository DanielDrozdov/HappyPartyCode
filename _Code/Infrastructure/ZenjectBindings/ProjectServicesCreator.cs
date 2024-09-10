using Data.Save;
using Zenject;

namespace Infrastructure.Services
{
    public class ProjectServicesCreator : MonoInstaller
    {
        private AudioSwitcher _audioSwitcher;
        private VibroSwitcher _vibroSwitcher;

        public override void InstallBindings()
        {
            CreateAudioSwitcher();
            CreateVibroSwitcher();
        }

        private void CreateAudioSwitcher()
        {
            _audioSwitcher = new AudioSwitcher();
        }
        
        private void CreateVibroSwitcher()
        {
            _vibroSwitcher = new VibroSwitcher();
        }
    }
}