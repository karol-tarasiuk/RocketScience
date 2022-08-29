namespace RocketScience.Library.Exceptions;

public class RocketTypeNotFoundException : Exception
{
    public RocketTypeNotFoundException()
    {
    }

    public RocketTypeNotFoundException(string message) : base(message)
    {
    }

    public RocketTypeNotFoundException(string message, Exception inner) : base(message, inner)
    {
    }

    public RocketTypeNotFoundException(Type rocketType) : this($"Rocket of type {rocketType.Name} not supported")
    {
    }
}
