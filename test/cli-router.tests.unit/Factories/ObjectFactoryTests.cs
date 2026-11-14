using CliRouter.Core.Models;
using CliRouter.Core.Factories;
using NUnit.Framework;

namespace CliRouter.Core.Factories;

public class ObjectFactoryTests
{
    private readonly ObjectFactory _sut;

    public ObjectFactoryTests()
    {
        _sut = new ObjectFactory();
    }

    private enum TestEnum {
        FirstValue = 1
    }

    [TestCase(typeof(Int32), "1", 1)]
    [TestCase(typeof(Nullable<Int32>), "1", 1)]
    [TestCase(typeof(decimal), "3.5", 3.5)]
    [TestCase(typeof(Nullable<decimal>), "3.5", 3.5)]
    [TestCase(typeof(TestEnum), "1", TestEnum.FirstValue)]
    [TestCase(typeof(Nullable<TestEnum>), "1", TestEnum.FirstValue)]
    [TestCase(typeof(TestEnum?), "1", TestEnum.FirstValue)]
    public void GivenGenericValuesForFlagsOnConcreteFields_WhenCreateCalledThenMapsToExpectedObject(
        Type type,
        string stringValue,
        Object expectedResult)
    {
        //Arrange
        var flagGenericValue = new GenericValue(
            Type: type,
            Value: stringValue,
            IsFlag: true
        );

        //Act
        var actualResult = _sut.Create(flagGenericValue);

        //Assert
        Assert.That(expectedResult, Is.EqualTo(actualResult));
    }
}
