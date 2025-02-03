using InventoryService.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryService.Infrastructure.RabbitMQ;

public static class ServiceRegistration
{
    public static IServiceCollection RegisterRabbitMqServices(this IServiceCollection services)
    {
        services.AddSingleton<IEventConsumer, RabbitMqConsumer>();
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