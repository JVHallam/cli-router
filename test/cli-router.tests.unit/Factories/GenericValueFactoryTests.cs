using CliRouter.Core.Factories;
using CliRouter.Core.Models;
using CliRouter.Core.Attributes;

namespace CliRouter.Tests.Unit.Core.Factories;

public class GenericValueFactoryTests
{
    private record ThreeStringConstructorModel(
        string Arg1,
        string Arg2,
        string Arg3
    );

    private GenericValueFactory _sut; 

    [SetUp]
    public void SetUp()
    {
        _sut = new GenericValueFactory();
    }

    [Test]
    public void GivenATypeAndValues_WhenCreateCalled_ThenReturnsExpectedTypes()
    {
        //Arrange
        var type = typeof(ThreeStringConstructorModel);
        var args = new string[]{ "one", "two", "three" };

        //Act
        var result = _sut.Create(type, args);

        //Assert
        Assert.That(result.Count, Is.EqualTo(3));
        Assert.That(result[0].Type, Is.EqualTo(typeof(string)));
        Assert.That(result[0].Value, Is.EqualTo("one"));
        Assert.That(result[0].IsFlag, Is.False);

        Assert.That(result[1].Type, Is.EqualTo(typeof(string)));
        Assert.That(result[1].Value, Is.EqualTo("two"));
        Assert.That(result[1].IsFlag, Is.False);

        Assert.That(result[2].Type, Is.EqualTo(typeof(string)));
        Assert.That(result[2].Value, Is.EqualTo("three"));
        Assert.That(result[2].IsFlag, Is.False);
    }

    private record ThreeStringConstructorWithFlagsModel(
        string Arg1,
        string Arg2,
        string Arg3
    )
    {
        [Flag("flag-one")]
        public required string MyFlag { get; set; }

        [Flag("flag-two")]
        public required int MySecondFlag { get; set; }
    }

    [Test]
    public void GivenATypeWithFlagsAndValues_WhenCreateCalled_ThenReturnsExpectedTypes()
    {
        //Arrange
        var type = typeof(ThreeStringConstructorWithFlagsModel);
        var args = new string[]{ 
            "one", 
            "--flag-one", 
            "value", 
            "two", 
            "--flag-two",
            "10",
            "three" 
        };

        //Act
        var result = _sut.Create(type, args);

        //Assert
        Assert.That(result.Count, Is.EqualTo(5));
        Assert.That(result[0].Type, Is.EqualTo(typeof(string)));
        Assert.That(result[0].Value, Is.EqualTo("one"));
        Assert.That(result[0].IsFlag, Is.False);

        Assert.That(result[1].Type, Is.EqualTo(typeof(string)));
        Assert.That(result[1].Value, Is.EqualTo("two"));
        Assert.That(result[1].IsFlag, Is.False);

        Assert.That(result[2].Type, Is.EqualTo(typeof(string)));
        Assert.That(result[2].Value, Is.EqualTo("three"));
        Assert.That(result[2].IsFlag, Is.False);

        Assert.That(result[3].Type, Is.EqualTo(typeof(string)));
        Assert.That(result[3].Value, Is.EqualTo("value"));
        Assert.That(result[3].IsFlag, Is.True);

        Assert.That(result[4].Type, Is.EqualTo(typeof(int)));
        Assert.That(result[4].Value, Is.EqualTo("10"));
        Assert.That(result[4].IsFlag, Is.True);
    }
}
