using RocketScience.Library;
using RocketScience.Library.Rocket;
using RocketScience.Tests.Rockets;

namespace RocketScience.Tests;

public class RocketEnvironmentTests
{
    [Test]
    public async Task RealUsage()
    {
        Configuration landingControllerConfig = new()
        {
            LandingArea = new Dimensions
            {
                Height = 100,
                Width = 100
            },
            LandingPlatform = new LandingPlatform
            {
                Size = new Dimensions { Width = 10, Height = 10 },
                StartingPosition = new Coordinates { X = 5, Y = 5 },
            },
            RocketTypes = new Dictionary<Type, Func<ILandingCoordinator, IRocket>>
            {
                { typeof(BlindRocket), lc => new BlindRocket(lc, new Dimensions { Width = 100, Height = 100 }) },
                { typeof(SequentionalRocket), lc => new SequentionalRocket(lc, new Coordinates(20, 20)) }
            }
        };

        RocketEnvironment landingController = new RocketEnvironment(landingControllerConfig);
        var rocketFactory = landingController.GetRocketFactory();

        SequentionalRocket rocket1 = rocketFactory.CreateRocket<SequentionalRocket>();
        SequentionalRocket rocket2 = rocketFactory.CreateRocket<SequentionalRocket>();
        BlindRocket rocket3 = rocketFactory.CreateRocket<BlindRocket>();

        Task rocketCheck1 = Task.Run(() => rocket1.CheckTrajectory());
        Task rocketCheck2 = Task.Run(() => rocket2.CheckTrajectory());
        Task rocketCheck3 = Task.Run(() => rocket3.CheckTrajectory());

        await Task.WhenAll(rocketCheck1, rocketCheck2, rocketCheck3);
    }
}