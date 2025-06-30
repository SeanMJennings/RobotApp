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
        if (currentState.Location != robotInstructions.FinalRobotState.Location || currentState.Direction != robotInstructions.FinalRobotState.Direction)
        {
            return RobotInstructionsResult.Create(RobotInstructionsResultType.Failure, currentState);
        }
        return RobotInstructionsResult.Create(RobotInstructionsResultType.Success, currentState);
    }
}