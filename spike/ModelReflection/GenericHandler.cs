using Interfaces;
using Models;

public abstract class GenericHandler<T> : IGenericHandler<T>
{
    public abstract void Handle(T request);

    public void Handle(string[] values)
    {
        var implementationTypeArgument = typeof(T);

        var genericValues = GenericValueFactory.Create(implementationTypeArgument, values);

        var argsForConstructor = genericValues
            .Select(ObjectFactory.Create)
            .ToArray();

        dynamic? request = Activator.CreateInstance(implementationTypeArgument, argsForConstructor);

        if(request == null)
        {
            throw new NotImplementedException("The created request was null. Are you sure the args were correct?");
        }

        Handle(request);
    }
}
