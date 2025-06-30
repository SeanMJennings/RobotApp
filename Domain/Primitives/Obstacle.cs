namespace Domain.Primitives;

public readonly record struct Obstacle(Location Location)
{
    public static Obstacle Create(Location location)
    {
        return new Obstacle(location);
    }
}