namespace OrderService.Application.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync<T>(T @event, string exchangeName, string routingKey,CancellationToken cancellationToken) where T : class;
}