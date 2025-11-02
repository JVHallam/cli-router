# Removing Base Classes
I think overall, I liked the idea of inherriting base classes for the convenience and speed.
However, Now that I want to implement more and more unit testing and not just rely on
integration tests, I'm finding them cumbersome. 
The limit unit testing, they force integration tests and test scope becomes quite large.
All in all, they seemed convenient whilst the project was in it's infancy. I'm now however
finding issues trying to grow the project further. When it was a toy project with no
real use or even long term plan, inherritence was amazing. Now that I'm trying to add
more features, it feels more like tech debt. Which is crazy as this is still a tiny code base.

## IRoute.cs
IRoute and ITemplatedRoute<T> are the only things I want the end user using
IRoute {
    Task HandleAsync(string[] request);
}

ITemplatedRoute<T>
{
    Task HandleAsync(T request);
}

This will require that routing be handled by Folders and namespaces.
I can do that without an issue.
I can use DI and some runtime resolution to handle all of this.

RootRoute 
=> Has all IRoute's injected into it
=> Creates Tree using all the Routes injected into it

Your project should be:
MyProject.NamespaceStuff.Routes.First.Second.Third;
MyProject/
    Routes/
        First/
            MyRoute.cs

The command to get to this route would then be:
MyConsoleApp first my-route <args>

It might just be best to build the tree code from scratch and throw
out the old bases.
