using System;
using System.Collections.Generic;
using System.Reflection;

public class ParametorConvertorsFinder 
{

    public IParameterConvertor[] FindParameterConvertors()
    {
        var convertorsList = new List<IParameterConvertor>();
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                if (type.IsAbstract)
                    continue;

                if (typeof(IParameterConvertor).IsAssignableFrom(type) == false)
                    continue;

                var constructor = type.GetConstructor(Type.EmptyTypes);

                if (constructor == null)
                    continue;

                var newConvertor = constructor.Invoke(Type.EmptyTypes) as IParameterConvertor;

                convertorsList.Add(newConvertor);
            }
        }

        return convertorsList.ToArray();
    }


}

/*
public interface IConsoleReflectionPass
{
    void ProcessType(Type type);

}
*/