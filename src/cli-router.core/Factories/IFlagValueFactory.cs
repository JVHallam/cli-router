namespace CliRouter.Core.Factories;

public interface IFlagValueFactory
{
    List<FlagValue> Create(string[] args);
}
