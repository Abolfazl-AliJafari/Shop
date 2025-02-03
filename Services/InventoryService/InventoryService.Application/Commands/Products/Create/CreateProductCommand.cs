using MediatR;

namespace InventoryService.Application.Commands.Products.Create;

public record CreateProductCommand(string Title,int Stock) 
    : IRequest<Guid>;