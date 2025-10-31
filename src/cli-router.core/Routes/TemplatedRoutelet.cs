using CliRouter.Core.Models;
using CliRouter.Core.Factories;

namespace CliRouter.Core.Routes;

public abstract class TemplatedRoutelet<T> : ITemplatedRoute<T>
{
    public abstract string Name { get; }

    public abstract Task HandleAsync(T request);

    public Task HandleAsync(string[] values)
    {
        var implementationTypeArgument = typeof(T);

        var genericValues = GenericValueFactory.Create(implementationTypeArgument, values);

        var argsForConstructor = ObjectFactory.Create(genericValues);

        var request = DynamicFactory.CreateInstance(implementationTypeArgument, argsForConstructor);

        return HandleAsync(request);
    }
}
