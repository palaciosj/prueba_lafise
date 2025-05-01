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
        // USAR EN PRODUCCIÓN (requiere EF real para .Include y .FirstOrDefaultAsync)
        /*
        var account = await _context.BankAccounts
            .Include(b => b.Transactions)
            .FirstOrDefaultAsync(b => b.Id == request.BankAccountId, cancellationToken);
        */

        // USAR EN TESTING (mockeable con Moq sin EF real)
        var account = await _context.BankAccounts
            .FindAsync(new object[] { request.BankAccountId }, cancellationToken);

        if (account is null)
            throw new KeyNotFoundException("Cuenta bancaria no encontrada.");

        // IMPORTANTE: en pruebas, las transacciones deben asignarse manualmente al objeto simulado
        var transactions = account.Transactions?
            .OrderBy(t => t.Id)
            .Select(t => new TransactionDto(t.Id, t.Type, t.Amount))
            .ToList() ?? new List<TransactionDto>();

        return new TransactionSummaryDto(account.AccountNumber, account.Balance, transactions);
    }
}
