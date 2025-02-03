using InventoryService.Application.Interfaces;
using MediatR;

namespace InventoryService.Application.Commands.Products.DecreaseStock;
public class DecreaseProductStockCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DecreaseProductStockCommand>
{
    public async Task Handle(DecreaseProductStockCommand request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product is null)
        {
            return;
        }
        product!.Stock -= request.Count;
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}