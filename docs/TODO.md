# TODO:
- Requirements to finish before using this in next project:
    - Handle children via interfaces
        - Instead of injecting the children into the parent one at a time
            - Inject all children that implement an interface

            public interface IMySubRoute : IRoute { }

            - Then state in the top level method public MyRoute(IMySubRoute[] childRoutes) : base (childRoutes)

    - Handle the flags better
        - In the RootRoute, convert the input "cli new test --flag arg --flag2 arg2 route final" into a model
        - Pass that model along the chain, maybe alongside the current stage
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
- Add a directory of examples
- Introduce ILogger
- Get this into NUGET so I can use it in other projects

