using hotel_backend.Exceptions.BaseExceptions;

namespace hotel_backend.Exceptions.SpecificExceptions;

public class InitializationException : ExpectedException
{
    public InitializationException()
    {
    }

    public InitializationException(string message) : base(message)
    {
    }

    public InitializationException(string message, Exception inner) : base(message, inner)
    {
    }
}