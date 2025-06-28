using FunctionalProgrammingKit;

namespace Testing;

internal static class TestingLanguage
{
    internal static T Given<T>(Func<T> func)
    {
        return func.Invoke();
    }

    internal static T And<T>(this T previousResult, Func<T, T> func)
    {
        return func.Invoke(previousResult);
    }    
    
    internal static T And<T>(this T _, Func<T> func)
    {
        return func.Invoke();
    }
    
    internal static void And<T>(this T previousResult, Action<T> func)
    {
        func.Invoke(previousResult);
    }    

    internal static T When<T>(this T previousResult, Func<T, T> func)
    {
        return func.Invoke(previousResult);
    }
    
    internal static Result<T> When<T>(this T previousResult, Func<T, Result<T>> func)
    {
        return func.Invoke(previousResult);
    }
    
    internal static Result<U> WhenTransforming<T, U>(this T previousResult, Func<T, Result<U>> func)
    {
        return func.Invoke(previousResult);
    }
    
    internal static Entity<U> WhenTransforming<T, U>(this T previousResult, Func<T, Entity<U>> func)
    {
        return func.Invoke(previousResult);
    }
    
    internal static T When<T>(this T _, Func<T> func)
    {
        return func.Invoke();
    }

    internal static T Then<T>(this T previousResult, Func<T, T> func)
    {
        return func.Invoke(previousResult);
    }    
    
    internal static void Then<T>(this T previousResult, Action<T> func)
    {
        func.Invoke(previousResult);
    }
    
    internal static void Then<T>(this T _, Action func)
    {
        func.Invoke();
    }
}