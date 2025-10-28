using Models;
using Interfaces;

//Parents Job is soley to route to the children, based on the values
//Child handles the object that results at the end
public class Parent
{
    private readonly List<IHandler> _children;

    public Parent()
    {
        _children = new List<IHandler>()
        {
            new Child(),
            new StringsChild()
        };
    }

    public void Handle(params string[] values)
    {
        foreach(var child in _children)
        {
            child.Handle(values);
        }
    }
}
