using FunctionalProgrammingKit;

namespace Testing;

[TestFixture]
public static partial class RobotAppShould
{
    private static object an_unknown_file()
    {
        return "unknown_file.txt";
    }
    
    private static object looking_for_file(object path) 
    {
        return looking_for_file((string)path);
    }

    private static object looking_for_file(string path)
    {
        return path.GetFile();
    }

    private static void file_is_not_found(object obj)
    {
        Assert.That((obj as Result<string>)!.Success, Is.EqualTo(false));
        Assert.That((obj as Result<string>)!.ErrorMessage(), Is.EqualTo("File not found: unknown_file.txt"));
    }
}