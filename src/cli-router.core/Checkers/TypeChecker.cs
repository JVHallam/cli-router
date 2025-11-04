namespace CliRouter.Core.Checkers;

public static class TypeChecker
{
    public static bool IsImplementationOf<T>(Type childType)
    {
        var parentType = typeof(T);

        if(!childType.IsClass)
        {
            return false;
        }

        if(childType.IsAbstract)
        {
            return false;
        }

        if(childType.IsInterface)
        {
            return false;
        }

        if(!parentType.IsAssignableFrom(childType))
        {
            return false;
        }

        return true;
    }
}
