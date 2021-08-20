using System;

// I believe it is possible to make a unique ID for a command by taking its Alias, ParametersType and ParametersCount
public class ConsoleCommand
{

    public readonly string Alias;
    public readonly string Description;
    public readonly bool IsBindable;
    public readonly Type TargetType;
    public readonly CommandParameterInfo[] Parameters;
 
    private readonly Func<object, object[], object> _function;

    public ConsoleCommand( 
        string alias,
        string description,
        bool isBindable,
        Type targetType, 
        CommandParameterInfo[] parameters, 
        Func<object, object[], object> function)
    {
        Alias = alias;
        Description = description;
        IsBindable = isBindable;
        TargetType = targetType;
        Parameters = parameters;
        _function = function;
    }

    public object Execute(object target, object[] parameters)
    {
        return _function.Invoke(target, parameters);
    }

}

public class CommandParameterInfo
{

    public static readonly CommandParameterInfo[] EmptyParameters = new CommandParameterInfo[0];

    public readonly string Name;
    public readonly Type ParameterType;

    public CommandParameterInfo(string name, Type parameterType)
    {
        Name = name;
        ParameterType = parameterType;
    }

}