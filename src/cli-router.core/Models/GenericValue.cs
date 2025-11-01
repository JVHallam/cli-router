using System.Reflection;

namespace CliRouter.Core.Models;

public record GenericValue(
    Type Type,
    string Value,
    bool IsFlag
)
{
    public PropertyInfo? PropertyInfo { get; set; }
};
