using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
public class ConsoleCommandAttribute : Attribute
{

    public readonly string CustomAlias;
    public readonly bool AliasOverridden;
    public readonly string Description;

    public ConsoleCommandAttribute(string description)
    {
        Description = description;
    }

    public ConsoleCommandAttribute(string customAlias, string description) : this(description)
    {
        CustomAlias = customAlias;
        AliasOverridden = true;
    }

}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
public class NonBindableAttribute : Attribute { }