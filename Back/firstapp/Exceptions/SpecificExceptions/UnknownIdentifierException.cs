﻿using firstapp.Exceptions.BaseExceptions;

namespace firstapp.Exceptions.SpecificExceptions;

public class UnknownIdentifierException : ExpectedException
{
    public UnknownIdentifierException()
    {
    }

    public UnknownIdentifierException(string message) : base(message)
    {
    }

    public UnknownIdentifierException(string message, Exception inner) : base(message, inner)
    {
    }
}