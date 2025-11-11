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
    public void GivenAListOfFlagValuesOnly_WhenCreateCalled_ThenReturnsExpectedTypes()
    {
        //Arrange
        var type = typeof(ThreeStringConstructorWithFlagsModel);

        var args = new List<FlagValue>()
        {
            new FlagValue("flag-one", "value-one"),
            new FlagValue("flag-two", "100")
        };

        //Act
        var result = _sut.Create(type, args);

        //Assert
        Assert.Multiple(() => {
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Type, Is.EqualTo(typeof(string)));
            Assert.That(result[0].Value, Is.EqualTo("value-one"));
            Assert.That(result[0].IsFlag, Is.True);

            Assert.That(result[1].Type, Is.EqualTo(typeof(int)));
            Assert.That(result[1].Value, Is.EqualTo("100"));
            Assert.That(result[1].IsFlag, Is.True);
        });

    }
}
