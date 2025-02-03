using Contract.Events;
using InventoryService.Application.Commands.Products.DecreaseStock;
using InventoryService.Application.Interfaces;
using MediatR;

namespace InventoryService.Api.HostedServices;

public class CreateOrderEventConsumer(
    ILogger<CreateOrderEventConsumer> logger,
    IEventConsumer eventConsumer,
    ISender sender)
    : IHostedService
{

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting RabbitMQ Consumer Hosted Service.");
        
        const string queueName = "order_queue"; 
        const string exchangeName = "order_exchange";
        const string routingKey = "order.created";

        await eventConsumer.ConsumeAsync<object>(async (message) =>
        {
            if (message is not OrderCreatedEvent @event)
            {
                return;
            }
            var command = new DecreaseProductStockCommand(@event.ProductId, @event.ProductCount);
            await sender.Send(command, cancellationToken);
            logger.LogInformation($"Message received: {message}");
        }, queueName, exchangeName, routingKey,cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping RabbitMQ Consumer Hosted Service.");
        return Task.CompletedTask;
    }
}