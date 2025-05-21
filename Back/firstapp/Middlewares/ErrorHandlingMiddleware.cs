﻿using System.Net;
using System.Text.Json;
using EntityFramework.Exceptions.Common;
using firstapp.Contracts;
using firstapp.Contracts.Other;
using firstapp.Exceptions.BaseExceptions;
using firstapp.Exceptions.SpecificExceptions;
using Microsoft.EntityFrameworkCore;

namespace firstapp.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;

    public ErrorHandlingMiddleware(RequestDelegate next,  IWebHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ExpectedException e) when (e is AuthorizationException or AccessException)
        {
            await HandleUnauthorizedExceptionAsync(context, "Authorization error", e, true);
        }
        catch (ExpectedException e) when (e is ConversionException or UnknownIdentifierException)
        {
            await HandleExceptionAsync(context, "Wrong format error", e, true);
        }
        catch (UniqueConstraintException e)
        {
            var ex = new Exception("Usernames, emails, role names must be unique", e);

            await HandleExceptionAsync(context, "Database error", ex, true);
        }
        catch (CannotInsertNullException e)
        {
            var ex = new Exception("Unable to insert null into some row in the database", e);

            await HandleExceptionAsync(context, "Database error", ex, true);
        }
        catch (MaxLengthExceededException e)
        {
            var ex = new Exception("Some string value is too long", e);

            await HandleExceptionAsync(context, "Database error", ex, true);
        }
        catch (NumericOverflowException e)
        {
            var ex = new Exception("Some numeric value is too big", e);

            await HandleExceptionAsync(context, "Database error", ex, true);
        }
        catch (ReferenceConstraintException e)
        {
            var ex = new Exception("Destructive action, object has some dependencies or unknown object reference", e);

            await HandleExceptionAsync(context, "Database error", ex, true);
        }
        catch (DbUpdateException e)
        {
            var ex = new Exception("Unexpected database exception", e);

            await HandleExceptionAsync(context, "Database error", ex, true);
        }
        catch (FileNotFoundException e)
        {
            await HandleNotFoundExceptionAsync(context);
        }
        catch (ExpectedException e) when (e is IntegrityException or InitializationException or ConfigurationException)
        {
            await HandleBadRequestExceptionAsync(context, "Critical system error", e, true);
        }
        catch (ExpectedException e)
        {
            var ex = new Exception("Unexpected exception, probably you did not register new exception type in the " +
                                   "error handling middleware", e);

            await HandleBadRequestExceptionAsync(context, "Unregistered exception", ex, true);
        }
        catch (Exception e)
        {
            var ex = new Exception("Unexpected server exception, check logs for more information", e);
        
            await HandleBadRequestExceptionAsync(context, "Unexpected exception", ex, true);
        }
    }

    private async Task HandleUnauthorizedExceptionAsync(HttpContext context, string exceptionGroup, Exception exception,
        bool isLogNeeded)
    {
        if (isLogNeeded)
        {
            using var scope = context.RequestServices.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ErrorHandlingMiddleware>>();

            logger.LogWarning("Error while authorization process: {ErrorMessage}\n" +
                              "Stack trace: {StackTrace}",
                exception.Message, exception.StackTrace);
        }

        var errorResponse = new BaseResponse<string>(
            null,
            new ExceptionDto(
                exceptionGroup,
                exception.Message));

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }

    private async Task HandleExceptionAsync(HttpContext context, string exceptionGroup, Exception exception,
        bool isLogNeeded)
    {
        if (isLogNeeded)
        {
            using var scope = context.RequestServices.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ErrorHandlingMiddleware>>();

            logger.LogError("An error occured while request processing: {ErrorMessage}\n" +
                            "Stack trace: {StackTrace}",
                exception.Message, exception.StackTrace);
        }

        var errorResponse = new BaseResponse<string>(
            null,
            new ExceptionDto(
                exceptionGroup,
                exception.Message));

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }

    private async Task HandleBadRequestExceptionAsync(HttpContext context, string exceptionGroup, Exception exception,
        bool isLogNeeded)
    {
        if (isLogNeeded)
        {
            using var scope = context.RequestServices.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ErrorHandlingMiddleware>>();

            logger.LogCritical("A critical error occured while request processing, " +
                               "immediately inform system administrator about that: {ErrorMessage}\n" +
                               "Stack trace: {StackTrace}",
                exception.Message, exception.StackTrace);
        }

        var errorResponse = new BaseResponse<string>(
            null,
            new ExceptionDto(
                exceptionGroup,
                exception.Message));

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
    
    private async Task HandleNotFoundExceptionAsync(HttpContext context)
    {
        context.Response.Clear();

        context.Response.StatusCode = StatusCodes.Status404NotFound;
        context.Response.ContentType = "text/html; charset=utf-8";
        
        var htmlPath = Path.Combine(_env.WebRootPath, "404Page.html");
        
        await context.Response.SendFileAsync(htmlPath);
    }
}