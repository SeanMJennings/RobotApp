using FunctionalProgrammingKit;

namespace Testing.Framework;

internal static class VerbsAndNouns
{
    internal static object Given(Func<object> func)
    {
        return func.Invoke();
    }

    internal static object And(this object previousResult, Func<object, object> func)
    {
        return func.Invoke(previousResult);
    }    
    
    internal static object And(this object _, Func<object> func)
    {
        return func.Invoke();
    }
    
    internal static void And(this object previousResult, Action<object> func)
    {
        func.Invoke(previousResult);
    }    

    internal static object When(this object previousResult, Func<object, object> func)
    {
        return func.Invoke(previousResult);
    }    
    
    internal static object When(this object _, Func<object> func)
    {
        return func.Invoke();
    }

    internal static object Then(this object previousResult, Func<object, object> func)
    {
        return func.Invoke(previousResult);
    }    
    
    internal static void Then(this object previousResult, Action<object> func)
    {
        func.Invoke(previousResult);
    }
    
    internal static void Then(this object _, Action func)
    {
        func.Invoke();
    }
}