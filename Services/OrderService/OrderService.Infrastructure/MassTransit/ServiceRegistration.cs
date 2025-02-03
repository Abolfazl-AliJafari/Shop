using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Interfaces;

namespace OrderService.Infrastructure.MassTransit;

public static class ServiceRegistration
{
    public static IServiceCollection RegisterMassTransitServices(this IServiceCollection services, IConfiguration configuration)
    {
       
        services.AddScoped<IMassTransitPublisher, MassTransitPublisher>();

        services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMQ:Host"], h =>
                {
                    h.Username(configuration["RabbitMQ:Username"]!);
                    h.Password(configuration["RabbitMQ:Password"]!);
                });
            });
        });

        return services;
    }
}