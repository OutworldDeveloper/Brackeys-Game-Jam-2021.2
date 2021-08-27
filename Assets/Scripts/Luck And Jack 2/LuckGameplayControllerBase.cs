using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckGameplayControllerBase : GameplayController
{

    public event QuestUpdatedEventHandler QuestUpdated;

    public int GravesSaved { get; private set; }
    public bool HasSeenRat { get; protected set; }

    protected readonly PlayerPawn PlayerPawn;
    protected readonly RatsSpawnPoint[] RatsSpawnPoints;
    protected readonly Rat.Factory RatFactory;

    public LuckGameplayControllerBase(
        IConsole console, 
        PlayerController playerController,
        PlayerPawn playerPawn,
        RatsSpawnPoint[] ratsSpawnPoints,
        Rat.Factory ratFactory) : base(console, playerController)
    {
        PlayerPawn = playerPawn;
        RatsSpawnPoints = ratsSpawnPoints;
        RatFactory = ratFactory;
    }

    public RatsSpawnPoint GetClosestRatsSpawnPoint(FlatVector location)
    {
        RatsSpawnPoint closest = null;
        float distanceToClosest = 0f;

        foreach (var spawnPoint in RatsSpawnPoints)
        {
            if (closest == null)
            {
                closest = spawnPoint;
                distanceToClosest = FlatVector.Distance(location, (FlatVector)closest.transform.position);
                continue;
            }

            float anotherDistance = FlatVector.Distance(location, (FlatVector)spawnPoint.transform.position);
            if (distanceToClosest > anotherDistance)
            {
                distanceToClosest = anotherDistance;
                closest = spawnPoint;
            }
        }

        return closest;
    }

    protected override void GameplayStarted()
    {
        PlayerController.Possess(PlayerPawn);
        RatFactory.Create();
    }

    public delegate void QuestUpdatedEventHandler(string newQuest);

}