using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimescaleManager
{
    public bool IsGamePaused { get; private set; }
    public event Action OnGamePaused;
    public event Action<float> OnTimescaleChanged;
    public float Timescale { get; private set; } = 1.0f;

    private readonly HashSet<object> _pauseRequests = new HashSet<object>();

    public void Pause(object requestingObject)
    {
        _pauseRequests.Add(requestingObject);
        UpdateTimescale();
    }

    public void Unpause(object requestingObject)
    {
        _pauseRequests.Remove(requestingObject);
        UpdateTimescale();
    }

    public void SetTimescale(float timescale)
    {
        Timescale = timescale;
        UpdateTimescale();
    }

    private void UpdateTimescale()
    {
        bool previousState = IsGamePaused;

        IsGamePaused = _pauseRequests.Count > 0;
        Time.timeScale = IsGamePaused ? 0f : Timescale;

        if (previousState == false && IsGamePaused)
        {
            OnGamePaused?.Invoke();
        }

        OnTimescaleChanged?.Invoke(Timescale);
    }

}