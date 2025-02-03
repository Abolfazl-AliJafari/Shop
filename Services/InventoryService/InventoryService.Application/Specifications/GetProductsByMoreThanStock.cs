using Ardalis.Specification;
using InventoryService.Domain.Entities;

namespace InventoryService.Application.Specifications;

public class GetProductsByMoreThanStock : Specification<Product>
{
    public GetProductsByMoreThanStock(int count)
    {
        Query.Where(p => p.Stock > count);
    }
}