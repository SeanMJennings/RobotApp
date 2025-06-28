using Domain.Primitives;
using FunctionalProgrammingKit;

namespace Domain.Entities;

public record GameState
{
    public Size Size { get; init; }

    public static Entity<GameState> Create(Size size)
    {
        return Entity<GameState>.Valid(new GameState { Size = size });
    }
}