using Application.Common.Interfaces;
using Domain.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Transactions.Commands.CreateTransaction;

public class CreateTransactionValidator : AbstractValidator<CreateTransactionCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateTransactionValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Amount)
            .GreaterThan(0)
            .WithMessage("El monto debe ser mayor a 0");

        RuleFor(v => v.Type)
            .IsInEnum()
            .WithMessage("El tipo de transacción no es válido");

        RuleFor(v => v.BankAccountId)
            .GreaterThan(0)
            .WithMessage("El ID de la cuenta es requerido")
            .MustAsync(BankAccountExists)
            .WithMessage("La cuenta bancaria especificada no existe");

        When(v => v.Type == TransactionType.Withdraw, () =>
        {
            RuleFor(v => v)
                .MustAsync(HaveSufficientBalance)
                .WithMessage("El balance de la cuenta es insuficiente para esta transacción");
        });
    }

    private async Task<bool> BankAccountExists(int bankAccountId, CancellationToken cancellationToken)
    {
        return await _context.BankAccounts
            .AnyAsync(b => b.Id == bankAccountId, cancellationToken);
    }

    private async Task<bool> HaveSufficientBalance(CreateTransactionCommand v, CancellationToken cancellationToken)
    {
        var account = await _context.BankAccounts
            .FirstOrDefaultAsync(b => b.Id == v.BankAccountId, cancellationToken);

        if (account is null)
            return false;

        return account.Balance >= v.Amount;
    }
}
