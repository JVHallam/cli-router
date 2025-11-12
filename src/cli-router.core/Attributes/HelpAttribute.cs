using System;

namespace CliRouter.Core.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class HelpAttribute : Attribute
{
    public string HelpText { get; }

    public HelpAttribute(string helpText)
    {
        HelpText = helpText;
    }
}
