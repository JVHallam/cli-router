using CliRouter.Core.Routes;

public record GenericInvocation<T>  (
    T Args,
    ITemplatedRoute InvokedRoute
);
