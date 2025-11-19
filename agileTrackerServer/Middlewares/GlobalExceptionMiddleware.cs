using agileTrackerServer.Models.ViewModels;
using System.Net;

namespace agileTrackerServer.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
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

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        var result = new ResultViewModel(
            message: "Ocorreu um erro interno no servidor.",
            success: false,
            data: null,
            errors: new List<string> { ex.Message }
        );

        return context.Response.WriteAsJsonAsync(result);
    }
}