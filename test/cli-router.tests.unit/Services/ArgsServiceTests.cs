using CliRouter.Core.Services;
using FakeItEasy;
using NUnit.Framework;

namespace CliRouter.Core.Services;

public class ArgsServiceTests
{
    private ArgsService _sut;

    [SetUp]
    public void SetUp()
    {
        _sut = new ArgsService(

        );
    }

    [TestCase("--flag-one", "valueone", "route-one", "--flag-two", "valuetwo", "route-two", "arg1", "arg2")]
    [TestCase("route-one", "--flag-one", "valueone", "route-two", "--flag-two", "valuetwo", "arg1", "arg2")]
    [TestCase("route-one", "--flag-one", "valueone", "route-two", "arg1", "--flag-two", "valuetwo", "arg2")]
    public void GivenArgsWithFlagsIn_WhenRemoveFlagsCalled_ThenRemovesFlags(params string[] args)
    {
        //Arrange
        var expectedArgs = new string[]
        {
            "route-one",
            "route-two",
            "arg1",
            "arg2"
        };

        //Act
        var actualArgs = _sut.RemoveFlags(args);

        //Assert
        Assert.Multiple(() => {
            Assert.That(actualArgs.Length, Is.EqualTo(4));
            Assert.That(actualArgs[0], Is.EqualTo("route-one"));
            Assert.That(actualArgs[1], Is.EqualTo("route-two"));
            Assert.That(actualArgs[2], Is.EqualTo("arg1"));
            Assert.That(actualArgs[3], Is.EqualTo("arg2"));
        });
    }

    [TestCase("--flag-one", "valueone", "route-one", "--flag-two", "valuetwo", "route-two", "arg1", "arg2")]
    [TestCase("route-one", "--flag-one", "valueone", "route-two", "--flag-two", "valuetwo", "arg1", "arg2")]
    [TestCase("route-one", "--flag-one", "valueone", "route-two", "arg1", "--flag-two", "valuetwo", "arg2")]
    public void GivenArgsWithFlags_WhenGetFlagsCalled_ThenReturnsArgs(params string[] args)
    {
        //When
        var flagsAndValues = _sut.GetFlags(args);

        //Then
        Assert.Multiple(() => {
            Assert.That(flagsAndValues[0], Is.EqualTo("--flag-one"));
            Assert.That(flagsAndValues[1], Is.EqualTo("valueone"));
            Assert.That(flagsAndValues[2], Is.EqualTo("--flag-two"));
            Assert.That(flagsAndValues[3], Is.EqualTo("valuetwo"));
        });
    }
}
