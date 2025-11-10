using System.Reflection;

namespace CliRouter.Core.Factories;

public interface IGenericValueFactory
{
    List<GenericValue> Create(Type type, string[] values);
}
