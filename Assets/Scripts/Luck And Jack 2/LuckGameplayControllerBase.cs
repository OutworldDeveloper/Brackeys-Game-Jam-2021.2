using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckGameplayControllerBase : GameplayController
{

    protected readonly PlayerPawn PlayerPawn;

    public LuckGameplayControllerBase(
        IConsole console, 
        PlayerController playerController,
        PlayerPawn playerPawn) : base(console, playerController)
    {
        PlayerPawn = playerPawn;
    }

    protected override void GameplayStarted()
    {
        PlayerController.Possess(PlayerPawn);
    }

}