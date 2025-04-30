using Application.Common.Interfaces;
using FluentValidation;

namespace Application.Customers.Commands.CreateCustomer;

public class CreateCustomerValidator :  AbstractValidator<CreateCustomerCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateCustomerValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .NotEmpty()
            .WithMessage("El nombre del cliente es requerido");

        RuleFor(v => v.BirthDate)
            .NotEmpty()
            .WithMessage("La fecha de nacimiento es requerida")
            .LessThan(DateTime.Today)
            .WithMessage("La fecha de nacimiento debe ser anterior a hoy");

        RuleFor(v => v.Gender)
            .IsInEnum()
            .WithMessage("El género es requerido");
        
        RuleFor(v => v.Incomes)
            .GreaterThan(0)
            .WithMessage("Los ingresos deben de ser mayor que 0");
    }
}