# TODO:
- Handle children via interfaces
    - Instead of injecting the children into the parent one at a time
        - Inject all children that implement an interface

        public interface IMainRoute : IRoute { }

        - Then state in the top level method public MyRoute(IMainRoute[] childRoutes) : base (childRoutes)

- Add extension methods for this to work with DI
- Add a directory of examples
- Introduce ILogger
- Get this into NUGET so I can use it in other projects

Handling flags:
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
