using System;

namespace CliRouter.Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class FlagAttribute : Attribute
{
    public string Name { get; }

    public FlagAttribute(string name)
    {
        Name = name;
    }
}
