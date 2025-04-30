using Application.Common.Interfaces;
using Domain.Events.CustomerEvents;
using Ardalis.GuardClauses;
using MediatR;

namespace Application.Customers.Commands.DeleteCustomer;

public record DeleteCustomerCommand(int Id) : IRequest;

public class DeleteCustomerCommandHandler(IApplicationDbContext _context) : IRequestHandler<DeleteCustomerCommand>
{
    //private readonly IApplicationDbContext _context = context;

    public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Customers
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.Customers.Remove(entity);

        entity.AddDomainEvent(new CustomerDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);
    }

}