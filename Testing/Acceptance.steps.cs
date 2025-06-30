using Domain.Entities;
using Domain.Primitives;
using FunctionalProgrammingKit;

namespace Testing;

[TestFixture]
public static partial class Acceptance
{
    private const string SampleFile = "Sample.txt";
    private const string Sample1File = "Sample1.txt";
    private const string Sample2File = "Sample2.txt";
    
    private static Result<string[]> sample_file_has_been_read()
    {
        return SampleFile.GetCleanedFileContents();
    }

    private static Result<string[]> sample_1_file_has_been_read()
    {
        return Sample1File.GetCleanedFileContents();
    }    
    
    private static Result<string[]> sample_2_file_has_been_read()
    {
        return Sample2File.GetCleanedFileContents();
    }
    
    private static Result<RobotInstructionsResult[]> calculating_instruction_results(Result<string[]> fileContents)
    {
        return fileContents.ParseRobotInstructions().ExecuteRobotInstructions();
    }
    
    private static Result<RobotInstructionsResult[]> success_is_calculated_for_first_robot_in_sample(Result<RobotInstructionsResult[]> result)
    {
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.EqualTo(true));
            Assert.That(result.Data[0].RobotInstructionsResultType, Is.EqualTo(RobotInstructionsResultType.Success));
            Assert.That(result.Data[0].RobotState, Is.EqualTo(RobotState.Create(Location.Create(1, 0), Direction.West)));
        });
        return result;
    }
    
    private static Result<RobotInstructionsResult[]> failure_is_calculated_for_second_robot_in_sample(Result<RobotInstructionsResult[]> result)
    {
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.EqualTo(true));
            Assert.That(result.Data[1].RobotInstructionsResultType, Is.EqualTo(RobotInstructionsResultType.Failure));
            Assert.That(result.Data[1].RobotState, Is.EqualTo(RobotState.Create(Location.Create(0, 0), Direction.West)));
        });
        return result;
    }    
    
    private static Result<RobotInstructionsResult[]> out_of_bounds_is_calculated_for_third_robot_in_sample(Result<RobotInstructionsResult[]> result)
    {
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.EqualTo(true));
            Assert.That(result.Data[2].RobotInstructionsResultType, Is.EqualTo(RobotInstructionsResultType.OutOfBounds));
        });
        return result;
    }
    
    private static Result<RobotInstructionsResult[]> first_robot_in_sample_1_file_is_successful(Result<RobotInstructionsResult[]> result)
    {
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.EqualTo(true));
            Assert.That(result.Data[0].RobotInstructionsResultType, Is.EqualTo(RobotInstructionsResultType.Success));
            Assert.That(result.Data[0].RobotState, Is.EqualTo(RobotState.Create(Location.Create(1, 1), Direction.East)));
        });
        return result;
    }    
    
    private static Result<RobotInstructionsResult[]> first_robot_in_sample_2_file_is_successful(Result<RobotInstructionsResult[]> result)
    {
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.EqualTo(true));
            Assert.That(result.Data[0].RobotInstructionsResultType, Is.EqualTo(RobotInstructionsResultType.Success));
            Assert.That(result.Data[0].RobotState, Is.EqualTo(RobotState.Create(Location.Create(1, 1), Direction.East)));
        });
        return result;
    }    
    
    private static Result<RobotInstructionsResult[]> second_robot_in_sample_1_file_is_successful(Result<RobotInstructionsResult[]> result)
    {
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.EqualTo(true));
            Assert.That(result.Data[1].RobotInstructionsResultType, Is.EqualTo(RobotInstructionsResultType.Success));
            Assert.That(result.Data[1].RobotState, Is.EqualTo(RobotState.Create(Location.Create(3, 3), Direction.North)));
        });
        return result;
    }    
    
    private static Result<RobotInstructionsResult[]> second_robot_in_sample_2_file_is_successful(Result<RobotInstructionsResult[]> result)
    {
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.EqualTo(true));
            Assert.That(result.Data[1].RobotInstructionsResultType, Is.EqualTo(RobotInstructionsResultType.Success));
            Assert.That(result.Data[1].RobotState, Is.EqualTo(RobotState.Create(Location.Create(3, 3), Direction.North)));
        });
        return result;
    }    
    
    private static Result<RobotInstructionsResult[]> third_robot_in_sample_1_file_is_successful(Result<RobotInstructionsResult[]> result)
    {
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.EqualTo(true));
            Assert.That(result.Data[2].RobotInstructionsResultType, Is.EqualTo(RobotInstructionsResultType.Success));
            Assert.That(result.Data[2].RobotState, Is.EqualTo(RobotState.Create(Location.Create(2, 4), Direction.South)));
        });
        return result;
    }    
    
    private static Result<RobotInstructionsResult[]> third_robot_in_sample_2_file_is_crashed(Result<RobotInstructionsResult[]> result)
    {
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.EqualTo(true));
            Assert.That(result.Data[2].RobotInstructionsResultType, Is.EqualTo(RobotInstructionsResultType.Crashed));
            Assert.That(result.Data[2].RobotState, Is.EqualTo(RobotState.Create(Location.Create(1, 3), Direction.East)));
        });
        return result;
    }
}