using CliRouter.Core.Models;
using CliRouter.Core.Routes;
using System.Collections.Generic;

namespace CliRouter.Core.Services;

public class ObjectMapper : IObjectMapper
{
    public Dictionary<string, ITemplatedRoute> ToDictionary(List<FullyQualifiedRoute> routes)
    {
        return routes.ToDictionary(
                x => x.RoutePath,
                x => x.Route
            );
    }

    public void MapFlagsOnto(
            dynamic targetObject,
            List<GenericValue> flagValues,
            Object[] flagsAsObjects)
    {
        for (int i = 0; i < flagValues.Count; ++i)
        {
            var propertyValue = flagValues[i];
            var objectValue = flagsAsObjects[i];

            try
            {
                var property = propertyValue.PropertyInfo;
                if (property == null)
                {
                    throw new NotImplementedException($"Property Value {propertyValue} has no property info.");
                }
                property.SetValue(targetObject, objectValue);
            }
            catch (Exception ex)
            {
                var message = $"Value: '{propertyValue}' cannot be converted to type the expected type";
                throw new Exception(message, ex);
            }
        }
    }
}
