namespace CliRouter.Core.Routes; 

public interface IBuiltInRouter
{
    bool CanHandle(string[] allArgs);
    Task HandleAsync(Dictionary<string, ITemplatedRoute> routes, string[] allArgs);
}
