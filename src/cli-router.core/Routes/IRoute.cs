namespace CliRouter.Core.Routes;

/*
public interface IRoute 
{
    public string Name { get; }
    public Task HandleAsync(string[] args);
}
*/

public interface IRoute : ITemplatedRoute<string[]>
{
}
