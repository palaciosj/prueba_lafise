using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Events.CustomerEvents;
using MediatR;
using Ardalis.GuardClauses;

namespace Application.Customers.Commands.UpdateCustomer;

public record UpdateCustomerCommand : IRequest<int>
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public DateTime? BirthDate { get; init; }
    public Gender? Gender { get; init; }
    public double? Incomes { get; init; }
}

public class UpdateCustomerCommandHandler(IApplicationDbContext _context) : IRequestHandler<UpdateCustomerCommand, int> 
{
    //private readonly IApplicationDbContext _context;

    public async Task<int> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Customers
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        if (request.Name is not null)
            entity.Name = request.Name;

        if (request.BirthDate is not null)
            entity.BirthDate = request.BirthDate.Value;

        if (request.Gender is not null)
            entity.Gender = request.Gender.Value;

        if (request.Incomes is not null)
            entity.Incomes = request.Incomes.Value;

        entity.AddDomainEvent(new CustomerUpdatedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;

    }

}