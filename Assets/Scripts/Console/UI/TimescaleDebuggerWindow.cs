using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TimescaleDebuggerWindow : BaseConsoleWindow<TimescaleDebuggerWindow>
{

    [SerializeField] private UI_TimescalePresetButton _prefabButton;
    [SerializeField] private Transform _parent;
    [SerializeField] private Slider _slider;
    [SerializeField] private Text _text;
    [Inject] private TimescaleDebugger _timescaleDebugger;
    [Inject] private TimescaleManager _timescaleManager;

    protected override string WindowDisplayName => "Timescale Debugger";
    protected override string WindowId => "TimescaleDebugger";

    protected override void Start()
    {
        base.Start();

        _slider.value = _timescaleManager.Timescale;
        _text.text = _timescaleManager.Timescale.ToString();
        _timescaleManager.OnTimescaleChanged += OnTimescaleChanged;
        _slider.onValueChanged.AddListener((value) => _timescaleDebugger.SetTimescale(value));

        foreach (var preset in _timescaleDebugger.Presets)
        {
            var presetButton = Instantiate(_prefabButton, _parent);
            presetButton.Setup(preset, (x) => _timescaleDebugger.SetTimescale(x));
        }
    }

    private void OnDisable()
    {
        _timescaleManager.OnTimescaleChanged -= OnTimescaleChanged;
    }

    private void OnTimescaleChanged(float value)
    {
        _slider.value = value;
        _text.text = value.ToString();
    }

    public class Factory : PlaceholderFactory<TimescaleDebugger, TimescaleDebuggerWindow> { }

}