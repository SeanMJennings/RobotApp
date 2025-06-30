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
    private const string ValidObstaclePattern = "^OBSTACLE[0-9][0-9]$";
    private const string ObstacleKeyword = "OBSTACLE";
    
    public static Entity<RobotApplicationState> ParseRobotInstructions(this Result<string[]> fileContents) 
    {
        if (fileContents.Failed) return Entity<RobotApplicationState>.Invalid(fileContents.Errors);
        var gridDimensions = ParseGridDimensions(fileContents.Data);
        var obstacles = ParseObstacles(fileContents.Data, 1);
        if (InvalidObstaclesArePresent(obstacles)) return RobotApplicationState.Create(gridDimensions, Invalid<RobotInstructions[]>([]), obstacles);
        var numberOfObstacles = obstacles.Match(Invalid: _ => 0, Valid: obs => obs.Length);
        var instructions = ParseAllRobotInstructions(fileContents.Data, 1 + numberOfObstacles);
        return RobotApplicationState.Create(gridDimensions, instructions, obstacles);
    }

    private static bool InvalidObstaclesArePresent(ValueObject<Obstacle[]> obstacles)
    {
        return !obstacles.IsValid;
    }

    private static ValueObject<GridDimensions> ParseGridDimensions(string[] fileContents)
    {
        var cleanedGridSize = fileContents[0].CleanFileLine().Split([GridKeyword], StringSplitOptions.None)[1].Trim();
        
        if (!Regex.IsMatch(cleanedGridSize, ValidGridDimensionsPattern)) return Invalid<GridDimensions>("Grid size is invalid. Expected format: GRID <width>x<height>");

        var dimensions = cleanedGridSize.Split(SizeSeparator);
        return Valid(GridDimensions.Create(uint.Parse(dimensions[0]), uint.Parse(dimensions[1])));
    }

    private static ValueObject<RobotInstructions[]> ParseAllRobotInstructions(string[] fileContents, int index)
    {
        if (index >= fileContents.Length) return Valid<RobotInstructions[]>([]);

        List<string> currentBatch = [];
        for (var i = index; i < index + 3 && i < fileContents.Length; i++) currentBatch.Add(fileContents[i]);
        
        var robotInstructions = ParseRobotInstructions(currentBatch.ToArray());
        var remainingInstructions = ParseAllRobotInstructions(fileContents, index + 3);
        
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
        if (!Regex.IsMatch(cleanedStartingLocation, ValidLocationPattern))
            return Invalid<RobotInstructions>("Starting location is invalid. Expected format: <x> <y> <direction> where direction is N, E, S, W>");
        
        var cleanedInstructions = fileContents[1].CleanFileLine();
        if (!Regex.IsMatch(cleanedInstructions, ValidInstructionsPattern))
            return Invalid<RobotInstructions>("Instructions are invalid. Expected format: <L|R|F> (e.g., LRF)");
        
        var cleanedEndLocation = fileContents[2].CleanFileLine();
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

    private static ValueObject<Obstacle[]> ParseObstacles(string[] fileContents, int index)
    {
        var obstacles = new List<Obstacle>();
        var instructionsHaveStarted = false;
        for (var i = index; i < fileContents.Length; i++)
        {
            var cleanedLine = fileContents[i].CleanFileLine();
            if (LineIsNeitherLocationOrInstructionOrObstacle(cleanedLine)) continue;

            if (LineIsLocationOrInstruction(cleanedLine))
            {
                instructionsHaveStarted = true;
                continue;
            }

            if (instructionsHaveStarted && LineIsObstacle(cleanedLine)) return Invalid<Obstacle[]>("At least one obstacle is invalid. Obstacles must be declared after grid size and before robot instructions in format: OBSTACLE <x> <y>");
            if (!LineIsObstacle(cleanedLine)) return Invalid<Obstacle[]>("Obstacle is invalid. Expected format: OBSTACLE <x> <y>");
            
            var row = uint.Parse(cleanedLine.Skip(ObstacleKeyword.Length).First().ToString());
            var column = uint.Parse(cleanedLine.Skip(ObstacleKeyword.Length).Skip(1).First().ToString());
            obstacles.Add(Obstacle.Create(Location.Create(row, column)));
        }
        return Valid(obstacles.ToArray());
    }

    private static bool LineIsNeitherLocationOrInstructionOrObstacle(string cleanedObstacle)
    {
        return !Regex.IsMatch(cleanedObstacle, ValidLocationPattern) 
               && !Regex.IsMatch(cleanedObstacle,ValidInstructionsPattern) 
               && !Regex.IsMatch(cleanedObstacle, ValidObstaclePattern);
    }    
    private static bool LineIsLocationOrInstruction(string cleanedObstacle)
    {
        return Regex.IsMatch(cleanedObstacle, ValidLocationPattern)
               || Regex.IsMatch(cleanedObstacle, ValidInstructionsPattern);
    }
    
    private static bool LineIsObstacle(string cleanedObstacle) => Regex.IsMatch(cleanedObstacle, ValidObstaclePattern);
}

public static class FileContentExtensions
{
    public static string CleanFileLine(this string line)
    {
        return line.Replace(" ", string.Empty).Replace("\r", string.Empty).ToUpperInvariant();
    }
}