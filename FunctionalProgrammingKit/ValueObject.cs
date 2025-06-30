namespace FunctionalProgrammingKit;

public readonly struct ValueObject<T>
{
    private bool IsValid => _errors.Length == 0;
    private readonly T? _value;
    private readonly Error[] _errors;
    private ValueObject(T v, Error[] errors) => (_value, _errors) = (v, errors);
    internal static ValueObject<T> CreateValid(T v) => new(v, []);
    internal static ValueObject<T> CreateInvalid(Error[] errors) => new(default!, errors);
    public R Match<R>(Func<Error[], R> Invalid, Func<T, R> Valid)
    {
        return IsValid ? Valid(_value!) : Invalid(_errors.ToArray());
    }
    public static implicit operator ValueObject<T>(Error error) => CreateInvalid([error]);
}

public static class ValueObject
{
    public static ValueObject<T> Valid<T>(T v) => ValueObject<T>.CreateValid(v);
    public static ValueObject<T> Invalid<T>(Error[] errors) => ValueObject<T>.CreateInvalid(errors);
    public static ValueObject<T> Invalid<T>(string error) => ValueObject<T>.CreateInvalid([error.ToErrorMessage()]);
}