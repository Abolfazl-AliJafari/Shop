using MassTransit;
using Microsoft.Extensions.Logging;
using OrderService.Application.Interfaces;

namespace OrderService.Infrastructure.MassTransit;

public class MassTransitPublisher(IPublishEndpoint publishEndpoint,ILogger<MassTransitPublisher> logger) : IMassTransitPublisher
{
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

    public async Task PublishAsync<T>(T @event,string queueName,CancellationToken cancellationToken) where T : class
    {
        // var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
        // {
        //     cfg.Host("rabbitmq://localhost");
        // });
        //
        // var sendEndpoint = await bus.GetSendEndpoint(new Uri($"queue:{queueName}"));
        // await sendEndpoint.Send(@event,cancellationToken);
        try
        {
            await _publishEndpoint.Publish(@event,cancellationToken);
            logger.LogInformation($"Published : {@event}");
        }
        catch (Exception e)
        {
            logger.LogInformation($"Error on publishing.");
            throw;
        }
       
    }
}