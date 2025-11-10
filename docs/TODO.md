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

# The rest:
- Add the default sub commands to each route:
    - List
        - st calculate list => Will list all subcommands for st calculate
    - Usage
        - st calculate atr => Requires args, so will output "st calculate atr <security> <date>"
    - Help
        - Basically the usage command, but with some help text - implementation of which is optional

- Add a directory of examples
- Introduce ILogger?
- Get this into NUGET so I can use it in other projects without suffering
    - Or just local nuget

In the order they appear
OR flags
Have the inputs serialized into the above
Just by specifying the args on the model
