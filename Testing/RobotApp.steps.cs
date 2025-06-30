using Common;
using Domain.Entities;
using Domain.Primitives;
using FunctionalProgrammingKit;

namespace Testing;

[TestFixture]
public static partial class RobotAppShould
{
    private const string UnknownFile = "unknown_file.txt";
    private const string SampleFile = "Sample.txt";
    private const string Sample1File = "Sample1.txt";
    private const string InvalidGridSizeText = "GRID 43";
    private const string InvalidLocationText = """
                                               GRID 4X3
                                               1 1
                                               RFR
                                               1 0 W
                                               """;    
    private const string InvalidMovementsText = """
                                               GRID 4X2
                                               1 1 E
                                               RFRT
                                               1 0 W
                                               """;    
    private const string InvalidEndingLocationText = """
                                               GRID 4X2
                                               1 1 E
                                               RFR
                                               1 W
                                               """;
    
    private static string a_non_existent_file()
    {
        return UnknownFile;
    }

    private static Result<string[]> looking_for_file(string path)
    {
        return path.GetCleanedFileContents();
    }

    private static void file_is_not_found(Result<string[]> result)
    {
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.EqualTo(false));
            Assert.That(result.ErrorMessage(), Is.EqualTo($"File not found: {UnknownFile}"));
        });
    }

    private static string a_known_file()
    {
        return SampleFile;
    }
    
    private static Result<string[]> a_known_file_with_invalid_grid_size_has_been_read()
    {
        return new Result<string[]>([InvalidGridSizeText]);
    }

    private static Result<string[]> a_known_file_with_invalid_starting_location_has_been_read()
    {
        return new Result<string[]>(InvalidLocationText.Split('\n'));
    }
    
    private static Result<string[]> a_known_file_with_invalid_movements_has_been_read()
    {
        return new Result<string[]>(InvalidMovementsText.Split('\n'));
    }    
    
    private static Result<string[]> a_known_file_with_invalid_ending_location_has_been_read()
    {
        return new Result<string[]>(InvalidEndingLocationText.Split('\n'));
    }

    private static Result<string[]> sample_file_has_been_read()
    {
        return SampleFile.GetCleanedFileContents();
    }

    private static Result<string[]> sample_1_file_has_been_read()
    {
        return Sample1File.GetCleanedFileContents();
    }

    private static void file_is_found(Result<string[]> result)
    {
        assert_success(result, FileExtensions.ReadAllLinesThatAreNotEmpty(SampleFile));
    }
    
    private static void assert_success<T>(this Result<T> result, T expected)
    {
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.EqualTo(true));
            Assert.That(result.Data, Is.EqualTo(expected));
        });
    }

    private static Entity<RobotApplicationState> parsing(Result<string[]> fileContents)
    {
        return fileContents.ParseRobotInstructions();
    }

    private static Result<RobotInstructionsResult[]> calculating_instruction_results_for_sample_file(Result<string[]> fileContents)
    {
        return fileContents.ParseRobotInstructions().ExecuteRobotInstructions();
    }
    
    private static void grid_size_is_invalid(Entity<RobotApplicationState> gameState)
    {
        Assert.Multiple(() =>
        {
            Assert.That(gameState.IsValid, Is.EqualTo(false));
            Assert.That(gameState.ErrorMessage(), Is.EqualTo("Grid size is invalid. Expected format: GRID <width>x<height>"));
        });
    }

    private static void starting_location_is_invalid(Entity<RobotApplicationState> gameState)
    {
        Assert.Multiple(() =>
        {
            Assert.That(gameState.IsValid, Is.EqualTo(false));
            Assert.That(gameState.ErrorMessage(), Is.EqualTo("Starting location is invalid. Expected format: <x> <y> <direction> where direction is N, E, S, W>"));
        });
    }
    
    private static void movements_are_invalid(Entity<RobotApplicationState> gameState)
    {
        Assert.Multiple(() =>
        {
            Assert.That(gameState.IsValid, Is.EqualTo(false));
            Assert.That(gameState.ErrorMessage(), Is.EqualTo("Instructions are invalid. Expected format: <L|R|F> (e.g., LRF)"));
        });
    }
    
    private static void ending_location_is_invalid(Entity<RobotApplicationState> gameState)
    {
        Assert.Multiple(() =>
        {
            Assert.That(gameState.IsValid, Is.EqualTo(false));
            Assert.That(gameState.ErrorMessage(), Is.EqualTo("Ending location is invalid. Expected format: <x> <y> <direction> where direction is N, E, S, W>"));
        });
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
    
    private static Result<RobotInstructionsResult[]> calculating_instruction_results_for_sample_1_file(Result<string[]> fileContents)
    {
        return fileContents.ParseRobotInstructions().ExecuteRobotInstructions();
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
}