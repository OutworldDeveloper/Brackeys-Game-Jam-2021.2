using System;
using UnityEngine;
using Zenject;

public class PlayerController : ITickable, ILateTickable, IInitializable, IDisposable
{

    public readonly Camera PlayerCamera;
    public Pawn CurrentPawn { get; private set; }
    protected readonly InputReciver InputReciver = new InputReciver(true);

    private readonly IConsole _console;
    private readonly CursorManager _cursorManager;
    private readonly InputSystem _inputSystem;
    private readonly FreeCamera _freeCamera;
    private UI_BaseHud _currentHud;
    private bool _isFreeCamera;
    private Pawn _lastPawn; 

    public PlayerController(
        IConsole console,
        CursorManager cursorManager,
        Camera playerCamera,
        InputSystem inputSystem,
        FreeCamera freeCamera)
    {
        _console = console;
        _cursorManager = cursorManager;
        PlayerCamera = playerCamera;
        _inputSystem = inputSystem;
        _freeCamera = freeCamera;
    }

    public void Initialize()
    {
        _console.RegisterObject(this);
        _inputSystem.AddReciver(InputReciver);
        OnPlayerStart();
    }

    public void Tick()
    {
        CurrentPawn?.PossessedTick();
        OnPlayerTick();
    }

    public void LateTick()
    {
        if (CurrentPawn == null)
            return;
        PlayerCamera.transform.position = CurrentPawn.CameraPosition;
        PlayerCamera.transform.rotation = CurrentPawn.CameraRotation;
    }

    public void Dispose()
    {
        _console.DeregisterObject(this);
        _inputSystem.RemoveReciver(InputReciver);
        if (CurrentPawn != null) 
            Unpossess();
    }

    public void Possess(Pawn pawn) 
    {
        Unpossess();
        CurrentPawn = pawn;

        _inputSystem.AddReciverToTheBottomOfStack(CurrentPawn.InputReciver);

        if (CurrentPawn.ShowCursor)
        {
            _cursorManager.Show(this);
        }

        var hud = CurrentPawn.CreateHud();
        SetHud(hud);

        CurrentPawn.OnPossesesed(this);
    }

    public void Unpossess()
    {
        SetHud(null);

        if (CurrentPawn == null)
            return;

        _inputSystem.RemoveReciver(CurrentPawn.InputReciver);

        if (CurrentPawn.ShowCursor)
        {
            _cursorManager.Hide(this);
        }
        CurrentPawn.OnUnpossessed(this);
        CurrentPawn = null;
    }

    private void SetHud(UI_BaseHud hud)
    {
        if (_currentHud != null)
        {
            _inputSystem.RemoveReciver(_currentHud.InputReciver);
            _currentHud.HideThenDestroy();
        }

        if (hud != null)
        {
            _currentHud = hud;
            _inputSystem.AddReciver(_currentHud.InputReciver);
        }
    }

    [ConsoleCommand("Toggles free camera")]
    public void ToggleFreeCamera()
    {
        if (_isFreeCamera)
        {
            if (_lastPawn != null)
            {
                Possess(_lastPawn);
            }
            _isFreeCamera = false;
            return;
        }
        _lastPawn = CurrentPawn;
        Possess(_freeCamera);
        _isFreeCamera = true;
    }

    [ConsoleCommand("Toggles hud")]
    public void ToggleHud()
    {
        
    }

    protected virtual void OnPlayerStart() { }
    protected virtual void OnPlayerTick() { }

}