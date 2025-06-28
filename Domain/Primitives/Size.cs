namespace Domain.Primitives;

public readonly struct Size(uint width, uint height)
{
    public uint Width { get; } = width;
    public uint Height { get; } = height;
}