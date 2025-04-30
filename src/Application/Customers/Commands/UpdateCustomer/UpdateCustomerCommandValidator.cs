using Application.Common.Interfaces;
using FluentValidation;

namespace Application.Customers.Commands.UpdateCustomer;

public class UpdateCustomerValidator :  AbstractValidator<UpdateCustomerCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCustomerValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("El ID del cliente es requerido");

        RuleFor(x => x.Name)
            .NotEmpty()
            .When(x => x.Name is not null)
            .WithMessage("El nombre no puede estar vacío");

        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.Today)
            .When(x => x.BirthDate is not null)
            .WithMessage("La fecha debe ser válida");

        RuleFor(x => x.Gender)
            .IsInEnum()
            .When(x => x.Gender is not null)
            .WithMessage("El género es inválido");

        RuleFor(x => x.Incomes)
            .GreaterThan(0)
            .When(x => x.Incomes is not null)
            .WithMessage("Los ingresos deben ser mayores a 0");
    }
}