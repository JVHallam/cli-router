using CliRouter.Core.Attributes;

namespace CliRouter.Tests.Integration.TestClasses;

public record NullableFieldsRequest
{
    [Flag("NullableInt")]
    public int? NullableInt { get; set; }
}
