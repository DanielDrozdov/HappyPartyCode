using Fusion;

namespace Core.Player
{
    public class PlayerDespawner : NetworkBehaviour, IPlayerLeft
    {
        public void PlayerLeft(PlayerRef leftPlayerRef)
        {
            if (Runner.IsServer && leftPlayerRef == Object.InputAuthority)
            {
                Runner.Despawn(Object);
            }
        }
    }
}