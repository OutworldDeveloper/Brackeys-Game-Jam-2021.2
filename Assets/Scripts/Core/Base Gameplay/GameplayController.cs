using Zenject;

public class GameplayController : IInitializable
{

    public readonly PlayerController PlayerController; // Maybe generic arguement instead

    private readonly IConsole _console;

    public GameplayController(IConsole console, PlayerController playerController)
    {
        _console = console;
        PlayerController = playerController;
    }

    public void Initialize()
    {
        _console.Log($"GameplayController of type {GetType()} is initialized.");
        GameplayStarted();
    }

    protected virtual void GameplayStarted() { }

}