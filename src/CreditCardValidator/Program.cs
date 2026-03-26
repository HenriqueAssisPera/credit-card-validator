using CreditCardValidator.Data;
using CreditCardValidator.Features.RegisterCard;
using CreditCardValidator.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
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

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionHandlerFeature?.Error;

        context.Response.ContentType = "application/json";

        if (exception is ValidationException validationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var errors = validationException.Errors
                .Select(e => e.ErrorMessage)
                .Distinct()
                .ToArray();

            var response = new
            {
                errors
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            return;
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var genericResponse = new
        {
            errors = new[] { "An unexpected error occurred." }
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(genericResponse));
    });
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();