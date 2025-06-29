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
            Assert.That(result!.Success, Is.EqualTo(false));
            Assert.That(result!.ErrorMessage(), Is.EqualTo($"File not found: {UnknownFile}"));
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

    private static Result<string[]> a_known_file_has_been_read()
    {
        return a_known_file().GetCleanedFileContents();
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

    private static void grid_size_is_correct(Entity<RobotApplicationState> robotApplicationState)
    {
        Assert.Multiple(() =>
        {
            Assert.That(robotApplicationState.IsValid, Is.EqualTo(true));
            Assert.That(robotApplicationState.Match().Value.GridDimensions, Is.EqualTo(GridDimensions.Create(4,3)));
        });
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
}