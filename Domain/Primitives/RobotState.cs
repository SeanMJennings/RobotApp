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
    public static (RobotState RobotState, bool OutOfBounds, bool crashed) Move(this RobotState robotState, Movement movement, GridDimensions gridDimensions, Obstacle[] obstacles)
    {
        return movement switch
        {
            Movement.Forward => robotState.Forward(gridDimensions, obstacles),
            Movement.Right => robotState.Right(gridDimensions, obstacles),
            Movement.Left => robotState.Left(gridDimensions, obstacles),
        };
    }

    private static (RobotState RobotState, bool OutOfBounds, bool crashed) Forward(this RobotState robotState, GridDimensions gridDimensions, Obstacle[] obstacles)
    {
        var newRobotState = robotState.Direction switch
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
        };
        var outOfBounds = IsRobotOutOfBounds(gridDimensions, newRobotState);
        if (outOfBounds) return (newRobotState, true, false);
        var hasRobotCrashed = HasRobotCrashedIntoObstacle(newRobotState, obstacles);
        return hasRobotCrashed ? (newRobotState, false, true) : (newRobotState, false, false);
    }

    private static bool IsRobotOutOfBounds(GridDimensions gridDimensions, RobotState newRobotState)
    {
        return newRobotState.Location.Horizontal > gridDimensions.Width - 1 || newRobotState.Location.Vertical > gridDimensions.Height - 1;
    }
    
    private static bool HasRobotCrashedIntoObstacle(RobotState newRobotState, IEnumerable<Obstacle> obstacles)
    {
        return obstacles.Any(obstacle => obstacle.Location == newRobotState.Location);
    }

    private static (RobotState RobotState, bool OutOfBounds, bool crashed) Right(this RobotState robotState, GridDimensions gridDimensions, Obstacle[] obstacles)
    {
        var newRobotState =  robotState.Direction switch
        {
            Direction.North => robotState with { Direction = Direction.East },
            Direction.East => robotState with { Direction = Direction.South },
            Direction.South => robotState with { Direction = Direction.West },
            Direction.West => robotState with { Direction = Direction.North },
            _ => robotState
        };
        var outOfBounds = IsRobotOutOfBounds(gridDimensions, newRobotState);
        if (outOfBounds) return (newRobotState, true, false);
        var hasRobotCrashed = HasRobotCrashedIntoObstacle(newRobotState, obstacles);
        return hasRobotCrashed ? (newRobotState, false, true) : (newRobotState, false, false);
    }
    
    private static (RobotState RobotState, bool OutOfBounds, bool crashed) Left(this RobotState robotState, GridDimensions gridDimensions, Obstacle[] obstacles)
    {
        var newRobotState =  robotState.Direction switch
        {
            Direction.North => robotState with { Direction = Direction.West },
            Direction.East => robotState with { Direction = Direction.North },
            Direction.South => robotState with { Direction = Direction.East },
            Direction.West => robotState with { Direction = Direction.South },
            _ => robotState
        };
        var outOfBounds = IsRobotOutOfBounds(gridDimensions, newRobotState);
        if (outOfBounds) return (newRobotState, true, false);
        var hasRobotCrashed = HasRobotCrashedIntoObstacle(newRobotState, obstacles);
        return hasRobotCrashed ? (newRobotState, false, true) : (newRobotState, false, false);
    }
}