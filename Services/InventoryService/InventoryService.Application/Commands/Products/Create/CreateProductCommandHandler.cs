using InventoryService.Application.Interfaces;
using InventoryService.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace InventoryService.Application.Commands.Products.Create;

public class CreateProductCommandHandler(IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CreateProductCommand,Guid>
{
    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = mapper.Map<Product>(request);
        unitOfWork.ProductRepository.Add(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return product.Id;
    }
}