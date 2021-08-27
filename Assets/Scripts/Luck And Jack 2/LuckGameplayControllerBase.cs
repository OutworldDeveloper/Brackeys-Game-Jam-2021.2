using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityRandom = UnityEngine.Random;

public class LuckGameplayControllerBase : GameplayController, ITickable, IDisposable
{

    private const float RatDetectionRange = 15f;

    public event QuestUpdatedEventHandler QuestUpdated;

    public int GravesSaved { get; private set; }
    public int RatsKilled { get; private set; }
    public bool HasSeenRat { get; private set; }
    public IReadOnlyList<Rat> RatsAlive => _ratsAlive;

    protected readonly PlayerPawn PlayerPawn;
    protected readonly Luck Luck;
    protected readonly Jack Jack;
    protected readonly RatsSpawnPoint[] RatsSpawnPoints;
    protected readonly Grave[] Graves;
    protected readonly Rat.Factory RatFactory;

    private readonly AnimationCurve _spawnCurve;
    private readonly List<Rat> _ratsAlive = new List<Rat>();
    private bool _isGameOver;

    public LuckGameplayControllerBase(
        IConsole console, 
        PlayerController playerController,
        PlayerPawn playerPawn,
        Luck luck,
        Jack jack,
        RatsSpawnPoint[] ratsSpawnPoints,
        Grave[] graves,
        Rat.Factory ratFactory,
        AnimationCurve spawnCurve) : base(console, playerController)
    {
        PlayerPawn = playerPawn;
        Luck = luck;
        Jack = jack;
        RatsSpawnPoints = ratsSpawnPoints;
        Graves = graves;
        RatFactory = ratFactory;
        _spawnCurve = spawnCurve;
    }

    public void Tick()
    {
        if (HasSeenRat == false)
        {
            foreach (var rat in _ratsAlive)
            {
                if (FlatVector.Distance((FlatVector)Luck.transform.position, (FlatVector)rat.transform.position) < RatDetectionRange ||
                    FlatVector.Distance((FlatVector)Jack.transform.position, (FlatVector)rat.transform.position) < RatDetectionRange)
                {
                    HasSeenRat = true;
                    OnFirstRatEncounter();
                }
            }
        }
    }

    public void Dispose()
    {
        Luck.Died -= OnLuckDied;
        Grave.GraveSaved -= OnGraveSaved;
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
        Luck.Died += OnLuckDied;
        Grave.GraveSaved += OnGraveSaved;
        PlayerController.Possess(PlayerPawn);
    }

    private void OnLuckDied(Actor sender)
    {
        if (_isGameOver == false)
        {
            _isGameOver = true;
            OnGameover();
        }
    }

    protected virtual void OnGraveSaved(Grave grave)
    {
        GravesSaved++;
    }

    protected virtual void OnGameover()
    {
        PlayerController.Unpossess();
    }

    protected virtual void OnFirstRatEncounter() { }

    protected virtual void OnRatDied(Actor sender)
    {
        sender.Died -= OnRatDied;
        _ratsAlive.Remove(sender as Rat);
        RatsKilled++;
        SpawnRats();
        if (HasSeenRat == false)
        {
            HasSeenRat = true;
            OnFirstRatEncounter();
        }
    }

    protected void SpawnRats()
    {
        int ratsMaxCount = Mathf.FloorToInt(_spawnCurve.Evaluate(GravesSaved));
        int toSpawn = ratsMaxCount - _ratsAlive.Count;

        for (int i = 0; i < toSpawn; i++)
        {
            var spawnPoint = RatsSpawnPoints[UnityRandom.Range(0, RatsSpawnPoints.Length)];
            var rat = RatFactory.Create();
            rat.transform.position = (FlatVector)spawnPoint.transform.position;
            rat.Died += OnRatDied;
            _ratsAlive.Add(rat);
        }
    }

    protected void UpdateQuest(string quest)
    {
        QuestUpdated?.Invoke(quest);
    }

    public delegate void QuestUpdatedEventHandler(string newQuest);

}