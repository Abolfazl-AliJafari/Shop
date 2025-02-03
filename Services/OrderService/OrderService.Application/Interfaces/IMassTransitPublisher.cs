namespace OrderService.Application.Interfaces;

public interface IMassTransitPublisher
{
   Task PublishAsync<T>(T @event,string queueName,CancellationToken cancellationToken) where T : class;

}