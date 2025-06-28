using Common;
using FunctionalProgrammingKit;

namespace Application;

public static class File
{
    public static Result<string[]> GetFileContents(this string path) => 
        (System.IO.File.Exists(path) ? Valid(FileExtensions.ReadAllLinesThatAreNotEmpty(path)) : Invalid<string[]>([new Error($"File not found: {path}")]))
            .Match(errors => new Result<string[]>(errors), content => new Result<string[]>(content));
}