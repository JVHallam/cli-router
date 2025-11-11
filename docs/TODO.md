# TODO:
- Requirements to finish before using this in next project:
    - Handle the flags better
        - In the RootRoute, convert the input "cli new test --flag arg --flag2 arg2 route final" into a model
        - Pass that model along the chain, maybe alongside the current stage
        - Flags need to be put at the end at the moment

    - I need the help function. That's absolutely essential for UX

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
