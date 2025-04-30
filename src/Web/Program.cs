using Application.Customers.Commands.CreateCustomer;
using Application.Customers.Commands.DeleteCustomer;
using Application.Customers.Commands.UpdateCustomer;
using Application.BankAccounts.Commands.CreateBankAccount;
using Application.BankAccounts.Commands.DeleteBankAccount;
using Application.BankAccounts.Queries.GetBalanceByNumber;
using Application.Transactions.Commands.CreateTransaction;
using Application.Transactions.Queries.GetTransactionSummary;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

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

app.UseHttpsRedirection();
app.MapPost("/customers", async (ISender sender, CreateCustomerCommand command) =>
{
    var id = await sender.Send(command);

    return TypedResults.Created("/Customers/{id}", id);
})
.WithName("CreateCustomer")
.WithOpenApi();

app.MapDelete("/customers/{id:int}", async (ISender sender, int id) =>
{
    await sender.Send(new DeleteCustomerCommand(id));
    return Results.NoContent();
})
.WithName("DeleteCustomer")
.WithOpenApi();

app.MapPatch("/customers/{id:int}", async (ISender sender, int id, UpdateCustomerCommand command) =>
{
    if (id != command.Id)
    {
        return Results.BadRequest("El ID de la ruta no coincide con el del cuerpo.");
    }

    var updatedId = await sender.Send(command);
    return Results.Ok(updatedId);
})
.WithName("UpdateCustomer")
.WithOpenApi();

app.MapPost("/bankaccounts", async (ISender sender, CreateBankAccountCommand command) =>
{
    var id = await sender.Send(command);
    return TypedResults.Created($"/bankaccounts/{id}", id);
})
.WithName("CreateBankAccount")
.WithOpenApi();

app.MapDelete("/bankaccounts/{id:int}", async (ISender sender, int id) =>
{
    await sender.Send(new DeleteBankAccountCommand(id));
    return Results.NoContent();
})
.WithName("DeleteBankAccount")
.WithOpenApi();

app.MapPost("/transactions", async (ISender sender, CreateTransactionCommand command) =>
{
    var result = await sender.Send(command);
    return TypedResults.Ok(result);
})
.WithName("CreateTransaction")
.WithOpenApi();

app.MapGet("/bankaccounts/balance", async (ISender sender, [AsParameters] GetBankAccountBalanceByNumberQuery query) =>
{
    var result = await sender.Send(query);
    return Results.Ok(result);
})
.WithName("GetBankAccountBalanceByNumber")
.WithOpenApi();

app.MapGet("/transactions/summary/{bankAccountId:int}", async (ISender sender, int bankAccountId) =>
{
    var result = await sender.Send(new GetTransactionSummaryQuery(bankAccountId));
    return Results.Ok(result);
})
.WithName("GetTransactionSummary")
.WithOpenApi();

app.Run();
