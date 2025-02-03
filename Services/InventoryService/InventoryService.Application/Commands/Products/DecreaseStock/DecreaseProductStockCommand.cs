using MediatR;

namespace InventoryService.Application.Commands.Products.DecreaseStock;

public record DecreaseProductStockCommand(Guid Id, int Count) : IRequest;