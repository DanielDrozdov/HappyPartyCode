using Data.Network;

namespace Infrastructure.Services.Input
{
    public interface IApplicationInputProvider
    {
        PlayerNetworkInput GetPlayerInputForNetwork();
        bool IsInputBlocked();
    }
}