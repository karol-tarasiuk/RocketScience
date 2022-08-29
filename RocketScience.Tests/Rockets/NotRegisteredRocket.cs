using RocketScience.Library.Rocket;

namespace RocketScience.Tests.Rockets;
public class NotRegisteredRocket : IRocket
{
    public void CheckTrajectory()
    {
        throw new NotImplementedException();
    }
}
