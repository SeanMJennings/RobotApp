namespace Domain.Primitives;

public readonly struct Location(uint horizontal, uint vertical)
{
    public uint Horizontal { get; } = horizontal;
    public uint Vertical { get; } = vertical;

    public static Location Create(uint horizontal, uint vertical)
    {
        return new Location(horizontal, vertical);
    }
}