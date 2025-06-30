namespace Domain.Primitives;

public enum Direction
{
    North,
    South,
    East,
    West
}

public static class DirectionExtensions
{
    public static Direction ToDirection(this char direction)
    {
        return direction switch
        {
            'N' => Direction.North,
            'S' => Direction.South,
            'E' => Direction.East,
            'W' => Direction.West
        };
    }
}