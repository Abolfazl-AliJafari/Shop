using Ardalis.Specification;
using InventoryService.Domain.Entities;

namespace InventoryService.Application.Specifications;

public class GetProductByIdSpecification : Specification<Product>
{
    public GetProductByIdSpecification(Guid id)
    {
         Query.Where(o => o.Id == id);
    }
}