using Domain.Primitives;
using FunctionalProgrammingKit;

namespace Domain.Entities;

public record RobotApplicationState
{
    public GridDimensions GridDimensions { get; private init; }
    public RobotState RobotState { get; private init; }

    public static Entity<RobotApplicationState> Create(GridDimensions gridDimensions)
    {
        return Entity<RobotApplicationState>.Valid(new RobotApplicationState { GridDimensions = gridDimensions });
    }



    public static Entity<RobotApplicationState> Create(ValueObject<GridDimensions> size)
    {
        return size.Match(Entity<RobotApplicationState>.Invalid, validSize => Entity<RobotApplicationState>.Valid(new RobotApplicationState { GridDimensions = validSize }));
    }
}