using CliRouter.Core.Attributes;

namespace CliRouter.Tests.Integration.TestClasses;

public record FlagsModel(
    string Arg1,
    int Arg2
)
{
    [Flag("flag-one")]
    public string Flag1 { get; set; }  = "Default";

    [Flag("flag-two")]
    public int Flag2 { get; set; } = 101;
}
