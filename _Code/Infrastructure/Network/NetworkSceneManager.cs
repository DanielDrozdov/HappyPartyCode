using System.Collections;
using Core.Player;
using Fusion;
using UI.System;
using UnityEngine;
using Utility;
using Zenject;

namespace Infrastructure.Network
{
    public class NetworkSceneManager : NetworkSceneManagerDefault
    {
        private ISceneTransition _sceneTransition;

        [Inject]
        private void Construct(ISceneTransition sceneTransition)
        {
            _sceneTransition = sceneTransition;
        }

        protected override IEnumerator LoadSceneCoroutine(SceneRef sceneRef, NetworkLoadSceneParameters sceneParams)
        {
            bool isTransitionCompleted = false;
            _sceneTransition.StartFadeOutTransition(() =>
            {
                isTransitionCompleted = true;
                PreDespawnPlayersBeforeNewSceneLoading();
            });
            
            while (!isTransitionCompleted)
            {
                 yield return null;
            }

            yield return WaitForConstants.OneSecond;

            yield return base.LoadSceneCoroutine(sceneRef, sceneParams);
        }

        private void PreDespawnPlayersBeforeNewSceneLoading()
        {
            if (Runner.IsServer)
            {
                PlayerMovement[] players = FindObjectsOfType<PlayerMovement>();

                for (int i = 0; i < players.Length; i++)
                {
                    Runner.Despawn(players[i].GetComponent<NetworkObject>());
                }
            }
        }
    }
}
