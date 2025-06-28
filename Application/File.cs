using FunctionalProgrammingKit;

namespace Application;

public static class File
{
    public static Result<string> GetFile(this string path) => 
        (System.IO.File.Exists(path) ? Valid(path) : Invalid<string>([new Error($"File not found: {path}")]))
            .Map(System.IO.File.ReadAllText)
            .Match(errors => new Result<string>(errors), content => new Result<string>(content));
}