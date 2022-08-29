using RocketScience.Library;
using RocketScience.Library.Rocket;

namespace RocketScience.Tests.Rockets;
public class SequentionalRocket : IRocket
{
    private readonly ILandingCoordinator landingCoordinator;
    private readonly Coordinates maxRange;
    private Coordinates landingCoorinates;

    public SequentionalRocket(ILandingCoordinator landingCoordinator, Coordinates maxRange)
    {
        this.landingCoordinator = landingCoordinator;
        this.maxRange = maxRange;
        landingCoorinates = new Coordinates(0, 0);
    }

    public void CheckTrajectory()
    {
        ResponseEnum landingStatus;
        int currentX = 0, currentY = 0;

        do
        {
            landingStatus = landingCoordinator.CheckCoordinates(this, landingCoorinates);
            landingCoorinates = new Coordinates(currentX++% maxRange.X, currentY++%maxRange.Y);
        }
        while (landingStatus != ResponseEnum.Ok);
    }
}
