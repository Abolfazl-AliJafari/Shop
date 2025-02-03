using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Interfaces;
using OrderService.Application.Interfaces.IRepositories;
using OrderService.Infrastructure.Persistence.Repositories;

namespace OrderService.Infrastructure.Persistence;

public static class ServiceRegistration
{
    public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgresql");
        services.AddDbContext<OrderServiceDbContext>(options =>
        {
            options.UseNpgsql(connectionString, builder => builder.MigrationsAssembly(typeof(OrderServiceDbContext).Assembly.FullName));
        });
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}