using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrderService.Application.Interfaces;
using RabbitMQ.Client;

namespace OrderService.Infrastructure.RabbitMQ;

public static class ServiceRegistration
{
    public static IServiceCollection RegisterRabbitMqServices(this IServiceCollection services)
    {
        services.AddScoped<IEventPublisher, RabbitMqPublisher>();
        // services.AddSingleton<IConnection>( sp => 
        // {
        //     var rabbitMqConfig = sp.GetRequiredService<IOptions<RabbitMqConfiguration>>().Value;
        //
        //     var factory = new ConnectionFactory
        //     {
        //         HostName = rabbitMqConfig.Host,
        //         Port = rabbitMqConfig.Port,
        //         UserName = rabbitMqConfig.Username,
        //         Password = rabbitMqConfig.Password
        //     };
        //
        //     return Task.Run(() => factory.CreateConnectionAsync().GetAwaiter().GetResult()).Result;
        // });
        return services;
    }
}