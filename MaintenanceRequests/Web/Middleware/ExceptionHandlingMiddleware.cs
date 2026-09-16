using System.Text.Json;

namespace MaintenanceRequests.Web.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (KeyNotFoundException exception)
        {
            await WriteErrorAsync(context, StatusCodes.Status404NotFound, exception.Message);
        }
        catch (InvalidOperationException exception)
        {
            await WriteErrorAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (ArgumentException exception)
        {
            await WriteErrorAsync(context, StatusCodes.Status400BadRequest, exception.Message);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            await WriteErrorAsync(context, StatusCodes.Status500InternalServerError,
                "Произошла внутренняя ошибка сервера.");
        }
    }

    private static async Task WriteErrorAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            error = message
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}