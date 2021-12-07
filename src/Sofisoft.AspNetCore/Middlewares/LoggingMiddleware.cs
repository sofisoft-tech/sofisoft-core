using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Sofisoft.Abstractions.Exceptions;
using Sofisoft.Abstractions.Managers;
using Sofisoft.Abstractions.Responses;
using static Sofisoft.Abstractions.SofisoftConstants;
using ApplicationException = Sofisoft.Abstractions.Exceptions.ApplicationException;

namespace Sofisoft.AspNetCore.Middlewares
{
    public sealed class LoggingMiddleware : IMiddleware
    {
        private readonly ILoggerManager _logger;

        public LoggingMiddleware(ILoggerManager logger)
            => _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                int statusCode = GetStatusCode(ex);

                if (statusCode == 500)
                {
                    ex.Data["ErrorId"] = await _logger.ErrorAsync(ex.Message,
                        ex.StackTrace!,
                        context.User.Identity?.Name ?? string.Empty,
                        context.Request.Headers[HeaderNames.UserAgent]);
                }

                await HandleExceptionAsync(context, ex, statusCode);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex, int statusCode)
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            ErrorResponse response;

            if(ex.Data["ErrorId"] is null)
            {
                response = new ErrorResponse(
                    GetMessage(context, ex),
                    statusCode,
                    GetTitle(ex),
                    GetErrors(ex)
                );
            }
            else
            {
                response = new ErrorResponse(
                    ex.Data["ErrorId"]?.ToString()!,
                    GetMessage(context, ex),
                    statusCode,
                    GetTitle(ex),
                    GetErrors(ex)
                );
            }
            

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }

        private static string GetCustomMessage(HttpContext context)
        {
            var cfg = context.RequestServices.GetSofisoftConfiguration();

            return context.Request.Method switch
            {
                "DELETE" => cfg.LanguageManager.GetString(HttpMethodError.Delete),
                "GET" => cfg.LanguageManager.GetString(HttpMethodError.Get),
                "PATCH" => cfg.LanguageManager.GetString(HttpMethodError.Patch),
                "POST" => cfg.LanguageManager.GetString(HttpMethodError.Post),
                "PUT" => cfg.LanguageManager.GetString(HttpMethodError.Put),
                _ => cfg.LanguageManager.GetString(HttpMethodError.Default)
            };
        }
        

        private static string GetMessage(HttpContext context, Exception ex) =>
            ex switch
            {
                BadRequestException => ex.Message,
                NotFoundException => ex.Message,
                ValidationException => ex.Message,
                TaskCanceledException => ex.Message,
                OperationCanceledException => ex.Message,
                _ => GetCustomMessage(context)
            };

        private static int GetStatusCode(Exception exception) =>
            exception switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                ValidationException => StatusCodes.Status422UnprocessableEntity,
                TaskCanceledException => StatusCodes.Status400BadRequest,
                OperationCanceledException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

        private static string? GetTitle(Exception exception) =>
            exception switch
            {
                ApplicationException applicationException => applicationException.Title,
                _ => "Server Error"
            };

        private static IReadOnlyDictionary<string, string[]>? GetErrors(Exception exception)
        {
            IReadOnlyDictionary<string, string[]>? errors = null;

            if (exception is ValidationException validationException)
            {
                errors = validationException.Errors;
            }

            return errors;
        }

    }
}