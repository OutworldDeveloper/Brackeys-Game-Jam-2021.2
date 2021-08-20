using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using Zenject;

public class Console : MonoBehaviour, IConsole
{

    [Inject] private Logger _logger;
    [Inject] private ConsoleRegistry _regestry;
    [Inject] private CommandsContainer _commandsContainer;
    [Inject] private ConsoleParser _parser;
    [Inject] private ConsoleProcessor _processor;
    [Inject] private ConsoleColors _colors;
    [Inject] private ConsoleWindow.Factory _consoleWindowFactory;

    private ConsoleWindow _currentWindow;

    private void Start()
    {
        Application.logMessageReceived += Application_LogMessageReceived;
        // FOR NOW
        RegisterObject(this);
    }

    [ConsoleCommand("Toggles console window")]
    public void ToggleConsole()
    {
        if (_currentWindow != null)
        {
            _currentWindow.Close();
            return;
        }
        _currentWindow = _consoleWindowFactory.Create();
    }

    public void Log(object message, LogType logType = LogType.Message)
    {
        _logger.Log(message, logType);
    }

    public void RegisterObject(object target)
    {
        _regestry.RegisterObject(target);
    }

    public void DeregisterObject(object target)
    {
        _regestry.DeregisterObject(target);
    }

    public void AddCommand(ConsoleCommand command)
    {
        _commandsContainer.AddCommand(command);
    }

    public void Submit(string input)
    {
        Log($"> <color=#{_colors.UserInputHEX}>{input}</color>"); // Maybe use color if succeded
        if (_parser.TryParse(input, out ConsoleInput parsedInput))
        {
            _processor.TryProcess(parsedInput);
        }
        else
        {
            Log($"Couldn't parse '{input}'", LogType.Error);
        }
    }

    private void Application_LogMessageReceived(string condition, string stackTrace, UnityEngine.LogType type)
    {
        var logType = LogType.Message;

        switch (type)
        {
            case UnityEngine.LogType.Error:
                logType = LogType.Error;
                break;
            case UnityEngine.LogType.Assert:
                logType = LogType.Warning;
                break;
            case UnityEngine.LogType.Warning:
                logType = LogType.Warning;
                break;
            case UnityEngine.LogType.Log:
                logType = LogType.Message;
                break;
            case UnityEngine.LogType.Exception:
                logType = LogType.Error;
                break;
        }

        Log(condition, logType);
    }

    [Serializable]
    public class Settings
    {
        public char ParametersOpen;
        public char ParametersClose;
        public char ParametersSplitter;
    }

}