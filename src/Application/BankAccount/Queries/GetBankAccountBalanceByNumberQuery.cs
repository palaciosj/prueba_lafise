using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.BankAccounts.Queries.GetBalanceByNumber;

public record GetBankAccountBalanceByNumberQuery(string AccountNumber) : IRequest<BankAccountBalanceDto>;

public record BankAccountBalanceDto(int Id, string AccountNumber, double Balance);

public class GetBankAccountBalanceByNumberQueryHandler
    : IRequestHandler<GetBankAccountBalanceByNumberQuery, BankAccountBalanceDto>
{
    private readonly IApplicationDbContext _context;

    public GetBankAccountBalanceByNumberQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BankAccountBalanceDto> Handle(GetBankAccountBalanceByNumberQuery request, CancellationToken cancellationToken)
    {
        var account = await _context.BankAccounts
            .Where(b => b.AccountNumber == request.AccountNumber)
            .Select(b => new BankAccountBalanceDto(b.Id, b.AccountNumber, b.Balance))
            .FirstOrDefaultAsync(cancellationToken);

        return account ?? throw new KeyNotFoundException("Cuenta bancaria no encontrada.");
    }
}
