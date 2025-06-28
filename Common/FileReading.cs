namespace Common;

public static class FileExtensions
{
    public static string[] ReadAllLinesThatAreNotEmpty(string path)
    {
        return File.ReadAllLines(path).Where(s => s != string.Empty).ToArray();
    }
}