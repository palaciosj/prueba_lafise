using Domain.Common;
using Domain.Entities;

namespace Domain.Events.BankAccountEvents;

public class BankAccountCreatedEvent(BankAccount bankAccount) : BaseEvent 
{
    public BankAccount BankAccount { get; } = bankAccount;
}