using Domain.Entities;
using Domain.Events.CustomerEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Customers.EventHandlers;

public class CustomerCreatedEventHandler(ILogger<CustomerCreatedEventHandler> logger) : INotificationHandler<CustomerCreatedEvent> 
{
    private readonly ILogger<CustomerCreatedEventHandler> _logger;

    public Task Handle(CustomerCreatedEvent notificacion, CancellationToken cancellationToken)
    {
        _logger.LogInformation("PruebaLAFISE Domain Event: {DomainEvent}", notificacion.GetType().Name);

        return Task.CompletedTask;
    }


}