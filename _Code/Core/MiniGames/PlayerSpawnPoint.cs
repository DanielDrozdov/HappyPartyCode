using UnityEngine;

namespace Core.MiniGames
{
    public class PlayerSpawnPoint : MonoBehaviour
    {
        [SerializeField] 
        private Mesh _playerMesh;
        
        private void OnDrawGizmos()
        {
            Color color = Color.red;
            color.a = 0.15f;
            Gizmos.color = color;
            Gizmos.DrawWireMesh(_playerMesh, transform.position, transform.rotation, transform.localScale);
        }
    }
}
