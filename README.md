# C# CLI Router

- What:
Makes routing commands feel a bit more like ASP.NET 

- Why:
I wanted to standardise how I did things across projects

- How:
Build
Install
Push to nuget

- Examples:
TODO

## Flags
A typical request object would be a record
```
public record GetOfferRequest (
    ItemType ItemType
);
```

Flags required this syntax:
```
using CliRouter.Core.Attributes;

...

public record GetOfferRequest
{
    [Flag("ItemType")]
    public ItemType? ItemType { get; set; }
}
```

Then, simply consumed like such
```
my-cli my-route get --ItemType 1
```
