# TODO:
- The mighty cleanup:
    - Search out all non-interface classes and DI them

    - Clean up the remaining compiler warnings

    - Adhere to the orchestrator pattern
        - Rename Router.cs to RouteOrchestrator
        - Router.cs is the entry point of this project and therefore should be the orchestrator
        - This will require breaking stuff down into some more classes

    - Refactor and break down some bulky classes
        - GenericValueFactory.cs
        - Router.cs

    - Delete unused classes

- Requirements to finish before using this in next project:
    - Handle children via interfaces
        - Instead of injecting the children into the parent one at a time
            - Inject all children that implement an interface

            public interface IMySubRoute : IRoute { }

            - Then state in the top level method public MyRoute(IMySubRoute[] childRoutes) : base (childRoutes)

    - Handle the flags better
        - In the RootRoute, convert the input "cli new test --flag arg --flag2 arg2 route final" into a model
        - Pass that model along the chain, maybe alongside the current stage

## Integration tests
- I want integration tests
- Which requires the DI container setup
- But how would injection and test stuff work?
- Suppose I'd need some test classes too
- Could use the extensions to pick up on the test classes

## Endpoint Models
public class Args {
    string Input { get; set; }
    DateTime DateFrom { get; set; }
    DateTime DateTo { get; set; }
}

## Handling flags:
- Just extract them out up front
- Pass them down the chain as a dictionary
- Could wrap up the whole request

What I could do:
- Parse the input
rsis new --my-var true --debug info test file
new request {
    string[] routes {
        rsis
        new
        test
        file
    }

    Dictionary<string, string> flags { get; set; }
}

Then pass that down the chain

# The rest:
- Remove the requirement for the "Name" property on things
    - UpdateRoutelet => update is the name
    - CalculateRoute => Calculate is the name
    - This enforces naming conventions going fowards
    - Also less to implement as you go along

- Add the default sub commands to each route:
    - List
        - st calculate list => Will list all subcommands for st calculate
    - Usage
        - st calculate atr => Requires args, so will output "st calculate atr <security> <date>"
    - Help
        - Basically the usage command, but with some help text - implementation of which is optional

- Add a directory of examples
- Introduce ILogger
- Get this into NUGET so I can use it in other projects


In the order they appear
OR flags
Have the inputs serialized into the above
Just by specifying the args on the model

## Automatic routing
- I shouldn't have to setup BaseRoutes tbqh
- This is boiler plate code that setting up is a little tiring
- I just want to be able to do:
    - /Routes/MyRoutes/Calculate/Test.cs
- This would remove the concept of the routes and routelet classes from the end user
