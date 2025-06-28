namespace FunctionalProgrammingKit;

public readonly record struct Error(string Message)
{
    public string Message { get; } = Message;
}

public static class StringExtensions
{
    public static Error ToErrorMessage(this string error)
    {
        return new Error(error);
    }
}