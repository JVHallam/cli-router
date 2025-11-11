using CliRouter.Core.Factories;
using FakeItEasy;
using NUnit.Framework;

namespace CliRouter.Core.Factories;

public class FlagValueFactoryTests
{

    private FlagValueFactory _sut;

    [SetUp]
    public void SetUp()
    {
        _sut = new FlagValueFactory(

        );
    }

    [Test]
    public void GivenAnArrayWithFlags_WhenCreateCalled_ThenReturnsExpectedFlagValues()
    {
        //Arrange
        var args = new string[]
        {
            "--flag-one",
            "value-one",
            "--flag-two",
            "value-two"
        };

        //Act
        var actualFlags = _sut.Create(args);

        //Assert
        Assert.Multiple(() => {
            Assert.That(actualFlags.Count, Is.EqualTo(2));

            //Make sure it's stripping the -- from the input string
            Assert.That(actualFlags[0].FlagName, Is.EqualTo("flag-one"));
            Assert.That(actualFlags[0].Value, Is.EqualTo("value-one"));

            Assert.That(actualFlags[1].FlagName, Is.EqualTo("flag-two"));
            Assert.That(actualFlags[1].Value, Is.EqualTo("value-two"));
        });
    }
}
