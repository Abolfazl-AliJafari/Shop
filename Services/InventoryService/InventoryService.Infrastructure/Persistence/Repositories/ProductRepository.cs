
using Ardalis.Specification.EntityFrameworkCore;
using InventoryService.Application.Interfaces;
using InventoryService.Domain.Entities;

namespace InventoryService.Infrastructure.Persistence.Repositories;

public class ProductRepository(InventoryServiceDbContext dbContext) : RepositoryBase<Product>(dbContext),IProductRepository 
{
    public void Add(Product product)
    {
        dbContext.Add(product);
    }
}