using Domain.Common;
using Domain.Entities;

namespace Domain.Events.CustomerEvents;

public class CustomerCreatedEvent(Customer customer) : BaseEvent 
{
    public Customer Customer { get; } = customer;
}