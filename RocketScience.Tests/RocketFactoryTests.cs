using FluentAssertions;
using Moq;
using RocketScience.Library;
using RocketScience.Library.Exceptions;
using RocketScience.Library.Rocket;
using RocketScience.Tests.Rockets;

namespace RocketScience.Tests;
public class RocketFactoryTests
{
    [Test]
    public void When_RocketTypeIsNotRegistered_Should_ThrowException()
    {
        Dictionary<Type, Func<ILandingCoordinator, IRocket>> rocketTypes = new()
        {
            { typeof(BlindRocket), lc => new BlindRocket(lc, new Dimensions { Width = 100, Height = 100 }) },
            { typeof(SequentionalRocket), lc => new SequentionalRocket(lc, new Coordinates(20, 20)) }
        };

        RocketFactory rocketFactory = new RocketFactory(rocketTypes, Mock.Of<ILandingCoordinator>());

        Action rocketCreation = () => rocketFactory.CreateRocket<NotRegisteredRocket>();

        rocketCreation.Should().Throw<RocketTypeNotFoundException>();
    }
}
