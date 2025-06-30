namespace FunctionalProgrammingKit;

public record Entity<T>
{
    internal readonly IEnumerable<Error> _errors;
    private readonly T _value;

    public bool IsValid => !_errors.Any();
    private Entity(T t) => (_errors, _value) = ([], t ?? throw new ArgumentNullException(nameof(t)));
    private Entity(IEnumerable<Error> errors) => (_errors, _value) = (errors, default!);

    public static Entity<T> Valid(T t) => new(t);
    public static Entity<T> Invalid(IEnumerable<Error> errors) => new(errors);

    public static implicit operator Entity<T>(T t) => Valid(t);
    public static implicit operator Entity<T>(Error error) => Invalid([error]);
    public static implicit operator Entity<T>(Error[] errors) => Invalid(errors.ToArray());

    public R Match<R>(Func<Error[], R> Invalid, Func<T, R> Valid)
    {
        return IsValid ? Valid(_value!) : Invalid(_errors.ToArray()!);
    }
    public Result<R> MatchResult<R>(Func<T, R> Valid)
    {
        return IsValid ? ValidResult(Valid(_value!)) : InvalidResult<R>(_errors.ToArray());
    }
    public Result<T> Match()
    {
        return IsValid ? ValidResult(_value!) : InvalidResult<T>(_errors.ToArray());
    }
}


public static class Entity
{
    public static Entity<T> Valid<T>(T v) => Entity<T>.Valid(v);
    public static Entity<T> Invalid<T>(Error[] errors) => Entity<T>.Invalid(errors);
    
    public static string ErrorMessage<T>(this Entity<T> result) => string.Join(Environment.NewLine, result._errors.Select(e => e.Message));
    
    public static Entity<T> SetValueObject<T, V>(this T entity, ValueObject<V> valueObject, Func<T, V, T> setter)
    {
        return Entity<T>.Valid(entity).SetValueObject(valueObject, setter);
    }

    public static Entity<T> SetValueObject<T, V>(this Entity<T> entity, ValueObject<V> valueObject, Func<T, V, T> setter)
    {
        return valueObject.Match(
            Invalid: e => Entity<T>.Invalid(e.Concat(entity._errors)),
            Valid: v => entity.Match(
                                Valid: t => Entity<T>.Valid(setter(t, v)),
                                Invalid: e => entity));
    }

    public static Entity<T> SetEntity<T, E>(this T parentEntity, Entity<E> entity, Func<T, E, T> setter)
    {
        return Entity<T>.Valid(parentEntity).SetEntity(entity, setter);
    }

    public static Entity<T> SetEntity<T, E>(this Entity<T> parentEntity, Entity<E> entity, Func<T, E, T> setter)
    {
        return entity.Match(
            Invalid: e => Entity<T>.Invalid(e.Concat(parentEntity._errors)),
            Valid: v => parentEntity.Match(
                                Valid: t => Entity<T>.Valid(setter(t, v)),
                                Invalid: e => parentEntity));
    }
}