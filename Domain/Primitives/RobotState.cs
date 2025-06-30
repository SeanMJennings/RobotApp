namespace Domain.Primitives;

public record RobotState
{
    public Direction Direction { get; init; }
    public Location Location { get; init; }

    public static RobotState Create(Location location, Direction direction)
    {
        return new RobotState { Location = location, Direction = direction };
    }
}

public static class RobotStateBehaviour
{
    public static RobotState Move(this RobotState robotState, Movement movement)
    {
        return movement switch
        {
            Movement.Forward => robotState.Forward(),
            Movement.Right => robotState.Right(),
            Movement.Left => robotState.Left(),
            _ => robotState
        };
    }

    private static RobotState Forward(this RobotState robotState)
    {
        return (robotState.Direction switch
        {
            Direction.North => robotState with
            {
                Location = robotState.Location with { Vertical = robotState.Location.Vertical + 1 }
            },
            Direction.East => robotState with
            {
                Location = robotState.Location with { Horizontal = robotState.Location.Horizontal + 1 }
            },
            Direction.South => robotState with
            {
                Location = robotState.Location with { Vertical = robotState.Location.Vertical - 1 }
            },
            Direction.West => robotState with
            {
                Location = robotState.Location with { Horizontal = robotState.Location.Horizontal - 1 }
            },
            _ => robotState
        });
    }
    
    private static RobotState Right(this RobotState robotState)
    {
        return (robotState.Direction switch
        {
            Direction.North => robotState with { Direction = Direction.East },
            Direction.East => robotState with { Direction = Direction.South },
            Direction.South => robotState with { Direction = Direction.West },
            Direction.West => robotState with { Direction = Direction.North },
            _ => robotState
        });
    }
    
    private static RobotState Left(this RobotState robotState)
    {
        return (robotState.Direction switch
        {
            Direction.North => robotState with { Direction = Direction.West },
            Direction.East => robotState with { Direction = Direction.North },
            Direction.South => robotState with { Direction = Direction.East },
            Direction.West => robotState with { Direction = Direction.South },
            _ => robotState
        });
    }
}