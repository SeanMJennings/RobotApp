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
    
    private static void assert_success<T>(this Entity<T> result, T expected)
    {
        Assert.Multiple(() =>
        {
            Assert.That(result.IsValid, Is.EqualTo(true));
            Assert.That(result.Match().Value, Is.EqualTo(expected));
        });
    }

    private static Entity<GameState> parsing_grid_size(Result<string[]> fileContents)
    {
        return fileContents.Parse();
    }

    private static void grid_size_is_correct(Entity<GameState> gameState)
    {
        Assert.Multiple(() =>
        {
            Assert.That(gameState.IsValid, Is.EqualTo(true));
            Assert.That(gameState.Match().Value.Size, Is.EqualTo(new Size(4, 3)));
        });
    }
    
    private static void grid_size_is_invalid(Entity<GameState> gameState)
    {
        Assert.Multiple(() =>
        {
            Assert.That(gameState.IsValid, Is.EqualTo(false));
            Assert.That(gameState.ErrorMessage(), Is.EqualTo("Grid size is invalid. Expected format: GRID <width>x<height>"));
        });
    }
}