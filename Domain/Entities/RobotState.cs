using Domain.Primitives;
using FunctionalProgrammingKit;

namespace Domain.Entities;

public record RobotState
{
    public Direction Direction { get; private init; }
    public Location Location { get; private init; }

    public static Entity<RobotState> Create(Location location, Direction direction)
    {
        return Entity<RobotState>.Valid(new RobotState { Location = location, Direction = direction });
    }

    public static Entity<RobotState> Create(ValueObject<Location> location, ValueObject<Direction> direction)
    {
        return location.Match(
            Entity<RobotState>.Invalid,
            validLocation => direction.Match(
                Entity<RobotState>.Invalid,
                validDirection => Entity<RobotState>.Valid(new RobotState { Location = validLocation, Direction = validDirection })));
    }
}