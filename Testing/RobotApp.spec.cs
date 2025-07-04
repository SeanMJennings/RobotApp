﻿namespace Testing;

public static partial class RobotAppShould
{
    [Test]
    public static void RejectNonExistentFile()
    {
        Given(a_non_existent_file)
            .WhenTransforming(looking_for_file)
            .Then(file_is_not_found);
    }
    
    [Test]
    public static void AcceptExistingFile()
    {
        Given(a_known_file)
            .WhenTransforming(looking_for_file)
            .Then(file_is_found);
    }
    
    [Test]
    public static void RejectInvalidGridDimensions()
    {
        Given(a_known_file_with_invalid_grid_size_has_been_read)
            .WhenTransforming(parsing)
            .Then(grid_size_is_invalid);
    }
    
    [Test]
    public static void RejectInvalidStartingLocation()
    {
        Given(a_known_file_with_invalid_starting_location_has_been_read)
            .WhenTransforming(parsing)
            .Then(starting_location_is_invalid);
    }
    
    [Test]
    public static void RejectInvalidMovements()
    {
        Given(a_known_file_with_invalid_movements_has_been_read)
            .WhenTransforming(parsing)
            .Then(movements_are_invalid);
    }
    
    [Test]
    public static void RejectInvalidEndingLocation()
    {
        Given(a_known_file_with_invalid_ending_location_has_been_read)
            .WhenTransforming(parsing)
            .Then(ending_location_is_invalid);
    }
    
    [Test]
    public static void CalculateSuccessForValidInstructions()
    {
        Given(sample_file_has_been_read)
            .WhenTransforming(calculating_instruction_results_for_sample_file)
            .Then(success_is_calculated_for_first_robot_in_sample);
    }
    
    [Test]
    public static void CalculateFailureForIncorrectEndLocation()
    {
        Given(sample_file_has_been_read)
            .WhenTransforming(calculating_instruction_results_for_sample_file)
            .Then(failure_is_calculated_for_second_robot_in_sample);
    }
    
    [Test]
    public static void CalculateOutOfBoundsForInvalidMovements()
    {
        Given(sample_file_has_been_read)
            .WhenTransforming(calculating_instruction_results_for_sample_file)
            .Then(out_of_bounds_is_calculated_for_third_robot_in_sample);
    }

    [Test]
    public static void RejectObstaclesNotDeclaredBeforeInstructions()
    {
        Given(a_known_file_with_obstacles_declared_late_has_been_read)
            .WhenTransforming(parsing)
            .Then(obstacles_are_invalid_as_they_are_late);
    }
    
    [Test]
    public static void CalculateCrashForInstructionsThatCollideWithObstacles()
    {
        Given(a_known_file_with_a_collision_has_been_read)
            .WhenTransforming(calculating_instruction_results_for_sample_file)
            .Then(crash_is_calculated);
    }
}