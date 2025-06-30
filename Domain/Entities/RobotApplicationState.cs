using Domain.Primitives;
using FunctionalProgrammingKit;

namespace Domain.Entities;

public record RobotApplicationState
{
    public GridDimensions GridDimensions { get; init; }
    public RobotInstructions[] InstructionsForRobots { get; init; } = null!;
    public Obstacle[] ObstaclesForRobots { get; init; } = null!;

    public static Entity<RobotApplicationState> Create(ValueObject<GridDimensions> gridDimensions, ValueObject<RobotInstructions[]> instructions, ValueObject<Obstacle[]> obstacles)
    {
        if (gridDimensions.IsValid && instructions.IsValid && obstacles.IsValid)
            return Entity.Valid(new RobotApplicationState())
                .SetGridDimensions(gridDimensions)
                .SetInstructionsForRobots(instructions)
                .SetObstaclesForRobots(obstacles);

        return CreateInvalid(gridDimensions, instructions, obstacles);
    }

    private static Entity<RobotApplicationState> CreateInvalid(ValueObject<GridDimensions> gridDimensions, ValueObject<RobotInstructions[]> instructions, ValueObject<Obstacle[]> obstacles)
    {
        var gridErrors = gridDimensions.Match(
            Invalid: errors => errors,
            Valid: _ => []);
        var instructionsErrors = instructions.Match(
            Invalid: errors => errors,
            Valid: _ => []);
        var obstaclesErrors = obstacles.Match(
            Invalid: errors => errors,
            Valid: _ => []);
        
        return Entity.Invalid<RobotApplicationState>(gridErrors.Concat(instructionsErrors).Concat(obstaclesErrors).ToArray());
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
    
    public static Entity<RobotApplicationState> SetObstaclesForRobots(this Entity<RobotApplicationState> robotApplicationState, ValueObject<Obstacle[]> obstacles)
    {
        return robotApplicationState.SetValueObject(obstacles, static (robotApplicationState, theObstacles) => robotApplicationState with { ObstaclesForRobots = theObstacles });
    }
}

public static class RobotApplicationStateBehaviour
{
    public static Result<RobotInstructionsResult[]> ExecuteRobotInstructions(this Entity<RobotApplicationState> robotApplicationState)
    {
        return robotApplicationState.MatchResult<RobotInstructionsResult[]>(state => ExecuteRobotInstructions(state.InstructionsForRobots, 0, state.GridDimensions, state.ObstaclesForRobots));
    }
    
    private static RobotInstructionsResult[] ExecuteRobotInstructions(RobotInstructions[] instructions, int index, GridDimensions gridDimensions, Obstacle[] obstacles)
    {
        if (index >= instructions.Length) return [];

        var robotInstructions = instructions[index].ExecuteRobotInstructions(gridDimensions, obstacles);
        var remainingInstructions = ExecuteRobotInstructions(instructions, index + 1, gridDimensions, obstacles);
        var results = new RobotInstructionsResult[remainingInstructions.Length + 1];
        results[0] = robotInstructions;
        remainingInstructions.CopyTo(results, 1);
        return results;
    }
}