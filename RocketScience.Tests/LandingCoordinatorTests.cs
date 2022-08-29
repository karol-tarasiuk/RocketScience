using FluentAssertions;
using Moq;
using RocketScience.Library;
using RocketScience.Library.Exceptions;
using RocketScience.Library.Rocket;
using System.Net.Sockets;

namespace RocketScience.Tests;
public class LandingCoordinatorTests
{
    private LandingCoordinator coordinator;

    [SetUp]
    public void Init()
    {
        Dimensions areaConfig = new Dimensions { Width = 100, Height = 100 };
        LandingPlatform landingConfig = new()
        {
            Size = new Dimensions { Width = 10, Height = 10 },
            StartingPosition = new Coordinates(5, 5)
        };

        coordinator = new(areaConfig, landingConfig);
    }

    [TestCase(5, 5, ExpectedResult = ResponseEnum.Ok)]
    [TestCase(-3, -7, ExpectedResult = ResponseEnum.OutOfPlatform)]
    [TestCase(16, 15, ExpectedResult = ResponseEnum.OutOfPlatform)]
    [TestCase(0, 0, ExpectedResult = ResponseEnum.OutOfPlatform)]
    [TestCase(100, 100, ExpectedResult = ResponseEnum.OutOfPlatform)]
    [TestCase(15, 15, ExpectedResult = ResponseEnum.Ok)]
    public ResponseEnum When_CoordinatesProvided_Should_ReturnStatus(int rocketX, int rocketY)
    {
        IRocket rocket = Mock.Of<IRocket>();

        return coordinator.CheckCoordinates(rocket, new Coordinates(rocketX, rocketY));
    }

    [Test]
    public void When_CoordinatesExactlyTheSame_ClashResponseReturned()
    {
        IRocket rocket1 = Mock.Of<IRocket>();
        IRocket rocket2 = Mock.Of<IRocket>();

        coordinator.CheckCoordinates(rocket1, new Coordinates(10, 10));
        ResponseEnum result = coordinator.CheckCoordinates(rocket2, new Coordinates(10, 10));

        result.Should().Be(ResponseEnum.Clash);
    }

    [Test]
    public void When_CoordinatesShiftedOnXAxis_ClashResponseReturned()
    {
        IRocket rocket1 = Mock.Of<IRocket>();
        IRocket rocket2 = Mock.Of<IRocket>();

        coordinator.CheckCoordinates(rocket1, new Coordinates(10, 10));
        ResponseEnum result = coordinator.CheckCoordinates(rocket2, new Coordinates(11, 10));

        result.Should().Be(ResponseEnum.Clash);
    }

    [Test]
    public void When_CoordinatesShiftedOnYAxis_ClashResponseReturned()
    {
        IRocket rocket1 = Mock.Of<IRocket>();
        IRocket rocket2 = Mock.Of<IRocket>();

        coordinator.CheckCoordinates(rocket1, new Coordinates(10, 10));
        ResponseEnum result = coordinator.CheckCoordinates(rocket2, new Coordinates(10, 11));

        result.Should().Be(ResponseEnum.Clash);
    }

    [Test]
    public void When_CoordinatesShiftedOnBothAxis_ClashResponseReturned()
    {
        IRocket rocket1 = Mock.Of<IRocket>();
        IRocket rocket2 = Mock.Of<IRocket>();

        coordinator.CheckCoordinates(rocket1, new Coordinates(10, 10));
        ResponseEnum result = coordinator.CheckCoordinates(rocket2, new Coordinates(11, 11));

        result.Should().Be(ResponseEnum.Clash);
    }

    [Test]
    public void When_PlatformHasNoSize_Should_ThrowException()
    {
        Dimensions areaConfig = new Dimensions { Width = 100, Height = 100 };
        LandingPlatform landingConfig = new()
        {
            Size = new Dimensions { Width = 0, Height = 0 },
            StartingPosition = new Coordinates(5, 5)
        };

        Action coordinationCreation = () => coordinator = new(areaConfig, landingConfig);

        coordinationCreation.Should().Throw<InvalidConfigurationException>();
    }

    [Test]
    public void When_AreaHasNegativeSize_Should_ThrowException()
    {
        Dimensions areaConfig = new Dimensions { Width = -50, Height = -50 };
        LandingPlatform landingConfig = new()
        {
            Size = new Dimensions { Width = 10, Height = 10 },
            StartingPosition = new Coordinates(5, 5)
        };

        Action coordinationCreation = () => coordinator = new(areaConfig, landingConfig);

        coordinationCreation.Should().Throw<InvalidConfigurationException>();
    }

    [TestCase(0, 0, 100, 100, false)]
    [TestCase(0, 0, 101, 101, true)]
    [TestCase(1, 1, 100, 100, true)]
    [TestCase(10, 10, 90, 90, false)]
    [TestCase(20, 20, 200, 200, true)]
    [TestCase(20, 20, 70, 70, false)]
    [TestCase(0, 0, -10, -10, true)]
    public void When_PlatformIsBiggerThanArea_Should_ThrowException(int startX, int startY, int width, int height, bool exceptionExpected)
    {
        Dimensions areaConfig = new Dimensions { Width = 100, Height = 100 };
        LandingPlatform landingConfig = new()
        {
            Size = new Dimensions { Width = width, Height = height },
            StartingPosition = new Coordinates(startX, startY)
        };

        Action coordinationCreation = () => coordinator = new(areaConfig, landingConfig);

        if (exceptionExpected)
        {
            coordinationCreation.Should().Throw<InvalidConfigurationException>();
        }
        else
        {
            coordinationCreation.Should().NotThrow<InvalidConfigurationException>();
        }
    }
}
