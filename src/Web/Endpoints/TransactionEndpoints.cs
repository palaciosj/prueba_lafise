using Application.Transactions.Commands.CreateTransaction;
using Application.Transactions.Queries.GetTransactionSummary;
using MediatR;

namespace Web.Endpoints;

public static class TransactionEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/transactions", async (ISender sender, CreateTransactionCommand command) =>
        {
            var result = await sender.Send(command);
            return TypedResults.Ok(result);
        })
        .WithName("CreateTransaction")
        .WithOpenApi();

        app.MapGet("/transactions/summary/{bankAccountId:int}", async (ISender sender, int bankAccountId) =>
        {
            var result = await sender.Send(new GetTransactionSummaryQuery(bankAccountId));
            return Results.Ok(result);
        })
        .WithName("GetTransactionSummary")
        .WithOpenApi();
    }
}
