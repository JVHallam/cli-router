using CliRouter.Core.Routes;

namespace CliRouter.Core.Services;

public class GenericTypeService : IGenericTypeService
{
    public Type GetImplementationTypeArgument(ITemplatedRoute templatedRoute)
    {
        var implementedInterface = templatedRoute
            .GetType()
            .GetInterfaces()
            .Where(x => x.IsGenericType)
            .FirstOrDefault();

        if (implementedInterface == null)
        {
            return typeof(string[]);
        }

        var implementationTypeArguementType = implementedInterface
            .GetGenericArguments()[0];

        return implementationTypeArguementType;
    }
}
