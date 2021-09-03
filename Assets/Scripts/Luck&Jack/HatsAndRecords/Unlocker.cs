using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Unlocker : IInitializable, IDisposable
{

    [Inject] private LuckGameplayBase _gameplay;
    [Inject] private UnlockablesManager _unlockablesManager;

    public void Initialize()
    {
        _gameplay.PlayerWon += OnGameEnd;
        _gameplay.PlayerLost += OnGameEnd;
    }

    public void Dispose()
    {
        _gameplay.PlayerWon -= OnGameEnd;
        _gameplay.PlayerLost -= OnGameEnd;
    }

    private void OnGameEnd()
    {
        _unlockablesManager.TryUnlockAll();
    }

}