using System;
using Zenject;

public class StatsSaver : IInitializable, IDisposable
{

    private readonly LuckGameplayBase _gameplay;
    private readonly SavingSystem _savingSystem;
    private readonly GameplayScene _gameplayScene;
    private readonly DataContainer _saveData;

    public StatsSaver(
        LuckGameplayBase gameplay, 
        SavingSystem savingSystem, 
        GameplayScene gameplayScene,
        DataContainer saveData)
    {
        _gameplay = gameplay;
        _savingSystem = savingSystem;
        _gameplayScene = gameplayScene;
        _saveData = saveData;
    }

    public void Initialize()
    {
        _gameplay.PlayerWon += OnPlayerWon;
        _gameplay.PlayerLost += OnPlayerLost;
    }

    public void Dispose()
    {
        _gameplay.PlayerWon -= OnPlayerWon;
        _gameplay.PlayerLost -= OnPlayerLost;
    }

    private void OnPlayerWon()
    {
        SaveStats(true);
    }

    private void OnPlayerLost()
    {
        SaveStats(false);
    }

    private void SaveStats(bool win)
    {
        var key = _gameplayScene.name;
        var stats = _saveData.GetData<Stats>(key);

        if (win)
        {
            stats.Win = true;
        }

        if (_gameplay.GravesSaved > stats.GravesSaved)
        {
            stats.GravesSaved = _gameplay.GravesSaved;
        }

        if (_gameplay.RatsKilled > stats.RatsKilled)
        {
            stats.RatsKilled = _gameplay.RatsKilled;
        }

        _saveData.SetData(key, stats);
        _savingSystem.SaveOverride(_saveData);
    }

    [Serializable]
    public struct Stats
    {
        public bool Win;
        public int GravesSaved;
        public int RatsKilled;

    }

}