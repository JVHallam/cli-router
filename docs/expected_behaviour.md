# Expected behaviour

Scenario: I call a route with the required args
Given a route with essential args
When I call that route with the args required
Then the route is called
(Implemented prior to 01/11/25)

Scenario: I call a route with the required args
Given a typed route with args
When I call that route with the args required
Then the route is called with the args converted into a model
(Implemented prior to 03/11/25)

Scenario: I call a route with flags
Given a route with Flags
When I call that route with the flags in any position
Then the args are parsed onto the model and passed to the route
(Implemented prior to 04/11/25)

Scenario: I call help on a route
Given a route
When the route is called with the --help flag
Then the help text is output, regardless of whether the args are correct
And it tells me what args there are, what flags and what the datatypes for all of those are

Scenario: I call a route that needs args, without the args
Given a route with essential args
When I call that route without any args
Then I'm notified what args are required
And the route is not called

Scenario: I call a route with the wrong args
Given a route with typed args
When I call that route with the wrong arg types
Then I'm notified that the given arg (e.g. time) is of the wrong format

Scenario: I call a route with flags but the wrong arg types
Given a route with flags
When I call that route with the flags but the wrong data types
Then I'm notified that the given flag needs to be of type

Scenario: I call a route with flags but don't provide args for the flags
Given a route with flags
When I call that route with the flags
Then the route still executes but with the defaults for the flags

Scenario: I call a route without sufficient args
Given A route with args
When I call that route and provide no args
Then the text for "help" is output instead

Scenario: Too many args provided
Given a route with args
When I call that route with too many args
Then I'm told that I've provided too many args
