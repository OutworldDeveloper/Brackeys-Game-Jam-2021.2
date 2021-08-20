using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ConsoleInputField : MonoBehaviour
{

    [Inject] private Console.Settings _parsingSettings;
    [Inject] private IConsole _console; // should be just processor I believe
    [Inject] private CommandsContainer _commands;
    [Inject] private  CommandDescriptionsGenerator _descriptionGenerator;
    [SerializeField] private InputField _inputField;
    [SerializeField] private RectTransform _suggestionsParent; // T
    [SerializeField] private Text _prefabSuggestionText;

    private readonly List<Text> _currentSuggestionTexts = new List<Text>();

    private void Start()
    {
        _inputField.onValueChanged.AddListener(value => InputFiled_OnValueChanged(value));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            string input = _inputField.text;
            if (string.IsNullOrWhiteSpace(input))
                return;
            _console.Submit(input);
            _inputField.text = string.Empty;
        }
    }

    private void InputFiled_OnValueChanged(string value)
    {
        ClearSuggestions();

        if (string.IsNullOrEmpty(value))
            return;

        string commandAlias = default;
        bool aliasParsed = false;
        int parametersCount = 0;
        int currentDepth = 0;

        for (int i = 0; i < _inputField.caretPosition; i++)
        {
            var character = _inputField.text[i];

            if (char.IsWhiteSpace(character))
                continue;

            if (aliasParsed == false)
            {
                if (character == _parsingSettings.ParametersOpen)
                {
                    aliasParsed = true;
                    parametersCount++;
                    continue;
                }
                commandAlias += character;
            }

            if (character == _parsingSettings.ParametersOpen)
            {
                currentDepth++;
            }
            else if (character == _parsingSettings.ParametersClose)
            {
                currentDepth--;
            }
            else if (character == _parsingSettings.ParametersSplitter)
            {
                if (currentDepth == 0)
                    parametersCount++;
            }
        }

        var matchingCommands = _commands.FindCommandsWhoseAliasContains(commandAlias);

        foreach (var command in matchingCommands)
        {
            if (command.Parameters.Length >= parametersCount)
                AddSuggestion(command);
        }

        _suggestionsParent.gameObject.SetActive(true);
    }

    private void AddSuggestion(ConsoleCommand command)
    {
        var newSuggestion = GameObject.Instantiate(_prefabSuggestionText, _suggestionsParent);
        newSuggestion.text = _descriptionGenerator.GenerateDescription(command);
        _currentSuggestionTexts.Add(newSuggestion);
    }

    private void ClearSuggestions()
    {
        foreach (var suggestionText in _currentSuggestionTexts)
        {
            Destroy(suggestionText.gameObject);
        }
        _currentSuggestionTexts.Clear();
        _suggestionsParent.gameObject.SetActive(false);
    }

}