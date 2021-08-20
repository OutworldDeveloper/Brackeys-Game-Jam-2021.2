using System;

[AttributeUsage(AttributeTargets.Class)]
public class ConsolePrefixAttribute : Attribute
{

    public readonly string CustomPrefix;
    public readonly bool PrefixOverridden;

    public ConsolePrefixAttribute()
    {
        PrefixOverridden = false;
    }

    public ConsolePrefixAttribute(string customPrefix)
    {
        CustomPrefix = customPrefix;
        PrefixOverridden = true;
    }

}