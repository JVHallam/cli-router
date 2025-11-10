using CliRouter.Core.Models;

namespace CliRouter.Core.Factories;

public interface IObjectFactory
{
    public Object[] Create(List<GenericValue> genericValues);
    public Object Create(GenericValue genericValue);
}
