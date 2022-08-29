using RocketScience.Library.Exceptions;
using RocketScience.Library.Rocket;

namespace RocketScience.Library;

public interface IRocketFactory
{
    TRocket CreateRocket<TRocket>() where TRocket : IRocket;
}

internal sealed class RocketFactory : IRocketFactory
{
    private readonly IReadOnlyDictionary<Type, Func<ILandingCoordinator, IRocket>> rocketTypes;
    private readonly ILandingCoordinator landingCoordinator;

    public RocketFactory(IReadOnlyDictionary<Type, Func<ILandingCoordinator, IRocket>> rocketTypes, ILandingCoordinator landingCoordinator)
    {
        this.rocketTypes = rocketTypes;
        this.landingCoordinator = landingCoordinator;
    }

    public TRocket CreateRocket<TRocket>() where TRocket : IRocket
    {
        bool rocketTypeFound = rocketTypes.TryGetValue(typeof(TRocket), out var rocket);

        if (rocketTypeFound && rocket is { })
        {
            return (TRocket)rocket.Invoke(this.landingCoordinator);
        }

        throw new RocketTypeNotFoundException(typeof(TRocket));
    }
}