using FunctionalProgrammingKit;

namespace Domain.Primitives;

public readonly record struct GridDimensions(uint Width, uint Height)
{
    public static GridDimensions Create(uint width, uint height)
    {
        return new GridDimensions(width, height);
    }
}

public static class GridDimensionsExtensions
{
    public static ValueObject<GridDimensions> Create(this GridDimensions gridDimensions)
    {
        return Valid(gridDimensions);
    }  
}