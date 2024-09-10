using System.Threading.Tasks;
using Core.Storages;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utility.Factories;
using Zenject;

namespace Core.Player.SkinChangers
{
    public class PlayerBaseSkinChanger : NetworkBehaviour
    {
        [SerializeField, ReadOnly] 
        private Animator _animator;
        
        protected IPlayersSkinsStorage _playersSkinsStorage;
        private IObjectsFactory _objectsFactory;
        private bool _isChangingSkin;

        [Inject]
        private void Construct(IPlayersSkinsStorage playersSkinsStorage, IObjectsFactory objectsFactory)
        {
            _playersSkinsStorage = playersSkinsStorage;
            _objectsFactory = objectsFactory;
        }

        protected async Task UpdateSkin(byte id)
        {
            if (_isChangingSkin) return;
            
            _isChangingSkin = true;
            DestroyLastSkin();
            SaveCurrentAnimatorState(out AnimatorStateInfo savedAnimatorStateInfo);
            var modelSkinTrm = await SpawnNewModelSkin(id);
            ChangeModelComponentsParent(modelSkinTrm);
            RestoreAnimatorState(savedAnimatorStateInfo);
            _isChangingSkin = false;
        }

        private void DestroyLastSkin()
        {
            if (transform.childCount == 0) return;
            
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Transform component = transform.GetChild(i);
                Destroy(component.gameObject);
            }
        }

        private async Task<Transform> SpawnNewModelSkin(byte id)
        {
            AssetReference skinRef = _playersSkinsStorage.GetSkinReferenceById(id);
            GameObject modelSkin = await _objectsFactory.Create(skinRef, new Vector3(0,1000,0));
            Transform modelSkinTrm = modelSkin.transform;
            modelSkinTrm.parent = transform;
            modelSkinTrm.localPosition = Vector3.zero;
            modelSkinTrm.localRotation = Quaternion.identity;
            return modelSkinTrm;
        }

        private void ChangeModelComponentsParent(Transform model)
        {
            for (int i = 0; i < model.childCount;)
            {
                Transform component = model.GetChild(i);
                component.gameObject.SetActive(true);
                component.parent = transform;
                component.localScale = Vector3.one;
            }
        }

        private void SaveCurrentAnimatorState(out AnimatorStateInfo animatorStateInfo)
        {
            animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        }

        private void RestoreAnimatorState(AnimatorStateInfo savedAnimatorStateInfo)
        {
            _animator.Rebind();
            _animator.Play(savedAnimatorStateInfo.shortNameHash, 0 , savedAnimatorStateInfo.normalizedTime);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_animator == null) TryGetComponent(out _animator);
        }
#endif
    }
}