using Ardalis.Specification;
using InventoryService.Domain.Entities;

namespace InventoryService.Application.Interfaces;

public interface IProductRepository : IRepositoryBase<Product>
{
    void Add(Product product);
}