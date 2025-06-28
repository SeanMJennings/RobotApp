namespace Domain.Primitives;

public readonly struct GridDimensions(uint width, uint height)
{
    public uint Width { get; } = width;
    public uint Height { get; } = height;
    public static GridDimensions Create(uint width, uint height)
    {
        return new GridDimensions(width, height);
    }
}