namespace FunctionalProgrammingKit;

public readonly struct ValueObject<T>
{
    public bool IsValid => _errors.Length == 0;
    internal readonly T? _value;
    internal readonly Error[] _errors;
    private ValueObject(T v, Error[] errors) => (_value, _errors) = (v, errors);
    internal static ValueObject<T> CreateValid(T v) => new(v, []);
    internal static ValueObject<T> CreateInvalid(Error[] errors) => new(default!, errors);
    public R Match<R>(Func<Error[], R> Invalid, Func<T, R> Valid)
    {
        return IsValid ? Valid(_value!) : Invalid(_errors.ToArray());
    }
    public ReturnWrapper<T> Match()
    {
        return IsValid ? ValidWrapper(_value!) : InvalidWrapper<T>(_errors.ToArray());
    } 
    public static implicit operator ValueObject<T>(Error error) => CreateInvalid([error]);
}

public static class ValueObject
{
    public static ValueObject<T> Valid<T>(T v) => ValueObject<T>.CreateValid(v);
    public static ValueObject<T> Invalid<T>(Error[] errors) => ValueObject<T>.CreateInvalid(errors);
}