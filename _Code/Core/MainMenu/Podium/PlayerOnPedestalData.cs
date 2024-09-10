using Fusion;

namespace Core.MainMenu.Podium
{
    public struct PlayerOnPedestalData
    {
        public NetworkObject Player { get; private set; }
        public int PedestalNumber { get; private set;}

        public PlayerOnPedestalData(NetworkObject player, int pedestalNumber)
        {
            Player = player;
            PedestalNumber = pedestalNumber;
        }
    }
}