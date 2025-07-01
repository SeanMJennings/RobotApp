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
    private const string LateObstaclesText = """
                                               GRID 4X2
                                               1 1 E
                                               RFR
                                               1 0 N
                                               OBSTACLE 1 1
                                               """;    
    private const string CollisionInstructions = """
                                               GRID 4X2
                                               OBSTACLE 1 1
                                               1 0 N
                                               F
                                               1 1 N
                                               """;
    
    private static string a_non_existent_file()
    {
        return UnknownFile;
    }

    private static Result<string[]> looking_for_file(string path)
    {
        return path.ReadAllLines();
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

    private static Result<string[]> a_known_file_with_obstacles_declared_late_has_been_read()
    {
        return new Result<string[]>(LateObstaclesText.Split('\n'));
    }     
    
    private static Result<string[]> a_known_file_with_a_collision_has_been_read()
    {
        return new Result<string[]>(CollisionInstructions.Split('\n'));
    }    
    
    private static Result<string[]> sample_file_has_been_read()
    {
        return SampleFile.ReadAllLines();
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
            Assert.That(gameState.ErrorMessage().Contains("Starting location is invalid. Expected format: <x> <y> <direction> where direction is N, E, S, W"), Is.True);
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
            Assert.That(gameState.ErrorMessage(), Is.EqualTo("Ending location is invalid. Expected format: <x> <y> <direction> where direction is N, E, S, W"));
        });
    }    
    
    private static void obstacles_are_invalid_as_they_are_late(Entity<RobotApplicationState> gameState)
    {
        Assert.Multiple(() =>
        {
            Assert.That(gameState.IsValid, Is.EqualTo(false));
            Assert.That(gameState.ErrorMessage(), Is.EqualTo("At least one obstacle is invalid. Obstacles must be declared after grid size and before robot instructions in format: OBSTACLE <x> <y>"));
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
    
    private static void crash_is_calculated(Result<RobotInstructionsResult[]> result)
    {
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.EqualTo(true));
            Assert.That(result.Data[0].RobotInstructionsResultType, Is.EqualTo(RobotInstructionsResultType.Crashed));
            Assert.That(result.Data[0].RobotState, Is.EqualTo(RobotState.Create(Location.Create(1, 1), Direction.North)));
        });
    }
}