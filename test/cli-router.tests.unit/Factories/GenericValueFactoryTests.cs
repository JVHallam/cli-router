using CliRouter.Core.Factories;
using CliRouter.Core.Models;

namespace CliRouter.Tests.Unit.Core.Factories;


public class GenericValueFactoryTests
{
    private record ThreeStringConstructorModel(
        string Arg1,
        string Arg2,
        string Arg3
    );

    [Test]
    public void GivenATypeAndValues_WhenCreateCalled_ThenReturnsExpectedTypes()
    {
        //Arrange
        var type = typeof(ThreeStringConstructorModel);
        var args = new string[]{ "one", "two", "three" };

        //Act
        var result = GenericValueFactory.Create(type, args);

        //Assert
        Assert.That(result.Count, Is.EqualTo(3));
        Assert.That(result[0].Type, Is.EqualTo(typeof(string)));
        Assert.That(result[1].Type, Is.EqualTo(typeof(string)));
        Assert.That(result[2].Type, Is.EqualTo(typeof(string)));
    }
}
