Given the routes:
rsis 
    test
        new
    config
        show
        new
        add

I want the following commands to result in the following output:

rsis --help:
    test
    config

rsis test --help:
    new     : args

rsis config --help:
    show    : args
    new     : args
    add     : args

This can be done by:
    Routing to the inbuilt help route
    if the flag --help is in the command
    then --help becomes the route

Custom help entries for test and config could then be implemented by hand by the end user.

The top level --help scenario:
    If help is in the flags
    Then shortcircuit to the built in help controller

    The in-built help route:
        - Outputs all top level routes

The second level --help scenario:
    If help is in the flags
    And the key doesn't match but it has values in it
    Find all keys that match it partially
    Pass all that to the help route
    Output all the keys that match it + 1

The third level --help scenario:
    If help is in the flags
    Gets the longest key anyhow
    Then goes to the help route with the expected route
    Uses reflection to output the args for each 
