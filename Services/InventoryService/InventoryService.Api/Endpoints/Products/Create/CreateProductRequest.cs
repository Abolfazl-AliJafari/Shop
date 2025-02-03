using InventoryService.Application.Commands.Products.Create;

namespace InventoryService.Api.Endpoints.Products.Create;

public record CreateProductRequest(string Title, int Stock)
{
    public CreateProductCommand ToCommand()
    {
        return new CreateProductCommand(Title, Stock);
    }
}