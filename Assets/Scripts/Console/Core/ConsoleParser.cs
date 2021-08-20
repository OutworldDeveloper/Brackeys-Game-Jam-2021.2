using System;
using System.Collections.Generic;

public class ConsoleParser 
{

    private readonly CommandsContainer _commands;
    private readonly IEnumerable<IParameterConvertor> _parameterConvertors;
    private readonly Console.Settings _settings; // fix naming
    private readonly ParameterConvertors _convertors;

    public ConsoleParser(
        Console.Settings settings,
        CommandsContainer commands, 
        IEnumerable<IParameterConvertor> parameterConvertors,
        ParameterConvertors convertors)
    {
        _settings = settings;
        _commands = commands;
        _parameterConvertors = parameterConvertors;
        _convertors = convertors;
    }

    // TODO: Make < and > setable
    public bool TryParse(string input, out ConsoleInput consoleInput)
    {
        string alias = default;
        List<string> stringParameters = new List<string>();

        bool isAliasParsed = false;
        int brackeysDepth = 0;
        string currentParameter = default;

        foreach (var currentCharacter in input)
        {
            // Before the first <
            if (isAliasParsed == false)
            {
                if (char.IsWhiteSpace(currentCharacter))
                    continue;

                if (currentCharacter == _settings.ParametersOpen)
                {
                    isAliasParsed = true;
                }
                else
                {
                    alias += currentCharacter;
                }

                continue;
            }

            if (currentCharacter == _settings.ParametersOpen)
                brackeysDepth++;

            // If deeper than 0
            if (brackeysDepth > 0)
            {
                currentParameter += currentCharacter;

                if (currentCharacter == _settings.ParametersClose)
                    brackeysDepth--;

                continue;
            }

            // If deep is zero
            if (currentCharacter == _settings.ParametersClose)
            {
                stringParameters.Add(currentParameter);
                break;
            }

            if (currentCharacter == _settings.ParametersOpen)
            {
                brackeysDepth++;
                currentParameter += currentCharacter;
                continue;
            }

            if (currentCharacter == _settings.ParametersSplitter)
            {
                if (string.IsNullOrEmpty(currentParameter) == false)
                {
                    stringParameters.Add(currentParameter);
                    currentParameter = "";
                }
                continue;
            }

            currentParameter += currentCharacter;
            continue;
        }

        int parametersCount = stringParameters.Count;

        var matchingCommands = _commands.FindCommands(alias, parametersCount);
        var convertedParams = new object[parametersCount];

        foreach (var matchingCommand in matchingCommands)
        {
            bool hasFailed = false;

            for (int i = 0; i < parametersCount; i++)
            {
                var targetType = matchingCommand.Parameters[i].ParameterType;

                if (targetType == typeof(ConsoleInput))
                {
                    if (TryParse(stringParameters[i], out ConsoleInput convertedParameter) == false)
                    {
                        hasFailed = true;
                        break;
                    }

                    convertedParams[i] = convertedParameter;
                    //break;
                }
                else
                {
                    if (_convertors.TryFindConvertor(targetType, out IParameterConvertor convertor) == false)
                    {
                        hasFailed = true;
                        break;
                    }

                    if (convertor.TryConvert(stringParameters[i], out object convertedParameter) == false)
                    {
                        hasFailed = true;
                        break;
                    }

                    convertedParams[i] = convertedParameter;
                }
            }

            if (hasFailed)
                continue;

            consoleInput = new ConsoleInput(matchingCommand, convertedParams);
            return true;
        }

        consoleInput = null;
        return false;
    }

}