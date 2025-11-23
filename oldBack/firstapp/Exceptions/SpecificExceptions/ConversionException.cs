using firstapp.Exceptions.BaseExceptions;

namespace firstapp.Exceptions.SpecificExceptions;

public class ConversionException : ExpectedException
{
    public ConversionException()
    {
    }

    public ConversionException(string message) : base(message)
    {
    }

    public ConversionException(string message, Exception inner) : base(message, inner)
    {
    }
}