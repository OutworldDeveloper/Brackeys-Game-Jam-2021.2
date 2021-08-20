using System;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class Logger : IInitializable
{

    public event EventHandler<ConsoleLog> LogEvent; // FIXME: Naming
    public event Action ClearedEvent; // FIXME: Naming
    public readonly int HistoryCapacity;
    public readonly List<ConsoleLog> History; // Should not be a string list

    private readonly IConsole _console;

    public Logger(IConsole console, int historyCapacity)
    {
        _console = console;
        HistoryCapacity = historyCapacity;
        History = new List<ConsoleLog>(historyCapacity);
    }

    public void Initialize()
    {
        _console.RegisterObject(this);
    }

    public void Log(object message, LogType logType = LogType.Message)
    {
        if (History.Count + 1 > HistoryCapacity)
        {
            History.RemoveAt(0);
        }

        var newLog = new ConsoleLog(message.ToString(), logType);

        History.Add(newLog);
        LogEvent?.Invoke(this, newLog);
    }

    [ConsoleCommand("Placeholder description")]
    public void Clear()
    {
        History.Clear();
        ClearedEvent?.Invoke();
    }

    [ConsoleCommand("Fuck you")]
    public void Say(string message)
    {
        Log(message, LogType.Message);
    }

}