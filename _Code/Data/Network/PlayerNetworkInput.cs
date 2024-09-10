using Fusion;
using UnityEngine;

namespace Data.Network
{
    public struct PlayerNetworkInput : INetworkInput
    {
        public Vector2 MovementVector;
        public NetworkBool IsJumpPressed;
    }
}
