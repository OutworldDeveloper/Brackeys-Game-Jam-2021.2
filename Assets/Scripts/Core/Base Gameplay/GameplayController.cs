using Zenject;

public class GameplayController : IInitializable
{

    public readonly PlayerController PlayerController; // Maybe generic arguement instead

    public GameplayController(PlayerController playerController)
    {
        PlayerController = playerController;
    }

    public void Initialize()
    {
        //var hud = GetCurrentHUD();
        //if (hud != null)
        //{
            //PlayerController.SetHud(hud);
        //}

        //_console.Log($"GameplayController of type {GetType()} is initialized.");
        GameplayStarted();
    }

    protected virtual void GameplayStarted() { }

}