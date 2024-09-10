namespace Infrastructure.Services
{
    public interface IActiveSceneComparer
    {
        bool IsCurrentSceneLobby();
        bool IsCurrentSceneMainMenu();
    }
}