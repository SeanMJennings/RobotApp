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
    private const string InvalidLocationText = "1 1 Z";
    
    private static string a_non_existent_file()
    {
        return UnknownFile;
    }

    private static Result<string[]> looking_for_file(string path)
    {
        return path.GetFile();
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
        return new Result<string[]>([InvalidLocationText]);
    }

    private static Result<string[]> a_known_file_has_been_read()
    {
        return a_known_file().GetFile();
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
            Assert.That(gameState.ErrorMessage(), Is.EqualTo("Starting location is invalid. Expected format: <x> <y> <direction where direction is N, E, S, W>"));
        });
    }
}