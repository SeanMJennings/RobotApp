namespace Testing;

public static partial class Acceptance
{
    [Test]
    public static void CorrectlyCalculatesAllOfSampleFile()
    {
        Given(sample_file_has_been_read)
            .WhenTransforming(calculating_instruction_results)
            .Then(success_is_calculated_for_first_robot_in_sample)
            .And(failure_is_calculated_for_second_robot_in_sample)
            .And(out_of_bounds_is_calculated_for_third_robot_in_sample);
    }

    [Test]
    public static void CorrectlyCalculatesAllOfSample1File()
    {
        Given(sample_1_file_has_been_read)
            .WhenTransforming(calculating_instruction_results)
            .Then(first_robot_in_sample_1_file_is_successful)
            .And(second_robot_in_sample_1_file_is_successful)
            .And(third_robot_in_sample_1_file_is_successful);
    }
    
    [Test]
    public static void CorrectlyCalculatesAllOfSample2File()
    {
        Given(sample_2_file_has_been_read)
            .WhenTransforming(calculating_instruction_results)
            .Then(first_robot_in_sample_2_file_is_successful)
            .And(second_robot_in_sample_2_file_is_successful)
            .And(third_robot_in_sample_2_file_is_crashed);
    }
}