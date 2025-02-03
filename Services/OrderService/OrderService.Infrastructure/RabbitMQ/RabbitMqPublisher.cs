using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OrderService.Application.Interfaces;

namespace OrderService.Infrastructure.RabbitMQ;

public class RabbitMqPublisher(IConfiguration configuration) : IEventPublisher
{
    private IConnection? _connection;

    private async Task<IConnection> GetConnectionAsync(CancellationToken cancellationToken)
    {
        if (_connection is not { IsOpen: true })
        {
            var factory = new ConnectionFactory 
                { 
                    HostName = configuration["RabbitMQ:Host"]!,
                    Port = int.Parse(configuration["RabbitMQ:Port"]!) ,
                    UserName = configuration["RabbitMQ:Username"],
                    Password = configuration["RabbitMQ:Password"]
                };
            _connection = await factory.CreateConnectionAsync(cancellationToken);
        }
        return _connection;
    }

    public async Task PublishAsync<T>(T @event,string exchangeName, string routingKey,CancellationToken cancellationToken)where T : class
    {
        var connection = await GetConnectionAsync(cancellationToken);
        var channel = await connection.CreateChannelAsync(cancellationToken : cancellationToken);

        try
        {
            await channel.ExchangeDeclareAsync(exchangeName,type: ExchangeType.Direct, cancellationToken : cancellationToken);
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
            await channel.BasicPublishAsync(exchange: exchangeName, routingKey: routingKey, mandatory: true, body: body,cancellationToken : cancellationToken);
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