using Domain.Primitives;
using FunctionalProgrammingKit;

namespace Domain.Entities;

public record RobotInstructionsWithResult
{
    public RobotState StartingRobotState { get; init; }
    public RobotState FinalRobotState { get; init; }
    public Direction[] Instructions { get; init; }

    public static Entity<RobotInstructionsWithResult> Create(RobotState startingRobotState, RobotState finalRobotState, Direction[] instructions)
    {
        return Entity<RobotInstructionsWithResult>.Valid(new RobotInstructionsWithResult
        {
            StartingRobotState = startingRobotState,
            FinalRobotState = finalRobotState,
            Instructions = instructions
        });
    }
    
    public static Entity<RobotInstructionsWithResult> Create(ValueObject<RobotState> startingRobotState, ValueObject<RobotState> finalRobotState, ValueObject<Direction[]> instructions)
    {
        return startingRobotState.Match(
            Entity<RobotInstructionsWithResult>.Invalid,
            validStartingState => finalRobotState.Match(
                Entity<RobotInstructionsWithResult>.Invalid,
                validFinalState => instructions.Match(
                    Entity<RobotInstructionsWithResult>.Invalid,
                    validInstructions => Entity<RobotInstructionsWithResult>.Valid(new RobotInstructionsWithResult
                    {
                        StartingRobotState = validStartingState,
                        FinalRobotState = validFinalState,
                        Instructions = validInstructions
                    }))));
    }
}