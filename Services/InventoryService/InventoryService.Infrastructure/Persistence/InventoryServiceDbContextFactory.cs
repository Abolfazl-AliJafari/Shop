using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace InventoryService.Infrastructure.Persistence;

public class InventoryServiceDbContextFactory:IDesignTimeDbContextFactory<InventoryServiceDbContext>
{
    public InventoryServiceDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<InventoryServiceDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=InventoryServiceDb;Username=postgres;Password=13851385Ab");

        return new InventoryServiceDbContext(optionsBuilder.Options);
    }
}