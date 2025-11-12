namespace CliRouter.Core.Routes;

public interface IBuiltInRoute
{
    bool CanHandle(string[] allArgs);
    Task HandleAsync(Dictionary<string, ITemplatedRoute> routes, string[] allArgs);
}
