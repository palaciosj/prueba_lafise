using Application.Common.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Transactions.Queries.GetTransactionSummary;

public record GetTransactionSummaryQuery(int BankAccountId) : IRequest<TransactionSummaryDto>;

public record TransactionSummaryDto(string AccountNumber, double Balance, List<TransactionDto> Transactions);

public record TransactionDto(int Id, TransactionType Type, double Amount);

public class GetTransactionSummaryQueryHandler : IRequestHandler<GetTransactionSummaryQuery, TransactionSummaryDto>
{
    private readonly IApplicationDbContext _context;

    public GetTransactionSummaryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TransactionSummaryDto> Handle(GetTransactionSummaryQuery request, CancellationToken cancellationToken)
    {
        var account = await _context.BankAccounts
            .Include(b => b.Transactions)
            .FirstOrDefaultAsync(b => b.Id == request.BankAccountId, cancellationToken);

        if (account is null)
            throw new KeyNotFoundException("Cuenta bancaria no encontrada.");

        var transactions = account.Transactions
            .OrderBy(t => t.Id)
            .Select(t => new TransactionDto(t.Id, t.Type, t.Amount))
            .ToList();

        return new TransactionSummaryDto(account.AccountNumber, account.Balance, transactions);
    }
}
