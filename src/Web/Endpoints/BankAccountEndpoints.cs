using Application.BankAccounts.Commands.CreateBankAccount;
using Application.BankAccounts.Commands.DeleteBankAccount;
using Application.BankAccounts.Queries.GetBalanceByNumber;
using MediatR;

namespace Web.Endpoints;

public static class BankAccountEndpoints
{
    public static void Map(WebApplication app)
    {
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

        app.MapGet("/bankaccounts/balance", async (ISender sender, [AsParameters] GetBankAccountBalanceByNumberQuery query) =>
        {
            var result = await sender.Send(query);
            return Results.Ok(result);
        })
        .WithName("GetBankAccountBalanceByNumber")
        .WithOpenApi();
    }
}
