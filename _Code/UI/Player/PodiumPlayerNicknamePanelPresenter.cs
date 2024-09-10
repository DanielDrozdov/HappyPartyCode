using Data.Save;
using Fusion;
using Zenject;

namespace UI.Player
{
    public class PodiumPlayerNicknamePanelPresenter : BasePlayerNicknamePanelPresenter
    {
        [Networked, OnChangedRender(nameof(OnNicknameChanged))]
        private string _nickname { get; set; }
        
        private IGameDataAccess _gameDataAccess;
        private IPlayersNicknamesStorageSetter _playersNicknamesStorageSetter;

        [Inject]
        private void Construct(IGameDataAccess gameDataAccess, IPlayersNicknamesStorageSetter playersNicknamesStorageSetter)
        {
            _gameDataAccess = gameDataAccess;
            _playersNicknamesStorageSetter = playersNicknamesStorageSetter;
        }

        public override void Spawned()
        {
            if (Object.HasInputAuthority)
            {
                RPC_SetPlayerNicknameToServer(_gameDataAccess.GameSaveData.Nickname);
            }
            else if(!string.IsNullOrEmpty(_nickname))
            {
                UpdateNickname();
            }
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private void RPC_SetPlayerNicknameToServer(string nickname)
        {
            _nickname = nickname;
        }

        private void OnNicknameChanged()
        {
            UpdateNickname();
        }

        private void UpdateNickname()
        {
            _view.UpdateNickname(_nickname);
            _playersNicknamesStorageSetter.SetPlayerNickname(Object.InputAuthority, _nickname);
        }
    }
}