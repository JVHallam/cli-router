using CliRouter.Core.Models;
using CliRouter.Core.Factories;

namespace CliRouter.Core.Routes;

//TODO: Delete Unused class
/*
public abstract class TemplatedRoutelet<T> : ITemplatedRoute<T>
{
    public abstract string Name { get; }

    public abstract Task HandleAsync(T request);

    public Task HandleAsync(string[] values)
    {
        var implementationTypeArgument = typeof(T);

        var genericValues = GenericValueFactory.Create(implementationTypeArgument, values);

        var constructorValues = genericValues
            .Where(x => !x.IsFlag)
            .ToList();

        var flagValues = genericValues
            .Where(x => x.IsFlag)
            .ToList();

        var argsForConstructor = ObjectFactory.Create(constructorValues);

        var argsForFlags = ObjectFactory.Create(flagValues);

        var request = DynamicFactory.CreateInstance(implementationTypeArgument, argsForConstructor);

        HandleFlags(request, flagValues, argsForFlags);

        return HandleAsync(request);
    }

    public void HandleFlags(
            dynamic targetObject, 
            List<GenericValue> flagValues, 
            Object[] flagsAsObjects)
    {
        for(int i = 0; i < flagValues.Count; ++i)
        {
            var propertyValue = flagValues[i];
            var objectValue = flagsAsObjects[i];

            try{
                var property = propertyValue.PropertyInfo;
                property.SetValue(targetObject, objectValue);
            }
            catch(Exception ex)
            {
                var message = $"Value: '{propertyValue.Value}' cannot be converted to type {propertyValue.PropertyInfo.PropertyType}";
                throw new Exception(message, ex);
            }
        }
    }
}
*/
