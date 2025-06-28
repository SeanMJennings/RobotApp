using FunctionalProgrammingKit;

namespace Domain.Primitives;

public static class Uint
{
    public static ValueObject<uint> Create(this uint value)
    {
        return Valid(value);
    }  
}

public static class Int
{
    public static ValueObject<int> Create(this int value)
    {
        return Valid(value);
    }  
}

public static class Decimal
{
    public static ValueObject<decimal> Create(this decimal value)
    {
        return Valid(value);
    }  
}

public static class Bool
{
    public static ValueObject<bool> Create(this bool the_bool)
    {
        return Valid(the_bool);
    }  
}

public static class String
{
    public static ValueObject<string> Create(this string value)
    {
        return Valid(value);
    }  
}