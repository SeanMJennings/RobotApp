using Domain.Primitives;
using FunctionalProgrammingKit;

namespace Domain.Entities;

public record RobotApplicationState
{
    public GridDimensions GridDimensions { get; init; }
    public RobotInstructions[] InstructionsForRobots { get; init; } = null!;

    public static Entity<RobotApplicationState> Create(ValueObject<GridDimensions> gridDimensions, ValueObject<RobotInstructions[]> instructions)
    {
        return Entity.Valid(new RobotApplicationState())
            .SetGridDimensions(gridDimensions)
            .SetInstructionsForRobots(instructions);
    }
}

public static class RobotApplicationStateSetters
{
    public static Entity<RobotApplicationState> SetGridDimensions(this Entity<RobotApplicationState> robotApplicationState, ValueObject<GridDimensions> gridDimensions)
    {
        return robotApplicationState.SetValueObject(gridDimensions, static (robotApplicationState, theGridDimensions) => robotApplicationState with { GridDimensions = theGridDimensions });
    }
    
    public static Entity<RobotApplicationState> SetInstructionsForRobots(this Entity<RobotApplicationState> robotApplicationState, ValueObject<RobotInstructions[]> instructions)
    {
        return robotApplicationState.SetValueObject(instructions, static (robotApplicationState, theInstructions) => robotApplicationState with { InstructionsForRobots = theInstructions });
    }
}