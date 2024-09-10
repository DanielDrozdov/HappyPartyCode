using System.Threading.Tasks;
using Fusion;

namespace Infrastructure.Network
{
    public interface INetworkConnector
    {
        Task<StartGameResult> ConnectToRoom(string roomName = null);
        Task<StartGameResult> CreateRoom(string roomName);
        bool CanCreateNewConnection();
    }
}