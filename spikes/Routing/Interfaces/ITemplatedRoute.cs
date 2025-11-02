public interface ITemplatedRoute 
{
    Task HandleAsync(Object request);
}

public interface ITemplatedRoute<T> : ITemplatedRoute
{
    Task HandleAsync(T request);

    Task ITemplatedRoute.HandleAsync(Object request) 
        => this.HandleAsync((T) request);
}
