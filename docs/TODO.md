# TODO:
- Add in arg-less routes (lmao, thought that'd be the first thing I did)
    - ISimpleRoute?

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
