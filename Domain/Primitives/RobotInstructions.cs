using Domain.Entities;
using FunctionalProgrammingKit;

namespace Domain.Primitives;

public record RobotInstructions
{
    public RobotState StartingRobotState { get; init; }
    public RobotState FinalRobotState { get; init; }
    public Movement[] Instructions { get; init; }

    public static RobotInstructions Create(RobotState startingRobotState, RobotState finalRobotState, Movement[] instructions)
    {
        return new RobotInstructions
        {
            StartingRobotState = startingRobotState,
            FinalRobotState = finalRobotState,
            Instructions = instructions
        };
    }
}

public static class RobotInstructionsExtensions
{
    public static ValueObject<RobotInstructions[]> Create(this RobotInstructions[] robotInstructions)
    {
        return Valid(robotInstructions);
    }  
}