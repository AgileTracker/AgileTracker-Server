using agileTrackerServer.Models.Exceptions;
using agileTrackerServer.Models.ViewModels;
using System.Net;
using System.Text.Json;

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
        catch (DomainException ex)
        {
            await HandleDomainExceptionAsync(context, ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            await HandleUnauthorizedExceptionAsync(context, ex);
        }
        catch (Exception)
        {
            await HandleInternalExceptionAsync(context);
        }
    }

    // ==========================
    // DOMAIN (404 / 400)
    // ==========================
    private static Task HandleDomainExceptionAsync(
        HttpContext context,
        DomainException ex)
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        context.Response.ContentType = "application/json";

        var result = ResultViewModel.Fail(ex.Message);

        return context.Response.WriteAsJsonAsync(result);
    }

    // ==========================
    // AUTH (401)
    // ==========================
    private static Task HandleUnauthorizedExceptionAsync(
        HttpContext context,
        UnauthorizedAccessException ex)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";

        var result = ResultViewModel.Fail(ex.Message);

        return context.Response.WriteAsJsonAsync(result);
    }

    // ==========================
    // INTERNAL (500)
    // ==========================
    private static Task HandleInternalExceptionAsync(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var result = ResultViewModel.Fail(
            "Ocorreu um erro interno no servidor."
        );

        return context.Response.WriteAsJsonAsync(result);
    }
}
