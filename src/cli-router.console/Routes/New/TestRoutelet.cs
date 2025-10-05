namespace CliRouter.Console.Routes.New;

public class TestRoutelet : INewRoutelet
{
    public string Name => "test";

    public Task HandleAsync(string[] args)
    {
        System.Console.WriteLine("New Test called");

        return Task.CompletedTask;
    }
}
