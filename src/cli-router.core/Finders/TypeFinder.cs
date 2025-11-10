using System.Reflection;

namespace CliRouter.Core.Finders;

public static class TypeFinder
{
    public static IEnumerable<Type> GetAllTypes(params Assembly[] assemblies)
    {
        var allAssemblies = new List<Assembly>()
        {
            Assembly.GetExecutingAssembly()
        };

        var entryAssembly = Assembly.GetEntryAssembly();

        if (entryAssembly != null)
        {
            allAssemblies.Add(entryAssembly);
        }

        allAssemblies = allAssemblies.Concat(assemblies).ToList();

        var allTypes = allAssemblies
            .SelectMany(x => x.GetTypes());

        return allTypes;
    }
}
