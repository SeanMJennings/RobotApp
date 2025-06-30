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

public static class RobotInstructionsBehaviour
{
    public static RobotInstructionsResult ExecuteRobotInstructions(this RobotInstructions robotInstructions)
    {
        var currentState = robotInstructions.Instructions.Aggregate(robotInstructions.StartingRobotState, (current, instruction) => current.Move(instruction));
        return RobotInstructionsResult.Create(RobotInstructionsResultType.Success, currentState);
    }
}