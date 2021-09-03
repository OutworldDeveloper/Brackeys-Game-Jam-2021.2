using System;
using Zenject;

public class StatsCapture : IInitializable, IDisposable
{

    [Inject] private LuckGameplayBase _gameplay;
    [Inject] private SceneLoader _sceneLoder;
    [Inject] private RecordsManager _playerProfile;

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
        var stats = new Stats()
        {
            Win = win,
            GravesSaved = _gameplay.GravesSaved,
            RatsKilled = _gameplay.RatsKilled,
        };

        var gameplayScene = _sceneLoder.LoadedGameplayScene;
        _playerProfile.UpdateRecords(gameplayScene, stats);
    }

    // Shouldn't be here
    [Serializable]
    public struct Stats
    {
        public bool Win;
        public int GravesSaved;
        public int RatsKilled;

    }

}