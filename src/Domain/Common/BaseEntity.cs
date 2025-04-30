using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }
    private readonly List<BaseEvent> _domainEvents = [];

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvents(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void CrearDomainEvents()
    {
        _domainEvents.Clear();
    }
}