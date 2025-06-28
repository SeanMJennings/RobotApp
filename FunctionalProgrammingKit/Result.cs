namespace FunctionalProgrammingKit;

public class Result<T>
{
    public readonly IEnumerable<Error> Errors;
    public readonly T Data;

    public bool Success => !Errors.Any();

    public Result(T t) => (Errors, Data) = ([], t);
    public Result(IEnumerable<Error> errors) => (Errors, Data) = (errors, default!);
}

public static class ResultExtensions
{
    public static string ErrorMessage<T>(this Result<T> result) => string.Join(Environment.NewLine, result.Errors.Select(e => e.Message));
}