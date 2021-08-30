using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityRandom = UnityEngine.Random;

public abstract class LuckGameplayBase : GameplayController
{

    public event QuestUpdatedEventHandler QuestUpdated;

    [SerializeField] private float _ratDetectionRange = 15f;
    [SerializeField] private float _jackSleepingRange = 14f;
    [SerializeField] private float _jackWakingUpRange = 3.5f;
    [SerializeField] private AnimationCurve _spawnCurve;
    [Inject] protected PlayerPawn PlayerPawn { get; private set; }
    [Inject] protected Luck Luck { get; private set; }
    [Inject] protected Jack Jack { get; private set; }
    [Inject] protected CharactersSpawnPoint SpawnPoint { get; private set; }
    [Inject] protected RatsSpawnPoint[] RatsSpawnPoints { get; private set; }
    [Inject] protected Grave[] Graves { get; private set; }
    [Inject] protected Rat.Factory RatFactory { get; private set; }
    [Inject] protected Ghost.Factory GhostFactory { get; private set; }

    public int GravesSaved { get; private set; }
    public int RatsKilled { get; private set; }
    public bool HasSeenRat { get; private set; }
    public IReadOnlyList<Rat> RatsAlive => _ratsAlive;
    protected virtual bool IgnoreDistanceSleeping => false;

    private readonly List<Rat> _ratsAlive = new List<Rat>();
    private bool _isGameOver;

    protected override void Start()
    {
        base.Start();
        Luck.Died += OnLuckDied;
        Grave.GraveSaved += OnGraveSaved;
        Luck.transform.position = SpawnPoint.transform.position;
        Jack.transform.position = SpawnPoint.transform.position;
        PlayerController.Possess(PlayerPawn);
        UpdateQuest();
    }

    protected virtual void OnDestroy()
    {
        Luck.Died -= OnLuckDied;
        Grave.GraveSaved -= OnGraveSaved;
    }

    protected virtual void Update()
    {
        if (IgnoreDistanceSleeping == false)
        {
            var distance = FlatVector.Distance((FlatVector)Luck.transform.position, (FlatVector)Jack.transform.position);
            if (Jack.IsSleeping)
            {
                if (distance < _jackWakingUpRange)
                {
                    Jack.WakeUp();
                }
            }
            else
            {
                if (distance > _jackSleepingRange)
                {
                    Jack.Sleep();
                }
            }
        }

        if (HasSeenRat == false)
        {
            foreach (var rat in _ratsAlive)
            {
                if (FlatVector.Distance((FlatVector)Luck.transform.position, (FlatVector)rat.transform.position) < _ratDetectionRange ||
                    FlatVector.Distance((FlatVector)Jack.transform.position, (FlatVector)rat.transform.position) < _ratDetectionRange)
                {
                    HasSeenRat = true;
                    OnFirstRatEncounter();
                }
            }
        }
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
        SpawnRats();
        UpdateQuest();
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
        UpdateQuest();
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

    protected void SpawnGhost()
    {
        SpawnGhost(Ghost.DefaultSettings);
    }

    protected void SpawnGhost(Ghost.GhostSettings settings)
    {
        var spawnPoint = RatsSpawnPoints[UnityRandom.Range(0, RatsSpawnPoints.Length)];
        var ghost = GhostFactory.Create(settings);
        ghost.transform.position = (FlatVector)spawnPoint.transform.position;
    }

    protected void UpdateQuest()
    {
        var quest = GetQuestText();
        QuestUpdated?.Invoke(quest);
    }

    protected abstract string GetQuestText();

    public delegate void QuestUpdatedEventHandler(string newQuest);

}