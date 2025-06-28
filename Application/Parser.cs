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
    
    public static Entity<RobotApplicationState> ParseRobotInstructions(this Result<string[]> fileContents) 
    {
        if (fileContents.Failed) return Entity<RobotApplicationState>.Invalid(fileContents.Errors);
        var robotApplicationState = fileContents.ToEntity().Match<Entity<RobotApplicationState>>(Entity<RobotApplicationState>.Invalid, content => RobotApplicationState.Create(ParseGridDimensions(content)));
        return robotApplicationState;
    }

    private static ValueObject<GridDimensions> ParseGridDimensions(string[] fileContents)
    {
        var cleanedGridSize = fileContents[0].Replace(" ", string.Empty).ToUpperInvariant().Split([GridKeyword], StringSplitOptions.None)[1].Trim();
        
        if (!Regex.IsMatch(cleanedGridSize, GridPattern)) return Invalid<GridDimensions>("Grid size is invalid. Expected format: GRID <width>x<height>");

        var dimensions = cleanedGridSize.Split(SizeSeparator);
        return Valid(new GridDimensions(uint.Parse(dimensions[0]), uint.Parse(dimensions[1])));
    }
}