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

public static class RobotApplicationStateBehaviour
{
    public static Result<RobotInstructionsResult[]> ExecuteRobotInstructions(this Entity<RobotApplicationState> robotApplicationState)
    {
        return robotApplicationState.MatchResult<RobotInstructionsResult[]>(state => ExecuteRobotInstructions(state.InstructionsForRobots, 0));
    }
    
    private static RobotInstructionsResult[] ExecuteRobotInstructions(RobotInstructions[] instructions, int index)
    {
        if (index >= instructions.Length) return [];

        var robotInstructions = instructions[index].ExecuteRobotInstructions();
        var remainingInstructions = ExecuteRobotInstructions(instructions, index + 1);
        var results = new RobotInstructionsResult[remainingInstructions.Length + 1];
        results[0] = robotInstructions;
        remainingInstructions.CopyTo(results, 1);
        return results;
    }
}