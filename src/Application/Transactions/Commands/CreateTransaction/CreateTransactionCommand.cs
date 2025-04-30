using Application.Common.Interfaces;
using Ardalis.GuardClauses;
using Domain.Entities;
using Domain.Enums;
using Domain.Events.TransactionEvents;
using MediatR;

namespace Application.Transactions.Commands.CreateTransaction;

public record CreateTransactionCommand : IRequest<int>
{
    public double Amount { get; init; }
    public TransactionType Type { get; init; }
    public int BankAccountId { get; init; }
}

public class CreateTransactionCommandHandler(IApplicationDbContext _context) : IRequestHandler<CreateTransactionCommand, int>
{
    public async Task<int> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var account = await _context.BankAccounts
            .FindAsync(new object[] { request.BankAccountId }, cancellationToken);

        Guard.Against.NotFound(request.BankAccountId, account);

        var transaction = new Transaction
        {
            Amount = request.Amount,
            Type = request.Type,
            BankAccountId = request.BankAccountId
        };

        transaction.AddDomainEvent(new TransactionCreatedEvent(transaction));
        _context.Transactions.Add(transaction);

        if (request.Type == TransactionType.Deposit)
            account.Balance += request.Amount;
        else if (request.Type == TransactionType.Withdraw)
            account.Balance -= request.Amount;

        await _context.SaveChangesAsync(cancellationToken);

        return transaction.Id;
    }
}
