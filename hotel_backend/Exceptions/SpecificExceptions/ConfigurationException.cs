using hotel_backend.Exceptions.BaseExceptions;

namespace hotel_backend.Exceptions.SpecificExceptions;

public class ConfigurationException : ExpectedException
{
    public ConfigurationException()
    {
    }

    public ConfigurationException(string message) : base(message)
    {
    }

    public ConfigurationException(string message, Exception inner) : base(message, inner)
    {
    }
}