using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Events.CustomerEvents;
using MediatR;

namespace Application.Customers.Commands.CreateCustomer;

public record CreateCustomerCommand : IRequest<int>
{
    public required string Name { get; init; }
    public DateTime BirthDate { get; init; }
    public Gender Gender { get; init; }
    public double Incomes { get; init; }
}

public class CreateCustomerCommandHandler(IApplicationDbContext _context) : IRequestHandler<CreateCustomerCommand, int> 
{
    //private readonly IApplicationDbContext _context;

    public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = new Customer{
            Name = request.Name,
            BirthDate = request.BirthDate,
            Gender = request.Gender,
            Incomes = request.Incomes,
        };

        entity.AddDomainEvent(new CustomerCreatedEvent(entity));

        _context.Customers.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

}