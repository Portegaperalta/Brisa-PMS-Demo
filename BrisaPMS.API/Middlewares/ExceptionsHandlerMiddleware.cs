using BrisaPMS.Application.Exceptions;
using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Users;
using BrisaPMS.Identity.Exceptions;
using System.Net;
using System.Text.Json;

namespace BrisaPMS.API.Middlewares
{
    public class ExceptionsHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionsHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception exception)
            {
                await HandleException(context, exception);
            }
        }

        private Task HandleException(HttpContext context, Exception exception)
        {
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var result = string.Empty;

            switch(exception)
            {
                case NotFoundException:
                    httpStatusCode = HttpStatusCode.NotFound;
                    result = JsonSerializer.Serialize(new
                    {
                        message = exception.Message
                    });
                    break;

                case EmptyRequiredFieldException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new
                    {
                        message = exception.Message
                    });
                    break;

                case InvalidFieldException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new
                    {
                        message = exception.Message
                    });
                    break;

                case MaxCharacterLimitException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new
                    {
                        message = exception.Message
                    });
                    break;

                case BusinessRuleException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new
                    {
                        message = exception.Message
                    });
                    break;

                case LanguageNotSupportedException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new
                    {
                        message = exception.Message
                    });
                    break;

                case InvalidItbisRateException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new
                    {
                        message = exception.Message
                    });
                    break;

                case InvalidServiceChargeRateException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new
                    {
                        message = exception.Message
                    });
                    break;

                case MediatorException:
                    httpStatusCode = HttpStatusCode.InternalServerError;
                    break;

                case ValidationException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new
                    {
                        message = exception.Message
                    });
                    break;

                case IdentityException:
                    httpStatusCode = HttpStatusCode.InternalServerError;
                    result = JsonSerializer.Serialize(new
                    {
                        message = exception.Message
                    });
                    break;
            }

            context.Response.StatusCode = (int)httpStatusCode;
            return context.Response.WriteAsync(result);
        }
    }

    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionsHandlerMiddleware>();
        }
    }
}