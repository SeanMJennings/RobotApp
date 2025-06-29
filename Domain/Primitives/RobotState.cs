namespace Domain.Primitives;

public record RobotState
{
    public Direction Direction { get; private init; }
    public Location Location { get; private init; }

    public static RobotState Create(Location location, Direction direction)
    {
        return new RobotState { Location = location, Direction = direction };
    }
}