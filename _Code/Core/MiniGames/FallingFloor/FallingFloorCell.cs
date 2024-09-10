using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.MiniGames.FallingFloor
{
    public class FallingFloorCell : MonoBehaviour
    {
        [SerializeField] 
        private float _destructionTime;

        [SerializeField, ReadOnly] 
        private byte _id;

        [SerializeField, FoldoutGroup("Components")] 
        private Collider _rbCollider;
        
        [SerializeField, FoldoutGroup("Components")] 
        private Collider _staticCollider;

        [SerializeField, ReadOnly, FoldoutGroup("Auto-Components")] 
        private Rigidbody _rb;

        [SerializeField, ReadOnly, FoldoutGroup("Auto-Components")] 
        private FallingFloorCellAnimation _animation;

        [SerializeField, ReadOnly, FoldoutGroup("Auto-Components")]
        private FallingFloorCellTrigger _trigger;
        
        private readonly int _destructTorquePower = 1000;

        public byte Id => _id;

        private void Awake()
        {
            _trigger.OnEnteredLavaZone += () => gameObject.SetActive(false);
        }

        public void StartDestructionProcess()
        {
            _animation.StartShakeRotation(_destructionTime, Destruct);
        }

        private void Destruct()
        {
            _staticCollider.enabled = false;
            _rbCollider.enabled = true;
            _rb.isKinematic = false;
            _rb.AddTorque(Random.Range(-1f, 1f) * _destructTorquePower * Vector3.forward);
        }

#if UNITY_EDITOR
        public void SetId(int id)
        {
            _id = (byte)id;
        }

        public void SetSize(Vector3 size)
        {
            _staticCollider.transform.localScale = size;
            _rbCollider.transform.localScale = size;
        }

        private void OnValidate()
        {
            if (_rb == null) _rb = GetComponentInChildren<Rigidbody>();

            if (_animation == null) _animation = GetComponentInChildren<FallingFloorCellAnimation>();

            if (_trigger == null) _trigger = GetComponentInChildren<FallingFloorCellTrigger>();
        }
#endif
    }
}
