using Web.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddAplicationServices();
builder.AddInfrastructureServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(config =>
{
    config.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception is FluentValidation.ValidationException validationEx)
        {
            var errors = validationEx.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";

            var response = new
            {
                message = "Validation failed",
                errors
            };

            await context.Response.WriteAsJsonAsync(response);
        }
        else if (exception is KeyNotFoundException)
        {
            context.Response.StatusCode = 404;
            var response = new
            {
                message = exception.Message
            };
            await context.Response.WriteAsJsonAsync(response);
        }
        
    });
});

app.UseHttpsRedirection();
CustomerEndpoints.Map(app);
BankAccountEndpoints.Map(app);
TransactionEndpoints.Map(app);

app.Run();
