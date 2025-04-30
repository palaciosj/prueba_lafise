using Application.Common.Interfaces;
using Domain.Events.BankAccountEvents;
using Ardalis.GuardClauses;
using MediatR;

namespace Application.BankAccounts.Commands.DeleteBankAccount;

public record DeleteBankAccountCommand(int Id) : IRequest;

public class DeleteBankAccountCommandHandler(IApplicationDbContext _context) : IRequestHandler<DeleteBankAccountCommand>
{
    public async Task Handle(DeleteBankAccountCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.BankAccounts
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.BankAccounts.Remove(entity);

        entity.AddDomainEvent(new BankAccountDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);
    }
}
