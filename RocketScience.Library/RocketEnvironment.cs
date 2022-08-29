using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("RocketScience.Tests")]

namespace RocketScience.Library;

public sealed class RocketEnvironment
{
    private readonly RocketFactory rocketFactory;
    private readonly LandingCoordinator platformCoordinator;

    public RocketEnvironment(Configuration configuration)
    {
        platformCoordinator = new LandingCoordinator(configuration.LandingArea, configuration.LandingPlatform);
        rocketFactory = new RocketFactory(configuration.RocketTypes, platformCoordinator);
    }

    public ILandingCoordinator GetCoordinator()
    {
        return this.platformCoordinator;
    }

    public IRocketFactory GetRocketFactory()
    {
        return this.rocketFactory;
    }
}
