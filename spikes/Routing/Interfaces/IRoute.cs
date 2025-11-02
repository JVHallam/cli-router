public interface IRoute
{
    Task HandleAsync(string[] args);
}
