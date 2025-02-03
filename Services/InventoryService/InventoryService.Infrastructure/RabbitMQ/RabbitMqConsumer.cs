using System.Text;
using InventoryService.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InventoryService.Infrastructure.RabbitMQ;

public class RabbitMqConsumer(IConfiguration configuration) : IEventConsumer, IDisposable
{
    private IConnection? _connection;

    private async Task<IConnection> GetConnectionAsync()
    {
        if (_connection is not { IsOpen: true })
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration["RabbitMQ:Host"]!,
                Port = int.Parse(configuration["RabbitMQ:Port"]!),
                UserName = configuration["RabbitMQ:Username"],
                Password = configuration["RabbitMQ:Password"]
            };
            _connection = await factory.CreateConnectionAsync();
        }

        return _connection;
    }

    public async Task ConsumeAsync<T>(Func<T, Task> onMessage, string queueName, string exchangeName, string routingKey,CancellationToken cancellationToken)
        where T : class
    {
        var connection = await GetConnectionAsync();
        var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        try
        {
            await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Direct, durable: false, cancellationToken: cancellationToken);
            
            await channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false, autoDelete: false,
                arguments: null, cancellationToken: cancellationToken);
            await channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: routingKey , cancellationToken:cancellationToken);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (sender, args) =>
            {
                try
                {
                    var body = args.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var @event = JsonConvert.DeserializeObject<T>(message);
                    if (@event != null)
                    {
                        await onMessage(@event);
                    }

                    await channel.BasicAckAsync(args.DeliveryTag, multiple: false , cancellationToken : cancellationToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                }
            };

            await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer,cancellationToken : cancellationToken);

        }
        finally
        {
            await channel.CloseAsync(cancellationToken);
        }
    }

    public void Dispose()
    {
        _connection?.Dispose();
        GC.SuppressFinalize(this);
    }
}