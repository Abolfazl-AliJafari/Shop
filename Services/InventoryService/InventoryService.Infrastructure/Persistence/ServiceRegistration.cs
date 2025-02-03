using InventoryService.Application.Interfaces;
using InventoryService.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryService.Infrastructure.Persistence;

public static class ServiceRegistration
{
    public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgresql");
        services.AddDbContext<InventoryServiceDbContext>(options =>
        {
            options.UseNpgsql(connectionString, builder => builder.MigrationsAssembly(typeof(InventoryServiceDbContext).Assembly.FullName));
        });
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProductRepository, ProductRepository>();
        return services;
    }
}