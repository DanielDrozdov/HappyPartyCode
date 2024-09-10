using Data.Network;

namespace Infrastructure.Services.Input
{
    public interface IPlatformInputProvider
    {
        PlayerNetworkInput GetInput();
    }
}