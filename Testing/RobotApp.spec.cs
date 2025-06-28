namespace Testing;

public static partial class RobotAppShould
{
    [Test]
    public static void RejectNonExistentFile()
    {
        Given(a_non_existent_file).WhenTransforming(looking_for_file).Then(file_is_not_found);
    }
    
    [Test]
    public static void AcceptExistingFile()
    {
        Given(a_known_file).WhenTransforming(looking_for_file).Then(file_is_found);
    }
    
    [Test]
    public static void RejectInvalidGridSize()
    {
        Given(a_known_file_with_invalid_grid_size_has_been_read).WhenTransforming(parsing_grid_size).Then(grid_size_is_invalid);
    }
    
    [Test]
    public static void ParseGridSize()
    {
        Given(a_known_file_has_been_read).WhenTransforming(parsing_grid_size).Then(grid_size_is_correct);
    }
}