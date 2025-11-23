using hotel_backend.Exceptions.BaseExceptions;

namespace hotel_backend.Exceptions.SpecificExceptions;

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