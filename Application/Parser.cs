using Domain.Entities;
using Domain.Primitives;
using FunctionalProgrammingKit;

namespace Application;

public static class Parser
{
    private const string GridKeyword = "GRID";
    private const char SizeSeparator = 'X';
    public static Entity<GameState> Parse(this Result<string[]> fileContents) 
    {
        return fileContents.ToEntity().Match<Entity<GameState>>(Entity<GameState>.Invalid, content => GameState.Create(ParseGridSize(content)));
    }

    private static Size ParseGridSize(string[] fileContents)
    {
        var size = fileContents[0].Replace(" ", string.Empty).ToUpperInvariant().Split([GridKeyword], StringSplitOptions.None)[1].Trim();
        var dimensions = size.Split(SizeSeparator);
        return new Size( uint.Parse(dimensions[0]), uint.Parse(dimensions[1]));
    }
}