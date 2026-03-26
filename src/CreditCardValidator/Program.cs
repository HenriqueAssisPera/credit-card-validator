using CreditCardValidator.Data;
using CreditCardValidator.Exceptions;
using CreditCardValidator.Features.RegisterCard;
using CreditCardValidator.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();       

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddValidatorsFromAssemblyContaining<RegisterCardCommandValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddScoped<CardValidator>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>("sqlserver");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionHandlerFeature?.Error;
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

        context.Response.ContentType = "application/json";

        switch (exception)
        {
            case ValidationException validationException:
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var errors = validationException.Errors
                    .Select(e => e.ErrorMessage)
                    .Distinct()
                    .ToArray();

                var response = new { errors };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                break;
            }

            case CardRegistrationException:
            {
                logger.LogError(exception, "Card registration failed.");

                context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;

                var response = new
                {
                    errors = new[] { "Failed to register the card. Please verify the data and try again." }
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                break;
            }

            case DatabaseUnavailableException:
            {
                logger.LogError(exception, "Database is unavailable.");

                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;

                var response = new
                {
                    errors = new[] { "The service is temporarily unavailable. Please try again later." }
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                break;
            }

            case OperationCanceledException:
            {
                logger.LogWarning("Request was cancelled.");

                context.Response.StatusCode = StatusCodes.Status499ClientClosedRequest;

                var response = new
                {
                    errors = new[] { "The request was cancelled." }
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                break;
            }

            default:
            {
                logger.LogError(exception, "An unexpected error occurred.");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = new
                {
                    errors = new[] { "An unexpected error occurred." }
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                break;
            }
        }
    });
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();