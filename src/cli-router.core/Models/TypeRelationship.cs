namespace CliRouter.Core.Models;

public record TypeRelationship
{
    public required Type Parent { get; init; }
    public required Type Child { get; init; }
}
