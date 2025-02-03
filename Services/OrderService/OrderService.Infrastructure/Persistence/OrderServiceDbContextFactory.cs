using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OrderService.Infrastructure.Persistence;

public class OrderServiceDbContextFactory:IDesignTimeDbContextFactory<OrderServiceDbContext>
{
    public OrderServiceDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<OrderServiceDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=OrderServiceDb;Username=postgres;Password=13851385Ab");

        return new OrderServiceDbContext(optionsBuilder.Options);
    }
}