# TODO:
- Add in arg-less routes (lmao, thought that'd be the first thing I did)
    - ISimpleRoute?

- Usage attribute
    - An attribute that goes over the route for example usage
    - [Usage("hm human create 1 0 0")];
    - This is printed in the help menu

- Automatically print items as json
    - Items returned from the route are serialized and printed as json automatically
    - That way routes only need to validate and fetch the object, not print it

- Argless routes on arged routes
I have the route
```
public class GetRoute : ITemplatedRoute<GetHumanRequest>
{
    public async Task HandleAsync(GetHumanRequest request)
}
```
This will only be able to be invoked with the correct args.
It'd be nice If I could an a method to the route that allows it to be invoked with nothing.
This is Probably an extension of the earlier ISimpleRoute interface.
Use case:
```
# Get human with id 1
hm human get 1

# Get all humans
hm human get
```
That way my CLI feels more like a rest api than something I've hobbled together.

- Nicer error messages:
    - When a route isn't found, it's best we catch the exception and print a nicer message
        - Instead of the exception stack, more "Command args for <command> are not valid."
    - When the wrong number of args are passed in, it complains that there wasn't enough
        - Unhandled exception. System.MissingMethodException: Constructor on type 'HonestMerchants.Console.Models.Human.CreateHumanRequest' not found.
        - Should just be the usage attributes' value

- You can supply ONLY the expected number of args
    - if too many args are passed in for the model then you trigger the same errors as above

- Support for enums
    - You can't have an Enum on a request model. We only support built ins
    - This can't be that bad, right?

- Prettier help message
    - Atm it works but it doesn't feel very nice
    - It's not the easiest to read

Route : human create
Route requires: 
HumanType : HonestMerchants.Core.Enums.HumanType
CurrentBalance : System.Decimal
EquilibriumAmount : System.Decimal
Flags:
None.

Would probably be better as:
Route:
    human create
Args:
    HumanType : HonestMerchants.Core.Enums.HumanType
    CurrentBalance : System.Decimal
    EquilibriumAmount : System.Decimal
Flags:
    None

- Examples on help messages
    Example:
        human create 0 0 0

- Help summary
    - I'd like the option to have a little summary for the application
    - Maybe as simple as adding an embedded file into the consuming project

# The rest:
- Add the default sub commands to each route:
    - Simply put, if the given commands don't match anything, call help with those keys
    - If nothing is matched, have help tell them nothing was found

    - List
        - st calculate list => Will list all subcommands for st calculate
    - Usage
        - st calculate atr => Requires args, so will output "st calculate atr <security> <date>"
    - Help
        - Basically the usage command, but with some help text - implementation of which is optional

- Add a directory of examples
- Introduce ILogger?
