
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryService.Infrastructure.MassTransit;

public static class ServiceRegistration
{
    public static IServiceCollection RegisterMassTransitConsumerServices(this IServiceCollection services, IConfiguration configuration)
    {
       
        services.AddMassTransit(config =>
        {
            config.AddConsumer<OrderCreatedConsumer>();
            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration["RabbitMQ:Host"], h =>
                {
                    h.Username(configuration["RabbitMQ:Username"]!);
                    h.Password(configuration["RabbitMQ:Password"]!);
                });
                cfg.ConfigureEndpoints(ctx);
                // cfg.ReceiveEndpoint("order_created_queue", e =>
                // {
                //     e.ConfigureConsumer<OrderCreatedConsumer>(ctx);
                // });
            });
        });
        
        return services;
    }
}