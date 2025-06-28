namespace FunctionalProgrammingKit;

public static class Return
{
    public static R Match<T,R>(this ValueObject<T> value, Func<T, R> Valid, Func<Error[], R> Invalid) => value.IsValid ? Valid(value._value!) : Invalid(value._errors);
}

public record ReturnWrapper<T>(T Value, Error[] Errors);

public static class ReturnWrapperExtensions
{
    public static ReturnWrapper<T> ValidWrapper<T>(T v) => new(v, []);
    public static ReturnWrapper<T> InvalidWrapper<T>(Error[] errors) => new(default!, errors);
}