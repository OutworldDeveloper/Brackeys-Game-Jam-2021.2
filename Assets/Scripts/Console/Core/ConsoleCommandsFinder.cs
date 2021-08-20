using System;
using System.Collections.Generic;
using System.Reflection;

public class ConsoleCommandsFinder
{

    private readonly IEnumerable<ICommandsCreationStrategy> _commandsCreationStrategies = new List<ICommandsCreationStrategy>()
    {
        new FromMethodStrategy(),
        new FromPropertyStrategy(),
        new FromFieldStrategy(),
    };

    public List<ConsoleCommand> FindCommands()
    {
        var commandsList = new List<ConsoleCommand>();
        //var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        var assemblies = new Assembly[]
        {
            Assembly.GetExecutingAssembly()
        };

        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                if (type.IsNested)
                    continue;

                GetCommandsInType(type, string.Empty, commandsList);
            }
        }

        return commandsList;
    }

    private void GetCommandsInType(Type type, string prefix, List<ConsoleCommand> commands)
    {
        var prefixAttribute = type.GetCustomAttribute<ConsolePrefixAttribute>();

        var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        var members = type.GetMembers(flags);

        if (prefixAttribute != null)
        {
            if (string.IsNullOrEmpty(prefix) == false)
                prefix += ".";
            if (prefixAttribute.PrefixOverridden)
                prefix += FixString(prefixAttribute.CustomPrefix);
            else
                prefix += FixString(type.Name);
        }

        foreach (var member in members)
        {
            if (member.DeclaringType != type)
                continue;

            var commandAttribute = member.GetCustomAttribute<ConsoleCommandAttribute>();

            if (commandAttribute == null)
                continue;

            bool isBindable = member.GetCustomAttribute<NonBindableAttribute>() == null;

            string alias = FixString(member.Name);

            if (commandAttribute.AliasOverridden)
            {
                alias = FixString(commandAttribute.CustomAlias);
            }

            if (string.IsNullOrEmpty(prefix) == false && member.GetCustomAttribute<IgnorePrefixAttribute>() == null)
                alias = $"{prefix}.{alias}";

            var commandInfo = new CommandInfo()
            {
                Alias = alias,
                Description = commandAttribute.Description,
                IsBindable = isBindable,
            };
            
            foreach (var strategy in _commandsCreationStrategies)
            {
                if (strategy.TargetMemberType == member.MemberType)
                {
                    strategy.CreateCommands(member, commandInfo, commands);
                    break;
                }
            }
            
        }

        var nestedTypes = type.GetNestedTypes();
        foreach (var nestedType in nestedTypes)
        {
            GetCommandsInType(nestedType, prefix, commands);
        }
    }

    private string FixString(string targetString)
    {
        string result = default;

        for (int i = 0; i < targetString.Length; i++)
        {
            char character = targetString[i];

            if (char.IsWhiteSpace(character))
            {
                result += '-';
                continue;
            }

            if (char.IsUpper(character))
            {
                if (i > 0)
                    result += '-';
                result += char.ToLower(character);
                continue;
            }

            result += character;
        }

        return result;
    }

}

public interface ICommandsCreationStrategy
{
    MemberTypes TargetMemberType { get; }
    void CreateCommands(MemberInfo member, CommandInfo info, List<ConsoleCommand> commands);

}

public class FromMethodStrategy : ICommandsCreationStrategy
{
    public MemberTypes TargetMemberType => MemberTypes.Method;

    public void CreateCommands(MemberInfo member, CommandInfo info, List<ConsoleCommand> commands)
    {
        var method = member as MethodInfo;
        var methodParameters = method.GetParameters();
        var commandParameters = new CommandParameterInfo[methodParameters.Length];

        for (int i = 0; i < methodParameters.Length; i++)
        {
            commandParameters[i] = new CommandParameterInfo(methodParameters[i].Name, methodParameters[i].ParameterType);
        }

        var command = new ConsoleCommand(
            info.Alias,
            info.Description,
            info.IsBindable,
            method.DeclaringType,
            commandParameters,
            (target, parameters) => method.Invoke(target, parameters));

        commands.Add(command);
    }

}

public class FromPropertyStrategy : ICommandsCreationStrategy
{
    public MemberTypes TargetMemberType => MemberTypes.Property;

    public void CreateCommands(MemberInfo member, CommandInfo info, List<ConsoleCommand> commands)
    {
        var property = member as PropertyInfo;

        var getCommanad = new ConsoleCommand(
            $"{info.Alias}.get",
            info.Description,
            info.IsBindable,
            property.DeclaringType,
            CommandParameterInfo.EmptyParameters,
            (target, parameters) => property.GetValue(target));

        var setCommand = new ConsoleCommand(
            $"{info.Alias}.set",
            info.Description,
            info.IsBindable,
            property.DeclaringType,
            new CommandParameterInfo[] { new CommandParameterInfo(property.Name, property.PropertyType) },
            (target, parameters) => { property.SetValue(target, parameters[0]); return null; });

        commands.Add(getCommanad);
        commands.Add(setCommand);
    }

}

public class FromFieldStrategy : ICommandsCreationStrategy
{
    public MemberTypes TargetMemberType => MemberTypes.Field;

    public void CreateCommands(MemberInfo member, CommandInfo info, List<ConsoleCommand> commands)
    {
        var field = member as FieldInfo;

        var getCommand = new ConsoleCommand(
            $"{info.Alias}.get",
            info.Description,
            info.IsBindable,
            field.DeclaringType,
            CommandParameterInfo.EmptyParameters,
            (target, parameters) => field.GetValue(target));

        var setCommand = new ConsoleCommand(
            $"{info.Alias}.set",
            info.Description,
            info.IsBindable,
            field.DeclaringType,
            new CommandParameterInfo[] { new CommandParameterInfo(field.Name, field.FieldType) },
            (target, parameters) => { field.SetValue(target, parameters[0]); return null; });

        commands.Add(getCommand);
        commands.Add(setCommand);
    }

}

public struct CommandInfo
{
    public string Alias;
    public string Description;
    public bool IsBindable;

}