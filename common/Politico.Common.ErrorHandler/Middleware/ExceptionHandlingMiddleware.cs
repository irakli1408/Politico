using Microsoft.AspNetCore.Http;
using Politico.Common.ErrorHandler.Exceptions;
using Politico.Common.ErrorHandler.Models;
using System.Net;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Politico.Common.ErrorHandler.Middleware;

public class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = new ErrorResponse();
        var status = HttpStatusCode.InternalServerError;

        switch (exception)
        {
            case NotFoundException nf:
                status = HttpStatusCode.NotFound;
                response.Message = nf.Message;

                _logger.LogWarning(exception,
                    "NotFoundException at {Path}. Method: {Method}",
                    context.Request.Path,
                    context.Request.Method);
                break;

            case AppValidationException ve:
                status = HttpStatusCode.BadRequest;
                response.Message = "Validation Error";
                response.Errors = ve.Errors;

                _logger.LogWarning(exception,
                    "Validation error at {Path}. Method: {Method}. Errors: {@Errors}",
                    context.Request.Path,
                    context.Request.Method,
                    ve.Errors);
                break;

            case UnauthorizedException ue:
                status = HttpStatusCode.Unauthorized;
                response.Message = ue.Message;

                _logger.LogWarning(exception,
                    "Unauthorized at {Path}. Method: {Method}. Message: {Message}",
                    context.Request.Path,
                    context.Request.Method,
                    ue.Message);
                break;

            default:
                response.Message = "Server error";

                _logger.LogError(exception,
                    "Unhandled exception at {Path}. Method: {Method}",
                    context.Request.Path,
                    context.Request.Method);
                break;
        }

        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/json";

        var json = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(json);
    }
}
