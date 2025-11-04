namespace CliRouter.Core.Factories;

public interface IDynamicFactory
{
    dynamic CreateInstance(Type targetType, object[] argsForConstructor);
}
