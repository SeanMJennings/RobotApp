namespace FunctionalProgrammingKit;

public readonly record struct Error(string Message)
{
    public string Message { get; } = Message;
}