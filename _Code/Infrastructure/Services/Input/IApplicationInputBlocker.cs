namespace Infrastructure.Services.Input
{
    public interface IApplicationInputBlocker
    {
        void BlockUserInput();
        void UnblockUserInput();
    }
}