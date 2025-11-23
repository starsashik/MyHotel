using hotel_backend.Exceptions.BaseExceptions;

namespace hotel_backend.Exceptions.SpecificExceptions;

public class AccessException : ExpectedException
{
    public AccessException()
    {
    }

    public AccessException(string message) : base(message)
    {
    }

    public AccessException(string message, Exception inner) : base(message, inner)
    {
    }
}