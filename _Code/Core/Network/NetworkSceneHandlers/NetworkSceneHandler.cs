using System;
using Fusion;
using Sirenix.OdinInspector;
using UI.System;
using UnityEngine;
using Zenject;
using Odin = Sirenix.OdinInspector;

namespace Core.Network.SceneHandlers
{
    public class NetworkSceneHandler : NetworkBehaviour
    {
        [SerializeField, GUIColor(nameof(GetFollowsVariableColor))]
        [LabelText(SdfIconType.ChevronDoubleRight)]
        private bool _ifCameraFollowsPlayer;

        [SerializeField, FoldoutGroup("Components"), Odin.ReadOnly]
        private PlayersLevelLoadingStateChecker _playersLoadingStateChecker;
        
        [SerializeField, FoldoutGroup("Components"), Odin.ReadOnly]
        private SceneNetworkPlayersSpawner _playersSpawner;

        [SerializeField, FoldoutGroup("Components"), Odin.ReadOnly]
        private SceneCameraAssigner _cameraAssigner;

        private ISceneTransition _sceneTransition;
        protected event Action OnSceneFadeInTransitionCompleted;
        
        [Inject]
        private void Construct(ISceneTransition sceneTransition)
        {
            _sceneTransition = sceneTransition;
        }
        
        public override void Spawned()
        {
            if (Runner.IsServer)
            {
                _playersLoadingStateChecker.OnAllPlayersLoaded += RPC_OnAllPlayersSpawnedAndLoaded;
                SpawnAllNetworkPlayers();
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_OnAllPlayersSpawnedAndLoaded()
        {
            _sceneTransition.StartFadeInTransition(OnSceneFadeInTransitionCompleted);
            InitializeSceneComponents();
        }

        private void SpawnAllNetworkPlayers()
        {
            if (!Runner.IsServer) return;
            
            _playersSpawner.SpawnPlayers();
        }

        private void InitializeSceneComponents()
        {
            NetworkObject localPlayer = FindLocalPlayer();
            
            if (_ifCameraFollowsPlayer)
            {
                _cameraAssigner.FollowAndLookAt(localPlayer.gameObject);
            }
        }

        private NetworkObject FindLocalPlayer()
        {
            NetworkObject[] networkObjects = FindObjectsOfType<NetworkObject>();

            foreach (NetworkObject networkObject in networkObjects)
            {
                if (networkObject.HasInputAuthority)
                {
                    return networkObject;
                }
            }

            return null;
        }

        private Color GetFollowsVariableColor() => _ifCameraFollowsPlayer ? Color.green : Color.red;
        
#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (_playersLoadingStateChecker == null) TryGetComponent(out _playersLoadingStateChecker);
            
            if (_playersSpawner == null) _playersSpawner = GetComponentInChildren<SceneNetworkPlayersSpawner>();

            if (_cameraAssigner == null) _cameraAssigner = GetComponentInChildren<SceneCameraAssigner>();
        }
#endif
    }
}
