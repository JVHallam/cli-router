using CliRouter.Core.Routes;

namespace CliRouter.Core.Services;

public interface IGenericTypeService
{
    public Type GetImplementationTypeArgument(ITemplatedRoute templatedRoute);
}
