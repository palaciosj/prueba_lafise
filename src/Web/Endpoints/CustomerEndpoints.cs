using Application.Customers.Commands.CreateCustomer;
using Application.Customers.Commands.DeleteCustomer;
using Application.Customers.Commands.UpdateCustomer;
using MediatR;

namespace Web.Endpoints;

public static class CustomerEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/customers", async (ISender sender, CreateCustomerCommand command) =>
        {
            var id = await sender.Send(command);
            return TypedResults.Created("/customers/{id}", id);
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
                return Results.BadRequest("El ID de la ruta no coincide con el del cuerpo.");

            var updatedId = await sender.Send(command);
            return Results.Ok(updatedId);
        })
        .WithName("UpdateCustomer")
        .WithOpenApi();
    }
}
