using System.Text.RegularExpressions;
using Domain.Entities;
using Domain.Primitives;
using FunctionalProgrammingKit;

namespace Application;

public static class Parser
{
    private const string GridKeyword = "GRID";
    private const char SizeSeparator = 'X';
    private const string GridPattern = @"^\d+X\d+$";
    
    public static Entity<GameState> Parse(this Result<string[]> fileContents) 
    {
        if (fileContents.Failed) return Entity<GameState>.Invalid(fileContents.Errors);
        var size = ParseGridSize(fileContents.Data);
        return fileContents.ToEntity().Match<Entity<GameState>>(Entity<GameState>.Invalid, content => GameState.Create(ParseGridSize(content)));
    }

    private static ValueObject<Size> ParseGridSize(string[] fileContents)
    {
        var cleanedGridSize = fileContents[0].Replace(" ", string.Empty).ToUpperInvariant().Split([GridKeyword], StringSplitOptions.None)[1].Trim();
        
        if (!Regex.IsMatch(cleanedGridSize, GridPattern)) return Invalid<Size>("Grid size is invalid. Expected format: GRID <width>x<height>");

        var dimensions = cleanedGridSize.Split(SizeSeparator);
        return Valid(new Size(uint.Parse(dimensions[0]), uint.Parse(dimensions[1])));
    }
}