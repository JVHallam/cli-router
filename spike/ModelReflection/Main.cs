using Models;
using Interfaces;

public class Main()
{
    public void Run()
    {
        var parent = new Parent();

        parent.Handle("This is a string", "2025-01-01");
    }
}
