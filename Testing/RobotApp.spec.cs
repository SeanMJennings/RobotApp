namespace Testing;

public static partial class RobotAppShould
{
    [Test]
    public static void RejectUnknownFile()
    {
        Given(an_unknown_file)
            .When(looking_for_file)
            .Then(file_is_not_found);
    }
}