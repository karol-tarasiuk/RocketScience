using RocketScience.Library.Rocket;

namespace RocketScience.Library;

public class Configuration
{
    public Configuration()
    {
        RocketTypes = new Dictionary<Type, Func<ILandingCoordinator, IRocket>>();
    }

    public Dimensions LandingArea { get; init; }
    public LandingPlatform LandingPlatform { get; init; }
    public IReadOnlyDictionary<Type, Func<ILandingCoordinator, IRocket>> RocketTypes { get; init; }
}

public class LandingPlatform
{
    public Dimensions Size { get; init; }
    public Coordinates StartingPosition { get; init; }
}

public readonly record struct Dimensions
{
    public int Width { get; init; }
    public int Height { get; init; }
};

public readonly record struct Coordinates(int X, int Y);