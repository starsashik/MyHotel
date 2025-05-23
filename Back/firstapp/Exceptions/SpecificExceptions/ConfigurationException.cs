﻿using firstapp.Exceptions.BaseExceptions;

namespace firstapp.Exceptions.SpecificExceptions;

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