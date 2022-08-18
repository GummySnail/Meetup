using System.Net;
using Meetup.Api.Models;
using Meetup.Core.Exceptions;
using Meetup.Core.Logic.RefreshToken.Exceptions;
using Meetup.Infrastructure.Identity.Exceptions;

namespace Meetup.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;


    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _logger.LogTrace(ex.StackTrace);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = GetStatusCode(ex);

            await context.Response.WriteAsync(
                new ErrorModel
                {
                    StatusCode = context.Response.StatusCode,
                    Message = context.Response.StatusCode == (int)HttpStatusCode.InternalServerError
                        ? "Internal server error"
                        : ex.Message
                }.ToString()
            );
        }
    }

    private static int GetStatusCode(Exception ex)
    {
        return ex switch
        {
            DefaultException => (int)HttpStatusCode.BadRequest,
            NotFoundException => (int)HttpStatusCode.NotFound,
            EmailIsTakenException => (int)HttpStatusCode.BadRequest,
            UserNameIsTakenException => (int)HttpStatusCode.BadRequest,
            PasswordsAreNotEqualException => (int)HttpStatusCode.BadRequest,
            InvalidPasswordException => (int)HttpStatusCode.Unauthorized,
            RefreshTokenException => (int)HttpStatusCode.BadRequest,
            /*UnauthorizedException => (int)HttpStatusCode.Unauthorized,
            ConfirmEmailException => (int)HttpStatusCode.BadRequest,
            ConfirmResetPasswordException => (int)HttpStatusCode.BadRequest,
            SaveDataBaseException => (int)HttpStatusCode.BadRequest,
            UserIsNotOwnerException => (int)HttpStatusCode.BadRequest,*/
            _ => (int)HttpStatusCode.InternalServerError
        };
    }
}