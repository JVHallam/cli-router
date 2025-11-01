namespace CliRouter.Core.Models;

public record GenericValue(
    Type Type,
    string Value,
    bool IsFlag
);
