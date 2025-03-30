using Microsoft.AspNetCore.Http;
using PriceData.Domain.Exceptions;
using Serilog;
using System;
using System.Net;
using System.Text.Json;

namespace PriceData.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode status;
        string message;
        string errorType;

        switch (exception)
        {
            case PriceNotFoundException:
                status = HttpStatusCode.NotFound;
                errorType = "Price Not Found";
                message = exception.Message;
                break;

            case InvalidSymbolException:
                status = HttpStatusCode.BadRequest;
                errorType = "Invalid Symbol";
                message = exception.Message;
                break;

            case ExternalApiException:
                status = HttpStatusCode.ServiceUnavailable;
                errorType = "External API Error";
                message = exception.Message;
                break;

            case DatabaseException:
                status = HttpStatusCode.InternalServerError;
                errorType = "Database Error";
                message = exception.Message;
                break;

            default:
                status = HttpStatusCode.InternalServerError;
                errorType = "Unhandled Error";
                message = "Occured unexpected error";
                break;
        }

        Log.Error($"Error: {errorType} - {message}");

        var response = new
        {
            StatusCode = (int)status,
            ErrorType = errorType,
            Message = message
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}

