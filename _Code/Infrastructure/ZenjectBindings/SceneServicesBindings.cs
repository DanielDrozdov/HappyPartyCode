using Sirenix.OdinInspector;
using UI;
using UnityEngine;
using Utility;
using Zenject;

namespace Infrastructure.ZenjectBindings
{
    public class SceneServicesBindings : MonoInstaller
    {
        [SerializeField, ReadOnly] 
        protected SceneUICanvas _sceneUI;
        
        public override void InstallBindings()
        {
            InjectSceneDiContainerInDependenciesInjector();
            InjectDependenciesInSceneUI();
        }

        private void InjectSceneDiContainerInDependenciesInjector()
        {
            IDependenciesInjector dependenciesInjector = Container.Resolve<IDependenciesInjector>();
            Container.Inject(dependenciesInjector);
        }

        private void InjectDependenciesInSceneUI()
        {
            Container.InjectGameObject(_sceneUI.gameObject);
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (_sceneUI == null) _sceneUI = FindObjectOfType<SceneUICanvas>();
        }
#endif
    }
}