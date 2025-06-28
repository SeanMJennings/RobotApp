namespace FunctionalProgrammingKit;

public static class Lift
{
    public static ValueObject<U> Bind<T, U>(this ValueObject<T> value, Func<T, ValueObject<U>> func)
    {
        return value.Match(Valid: func, Invalid: Invalid<U>);
    }
    
    public static ValueObject<U> Map<T, U>(this ValueObject<T> value, Func<T, U> func)
    {
        return value.Match(Valid: v => ValueObject.Valid(func(v)), Invalid: Invalid<U>);
    }
    
    public static Entity<U> Bind<T, U>(this Entity<T> entity, Func<T, Entity<U>> func)
    {
        return entity.Match(Valid: func, Invalid: Entity.Invalid<U>);
    }
    
    public static Entity<U> Map<T, U>(this Entity<T> entity, Func<T, U> func)
    {
        return entity.Match(Valid: v => Entity.Valid(func(v)), Invalid: Invalid<U>);
    }
}