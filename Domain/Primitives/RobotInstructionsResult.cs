namespace Domain.Primitives;

public record RobotInstructionsResult(RobotInstructionsResultType RobotInstructionsResultType, RobotState RobotState)
{
    public static RobotInstructionsResult Create(RobotInstructionsResultType RobotInstructionsResultType, RobotState RobotState)
    {
        return new RobotInstructionsResult(RobotInstructionsResultType, RobotState);
    }
    
}

public enum RobotInstructionsResultType
{
    Failure,
    Success,
}