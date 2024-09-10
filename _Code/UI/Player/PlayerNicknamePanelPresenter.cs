using Fusion;
using UnityEngine;
using UpdateSys;
using Zenject;

namespace UI.Player
{
    public class PlayerNicknamePanelPresenter : BasePlayerNicknamePanelPresenter, IUpdatable
    {
        private Camera _mainCamera;
        private Transform _transform;
        private IPlayersNicknamesStorage _playersNicknamesStorage;

        [Inject]
        private void Construct(IPlayersNicknamesStorage playersNicknamesStorage)
        {
            _playersNicknamesStorage = playersNicknamesStorage;
        }

        private void Awake()
        {
            _mainCamera = Camera.main;
            _transform = transform;
        }

        public void OnSystemUpdate(float deltaTime)
        {
            RotateNickname();
        }

        public override void Spawned()
        {
            this.StartUpdate();
            SetNickname();
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            this.StopUpdate();
        }
        
        private void SetNickname()
        {
            string nickname = _playersNicknamesStorage.GetPlayerNickname(Object.InputAuthority);
            _view.UpdateNickname(nickname);
        }

        private void RotateNickname()
        {
            Vector3 selfPos = _transform.position;
            Vector3 direction = selfPos - _mainCamera.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction.normalized);
            _transform.rotation = rotation;
        }
    }
}