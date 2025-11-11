namespace CliRouter.Core.Factories;

public class FlagValueFactory : IFlagValueFactory
{
    //Assuming an array of flags in the structure
    //new string[]{ "--flag-one", "value", "--flag-two", "value" }
    public List<FlagValue> Create(string[] args)
    {
        var pairs = args.Zip(args.Skip(1), (a, b) => new PairedArgs(a, b));

        return pairs
            .Where(x => x.LeftArg.Contains("--"))
            .Select(x => new FlagValue(x.LeftArg.Replace("--", ""), x.RightArg))
            .ToList();
    }
}
