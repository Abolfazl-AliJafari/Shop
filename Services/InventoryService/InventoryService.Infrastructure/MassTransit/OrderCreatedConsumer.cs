using Contract.Events;
using InventoryService.Application.Commands.Products.DecreaseStock;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryService.Infrastructure.MassTransit;

public class OrderCreatedConsumer(ISender sender,ILogger<OrderCreatedConsumer> logger) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var @event = context.Message;
        var command = new DecreaseProductStockCommand(@event.ProductId, @event.ProductCount);
        await sender.Send(command);
        logger.LogInformation($"Consumed: {@event}");
    }
}