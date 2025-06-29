namespace Domain.Primitives;

public enum Movement
{
    Left,
    Right,
    Forward,
}

public static class MovementExtensions
{
    public static Movement[] ToMovements(this string movements)
    {
        if (string.IsNullOrWhiteSpace(movements)) return [];

        return movements
            .ToUpperInvariant()
            .Select(c => c switch
            {
                'L' => Movement.Left,
                'R' => Movement.Right,
                'F' => Movement.Forward
            })
            .ToArray();
    }
}