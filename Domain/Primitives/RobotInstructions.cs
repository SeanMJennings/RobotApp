namespace Domain.Primitives;

public record RobotInstructions
{
    public required RobotState StartingRobotState { get; init; }
    public required RobotState FinalRobotState { get; init; }
    public required Movement[] Instructions { get; init; }

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
    public static RobotInstructionsResult ExecuteRobotInstructions(this RobotInstructions robotInstructions, GridDimensions gridDimensions)
    {
        var currentState = robotInstructions.StartingRobotState;
        foreach (var movement in robotInstructions.Instructions)
        {
            var (newState, outOfBounds) = currentState.Move(movement, gridDimensions);
            if (outOfBounds)
            {
                return RobotInstructionsResult.Create(RobotInstructionsResultType.OutOfBounds, currentState);
            }
            currentState = newState;
        }
        return RobotInstructionsResult.Create(LastStateDoesNotMatchExpectedFinalState(robotInstructions, currentState) ? RobotInstructionsResultType.Failure : RobotInstructionsResultType.Success, currentState);
    }

    private static bool LastStateDoesNotMatchExpectedFinalState(RobotInstructions robotInstructions, RobotState currentState)
    {
        return currentState.Location != robotInstructions.FinalRobotState.Location || currentState.Direction != robotInstructions.FinalRobotState.Direction;
    }
}