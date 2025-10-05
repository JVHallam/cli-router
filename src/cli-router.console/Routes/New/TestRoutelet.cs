namespace CliRouter.Console.Routes.New;

public class TestRoutelet : INewRoutelet
{
    public string Name => "test";

    public async Task HandleAsync(string[] args)
    {
        System.Console.WriteLine("New Test called");
    }
}
