using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.BankAccounts.Commands.CreateBankAccount;

public class CreateBankAccountValidator : AbstractValidator<CreateBankAccountCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateBankAccountValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.AccountNumber)
            .NotEmpty()
            .WithMessage("El número de cuenta es requerido")
            .MustAsync(BeUniqueAccountNumber)
            .WithMessage("El número de cuenta ya está en uso");

        RuleFor(v => v.Balance)
            .GreaterThan(0)
            .WithMessage("El balance inicial debe ser mayor que 0");

        RuleFor(v => v.CustomerId)
            .GreaterThan(0)
            .WithMessage("El ID del cliente es obligatorio")
            .MustAsync(CustomerExists)
            .WithMessage("El cliente especificado no existe");
    }

    private async Task<bool> BeUniqueAccountNumber(string accountNumber, CancellationToken cancellationToken)
    {
        return !await _context.BankAccounts
            .AnyAsync(b => b.AccountNumber == accountNumber, cancellationToken);
    }

    private async Task<bool> CustomerExists(int customerId, CancellationToken cancellationToken)
    {
        return await _context.Customers
            .AnyAsync(c => c.Id == customerId, cancellationToken);
    }
}
