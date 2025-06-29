using System.Text.RegularExpressions;
using Domain.Entities;
using Domain.Primitives;
using FunctionalProgrammingKit;

namespace Application;

public static class Parser
{
    private const string GridKeyword = "GRID";
    private const char SizeSeparator = 'X';
    private const string ValidGridDimensionsPattern = @"^\d+X\d+$";
    private const string ValidLocationPattern = "^[0-9]{2}[ENSW]$";
    private const string ValidInstructionsPattern = "^[LRF]+$";
    
    public static Entity<RobotApplicationState> ParseRobotInstructions(this Result<string[]> fileContents) 
    {
        if (fileContents.Failed) return Entity<RobotApplicationState>.Invalid(fileContents.Errors);
        var gridDimensions = ParseGridDimensions(fileContents.Data);
        var instructions = ParseRobotInstructions(fileContents.Data, 1);
        return RobotApplicationState.Create(gridDimensions, instructions);
    }

    private static ValueObject<GridDimensions> ParseGridDimensions(string[] fileContents)
    {
        var cleanedGridSize = fileContents[0].CleanFileLine().Split([GridKeyword], StringSplitOptions.None)[1].Trim();
        
        if (!Regex.IsMatch(cleanedGridSize, ValidGridDimensionsPattern)) return Invalid<GridDimensions>("Grid size is invalid. Expected format: GRID <width>x<height>");

        var dimensions = cleanedGridSize.Split(SizeSeparator);
        return Valid(GridDimensions.Create(uint.Parse(dimensions[0]), uint.Parse(dimensions[1])));
    }

    private static ValueObject<RobotInstructions[]> ParseRobotInstructions(string[] fileContents, int index)
    {
        if (index >= fileContents.Length) return Valid<RobotInstructions[]>([]);

        List<string> currentBatch = [];
        for (var i = index; i < index + 3 && i < fileContents.Length; i++) currentBatch.Add(fileContents[i]);
        
        var robotInstructions = ParseRobotInstructions(currentBatch.ToArray());
        var remainingInstructions = ParseRobotInstructions(fileContents, index + 3);
        
        return robotInstructions.Match(
            Invalid: Invalid<RobotInstructions[]>,
            Valid: instructions =>
            {
                var nextInstructions = remainingInstructions.Match(
                    Invalid: _ => [],
                    Valid: next => next);
                return Valid(new[] { instructions }.Concat(nextInstructions).ToArray());
            });
    }
    
    private static ValueObject<RobotInstructions> ParseRobotInstructions(string[] fileContents)
    {
        var cleanedStartingLocation = fileContents[0].CleanFileLine();
        var cleanedInstructions = fileContents[1].CleanFileLine();
        var cleanedEndLocation = fileContents[2].CleanFileLine();
        if (!Regex.IsMatch(cleanedStartingLocation, ValidLocationPattern))
            return Invalid<RobotInstructions>("Starting location is invalid. Expected format: <x> <y> <direction> where direction is N, E, S, W>");
        if (!Regex.IsMatch(cleanedInstructions, ValidInstructionsPattern))
            return Invalid<RobotInstructions>("Instructions are invalid. Expected format: <L|R|F> (e.g., LRF)");
        if (!Regex.IsMatch(cleanedEndLocation, ValidLocationPattern))
            return Invalid<RobotInstructions>("Ending location is invalid. Expected format: <x> <y> <direction> where direction is N, E, S, W>");
        var startingRobotState = RobotState.Create(
            Location.Create(uint.Parse([cleanedStartingLocation[0]]), uint.Parse([cleanedStartingLocation[1]])),
            cleanedStartingLocation[2].ToDirection());
        var instructions = cleanedInstructions.ToMovements();
        var finalRobotState = RobotState.Create(
            Location.Create(uint.Parse([cleanedEndLocation[0]]), uint.Parse([cleanedEndLocation[1]])),
            cleanedEndLocation[2].ToDirection());
        return Valid(RobotInstructions.Create(startingRobotState, finalRobotState, instructions));
    }
}

public static class FileContentExtensions
{
    public static string CleanFileLine(this string line)
    {
        return line.Replace(" ", string.Empty).Replace("\r", string.Empty).ToUpperInvariant();
    }
}