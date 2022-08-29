using RocketScience.Library.Exceptions;
using RocketScience.Library.Rocket;
using System.Collections.Concurrent;

namespace RocketScience.Library;

public interface ILandingCoordinator
{
    ResponseEnum CheckCoordinates(IRocket rocket, Coordinates position);
}

internal sealed class LandingCoordinator : ILandingCoordinator
{
    private readonly object lockObj = new();

    private readonly Dimensions areaConfiguration;
    private readonly LandingPlatform platformConfiguration;

    private readonly BlockingCollection<LandingLot> landingMatrix;

    public LandingCoordinator(Dimensions areaConfiguration, LandingPlatform platformConfiguration)
    {
        ValidateConfiguration(areaConfiguration, platformConfiguration);

        this.areaConfiguration = areaConfiguration;
        this.platformConfiguration = platformConfiguration;

        this.landingMatrix = new BlockingCollection<LandingLot>();
    }

    public ResponseEnum CheckCoordinates(IRocket rocket, Coordinates position)
    {
        lock (lockObj)
        {
            var recordedRocket = landingMatrix.SingleOrDefault(r => r.RocketId == rocket.GetHashCode());

            if (recordedRocket is not null)
            {
                return ResponseEnum.Ok;
            }

            if (position.X < platformConfiguration.StartingPosition.X
                || position.X > platformConfiguration.Size.Width + platformConfiguration.StartingPosition.X
                || position.Y < platformConfiguration.StartingPosition.Y
                || position.Y > platformConfiguration.Size.Height + platformConfiguration.StartingPosition.Y
                || position.X > areaConfiguration.Width
                || position.Y > areaConfiguration.Height)
            {
                return ResponseEnum.OutOfPlatform;
            }

            bool isAnyConflicting = landingMatrix.Any(l => l.InRange(position));

            if (isAnyConflicting)
            {
                return ResponseEnum.Clash;
            }

            this.landingMatrix.Add(new LandingLot(position, rocket.GetHashCode()));
            return ResponseEnum.Ok;
        }
    }

    private void ValidateConfiguration(Dimensions areaConfiguration, LandingPlatform platformConfiguration)
    {
        if(areaConfiguration.Width < 0 || areaConfiguration.Height < 0)
        {
            throw new InvalidConfigurationException("Area dimensions are invalid");
        }

        if (platformConfiguration.Size.Width < 0 || platformConfiguration.Size.Height < 0)
        {
            throw new InvalidConfigurationException("Platform dimensions are invalid");
        }

        if (platformConfiguration.Size.Width == 0 || platformConfiguration.Size.Height == 0)
        {
            throw new InvalidConfigurationException("Landing platform should have at least 1x1 in dimensions");
        }

        if(platformConfiguration.StartingPosition.X + platformConfiguration.Size.Width > areaConfiguration.Width
            || platformConfiguration.StartingPosition.Y + platformConfiguration.Size.Height > areaConfiguration.Height)
        {
            throw new InvalidConfigurationException("Landing platform is bigger than landing area");
        }
    }

    private class LandingLot
    {
        private readonly Coordinates position;

        public LandingLot(Coordinates position, int rocketId)
        {
            this.position = position;
            RocketId = rocketId;
        }

        public int RocketId { get; }

        public bool InRange(Coordinates coordinates)
        {
            if(new int[] { position.X, position.X + 1, position.X - 1 }.Contains(coordinates.X))
            {
                return true;
            }

            if (new int[] { position.Y, position.Y + 1, position.Y - 1 }.Contains(coordinates.Y))
            {
                return true;
            }

            return false;
        }
    }
}