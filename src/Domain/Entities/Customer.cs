using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Customer : BaseAuditableEntity 
{
    public required string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public double Incomes { get; set; }
    public ICollection<BankAccount> BankAccounts { get; set; } = [];
}
