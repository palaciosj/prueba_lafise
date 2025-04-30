using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events.BankAccountEvents;
using MediatR;

namespace Application.BankAccounts.Commands.CreateBankAccount;

public record CreateBankAccountCommand : IRequest<int>
{
    public required string AccountNumber { get; init; }
    public double Balance { get; init; }
    public int CustomerId { get; init; }
}

public class CreateBankAccountCommandHandler(IApplicationDbContext _context) : IRequestHandler<CreateBankAccountCommand, int>
{
    public async Task<int> Handle(CreateBankAccountCommand request, CancellationToken cancellationToken)
    {
        var entity = new BankAccount
        {
            AccountNumber = request.AccountNumber,
            Balance = request.Balance,
            CustomerId = request.CustomerId
        };

        entity.AddDomainEvent(new BankAccountCreatedEvent(entity));

        _context.BankAccounts.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
