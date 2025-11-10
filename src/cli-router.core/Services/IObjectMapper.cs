using CliRouter.Core.Models;
using CliRouter.Core.Routes;
using System.Collections.Generic;

namespace CliRouter.Core.Services;

public interface IObjectMapper
{
    Dictionary<string, ITemplatedRoute> ToDictionary(List<FullyQualifiedRoute> routes);
    void MapFlagsOnto(dynamic targetObject, List<GenericValue> flagValues, Object[] flagsAsObjects);
}
