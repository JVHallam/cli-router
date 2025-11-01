namespace CliRouter.Tests.Integration.TestClasses;

public record FlagsModel(
    string Arg1,
    int Arg2
)
{
    public string Flag1 { get; set; }  = "Default";
    public int Flag2 { get; set; } = 101;
}
