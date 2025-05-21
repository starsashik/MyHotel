﻿using firstapp.Exceptions.BaseExceptions;

namespace firstapp.Exceptions.SpecificExceptions;

public class AuthorizationException : ExpectedException
{
    public AuthorizationException()
    {
    }

    public AuthorizationException(string message) : base(message)
    {
    }

    public AuthorizationException(string message, Exception inner) : base(message, inner)
    {
    }
}