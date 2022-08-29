using RocketScience.Library;
using RocketScience.Library.Rocket;

namespace RocketScience.Tests.Rockets;
internal class BlindRocket : IRocket
{
    private readonly ILandingCoordinator landingCoordinator;
    private readonly Dimensions landingArea;
    private Coordinates landingCoorinates;

    public BlindRocket(ILandingCoordinator landingCoordinator, Dimensions landingArea)
    {
        this.landingCoordinator = landingCoordinator;
        this.landingArea = landingArea;
        landingCoorinates = new Coordinates(0, 0);
    }

    public void CheckTrajectory()
    {
        ResponseEnum landingStatus;

        Random random = new();

        do
        {
            landingStatus = landingCoordinator.CheckCoordinates(this, landingCoorinates);
            landingCoorinates = new Coordinates(random.Next(0, landingArea.Width), random.Next(0, landingArea.Height));
        }
        while (landingStatus != ResponseEnum.Ok);
    }
}
