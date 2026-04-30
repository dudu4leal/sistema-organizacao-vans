using System.Net;
using System.Text.Json;
using CaronaAlvinegra.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CaronaAlvinegra.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain rule violation");
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = ex.Message }));
        }
        catch (FluentValidation.ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation failed");
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            var errors = ex.Errors.Select(e => e.ErrorMessage);
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { errors }));
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message?.Contains("FOREIGN KEY") == true)
        {
            _logger.LogError(ex, "Foreign key constraint violation");
            context.Response.StatusCode = (int)HttpStatusCode.Conflict;
            context.Response.ContentType = "application/json";

            var message = "Operação não permitida: este registro possui vínculos com outros registros no sistema. " +
                          "Remova ou reatribua os registros dependentes antes de prosseguir.";

            // Mensagem mais específica baseada na entidade
            if (ex.Message.Contains("Presencas") || ex.Message.Contains("Passageiros"))
                message = "Não é possível excluir: existem presenças ou passageiros vinculados a este registro.";
            else if (ex.Message.Contains("Usuarios"))
                message = "Não é possível excluir: existem usuários vinculados a este registro. Reatribua-os primeiro.";

            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = message }));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Erro interno do servidor." }));
        }
    }
}
