namespace InventoryService.Application.Interfaces;

public interface IEventConsumer
{
    Task ConsumeAsync<T>(Func<T, Task> onMessage, string queueName, string exchangeName, string routingKey,CancellationToken cancellationToken)
        where T : class;
}