using Domain.Primitives;
using FunctionalProgrammingKit;

namespace Domain.Entities;

public record GameState
{
    public Size Size { get; private init; }

    public static Entity<GameState> Create(Size size)
    {
        return Entity<GameState>.Valid(new GameState { Size = size });
    }

    public static Entity<GameState> Create(ValueObject<Size> size)
    {
        return size.Match(Entity<GameState>.Invalid, validSize => Entity<GameState>.Valid(new GameState { Size = validSize }));
    }
}