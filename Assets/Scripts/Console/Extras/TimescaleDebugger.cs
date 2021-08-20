using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TimescaleDebugger : ConsoleModule
{

    [Inject] private IConsole _console;
    [Inject] private TimescaleManager _manager;
    [Inject] private TimescaleDebuggerWindow.Factory _factory;
    [Inject] private ConsoleFilesManager _fileManager;

    public float[] Presets => _presets.ToArray();
    public List<float> _presets;
    private TimescaleDebuggerWindow _current;

    private void Start()
    {
        _console.RegisterObject(this);

        if (_fileManager.DataContainer.TryGetData("TimescaleDebuggerPresets", out List<float> presets))
        {
            _presets = presets;
            return;
        }
        _presets = new List<float>()
        {
            0f, 0.25f, 0.5f, 1f, 2f,
        };
    }

    private void OnDisable()
    {
        _fileManager.DataContainer.SetData("TimescaleDebuggerPresets", _presets);
    }

    [ConsoleCommand("Toggles timescale debugger window")]
    public void ToggleTimescaleDebugger()
    {
        if (_current != null)
        {
            _current.Close();
            return;
        }
        _current = _factory.Create(this);
    }

    [ConsoleCommand("Sets the current timescale")]
    public void SetTimescale(float value)
    {
        _manager.SetTimescale(value);
    }

    [ConsoleCommand("Adds a timescale preset to the timescale debugger window")]
    public void AddTimescalePreset(float value)
    {
        if (_presets.Count < 10)
        {
            _presets.Add(value);
            return;
        }
        _console.Log("Couldn't add new tiscale preset", LogType.Error);
    }

}