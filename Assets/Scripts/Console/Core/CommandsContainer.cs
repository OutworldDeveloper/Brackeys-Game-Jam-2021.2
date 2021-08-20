using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using Zenject;

public class CommandsContainer : IInitializable
{

    private readonly IConsole _console;
    private readonly CommandDescriptionsGenerator _descriptionGenerator;
    private List<ConsoleCommand> _commands;

    public CommandsContainer(IConsole console, CommandDescriptionsGenerator descriptionGenerator)
    {
        _console = console;
        _descriptionGenerator = descriptionGenerator;
    }

    public void Initialize()
    {
        _console.RegisterObject(this);
        var stopwatch = new Stopwatch();
        var commandsFinder = new ConsoleCommandsFinder();
        stopwatch.Start();
        _commands = commandsFinder.FindCommands();
        stopwatch.Stop();
        _console.Log($"[CommandsContainer] Found {_commands.Count} commands ({stopwatch.ElapsedMilliseconds}ms)");
    }

    public void AddCommand(ConsoleCommand command)
    {
        _commands.Add(command);
    }

    public ConsoleCommand[] FindCommands(string alias, int parametersCount)
    {
        var result = new List<ConsoleCommand>();

        foreach (var command in _commands)
        {
            if (alias != command.Alias)
                continue;

            var currentCommandParameters = command.Parameters;

            if (currentCommandParameters.Length != parametersCount)
                continue;

            result.Add(command);
        }

        return result.ToArray();
    }

    public ConsoleCommand[] FindCommandsWhoseAliasContains(string alias) // Maybe it also needs return type
    {
        var result = new List<ConsoleCommand>();

        foreach (var command in _commands)
        {
            if (command.Alias.Contains(alias) == false)
                continue;

            result.Add(command);
        }

        return result.ToArray();
    }

    [ConsoleCommand("Prints avaliable commands")]
    public void Help()
    {
        foreach (var command in _commands)
        {
            _console.Log(_descriptionGenerator.GenerateDescription(command));
        }
    }

}