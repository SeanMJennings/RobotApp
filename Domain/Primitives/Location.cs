namespace Domain.Primitives;

public readonly record struct Location(uint Horizontal, uint Vertical)
{
    public static Location Create(uint horizontal, uint vertical)
    {
        return new Location(horizontal, vertical);
    }
}