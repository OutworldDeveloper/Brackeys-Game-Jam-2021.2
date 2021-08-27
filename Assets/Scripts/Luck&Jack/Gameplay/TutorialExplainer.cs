using System;
using UnityEngine;
using Zenject;

public class TutorialExplainer : MonoBehaviour
{

    [Inject] private Luck _luck;
    [Inject] private TutorialWall _tutorialWall;
    [Inject] private LuckGameplayTutorial _gameplayTutorial;
    [Inject] private UI_HelperText _helperText;
    [Header("Keys")]
    [SerializeField] private Setting_KeyCode[] _luckMovement;
    [SerializeField] private Setting_KeyCode[] _jackMovement;
    [SerializeField] private KeyStringOverride[] _keyStringOverrides;
    [Header("Tutorial Wall")]
    [SerializeField] private float _messageDistance = 3.4f;
    [SerializeField] private float _messageDuration = 0.5f;

    private bool _jackSaved;

    private void Start()
    {
        _gameplayTutorial.JackSaved += OnJackSaved;

        var toShow = GenerateMovementTip(_luckMovement, "Luck");
        _helperText.Show(toShow, 10f);
    }

    private void Update()
    {
        if (_jackSaved)
            return;

        var distance = FlatVector.Distance((FlatVector)_luck.transform.position, (FlatVector)_tutorialWall.transform.position);
        if (distance < _messageDistance)
        {
            _helperText.Show("You need to wake up Jack first.", _messageDuration);
        }
    }

    private void OnDestroy()
    {
        _gameplayTutorial.JackSaved -= OnJackSaved;
    }

    private void OnJackSaved()
    {
        _gameplayTutorial.JackSaved -= OnJackSaved;
        _jackSaved = true;
        var toShow = GenerateMovementTip(_jackMovement, "Jack");
        _helperText.Show(toShow, 15f);
    }

    private string GenerateMovementTip(Setting_KeyCode[] movement, string character)
    {
        var result = "Use ";

        foreach (var setting in movement)
        {
            var keyCode = setting.GetValue();
            var keyCodeString = keyCode.ToString();

            foreach (var stringOverride in _keyStringOverrides)
            {
                if (stringOverride.KeyCode == keyCode)
                {
                    keyCodeString = stringOverride.Override;
                    break;
                }
            }

            result += $"{keyCodeString} ";
        }

        result += $"to move {character}";

        return result;
    }

    [Serializable]
    private struct KeyStringOverride
    {
        public KeyCode KeyCode;
        public string Override;
    }

}