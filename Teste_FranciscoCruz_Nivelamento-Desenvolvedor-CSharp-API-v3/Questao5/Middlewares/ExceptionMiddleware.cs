using Newtonsoft.Json;
using Questao5.Infrastructure.Exceptions;
using System.Net;

namespace Questao5.Infrastructure.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (DomainException ex)
        {
            var message = JsonConvert.SerializeObject(new { Tipo = ex.TipoErro.ToString(), Mensagem = ex.Message });
            await HandleRequestExceptionAsync(httpContext, message, HttpStatusCode.BadRequest);
        }
    }

    private static async Task HandleRequestExceptionAsync(HttpContext context, string message, HttpStatusCode statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsync(message);
    }
}