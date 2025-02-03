using InventoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Infrastructure.Persistence;

public class InventoryServiceDbContext(DbContextOptions<InventoryServiceDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    
}