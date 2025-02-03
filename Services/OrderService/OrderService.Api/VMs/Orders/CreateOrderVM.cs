using OrderService.Application.Commands.Orders.Create;

namespace OrderService.Api.VMs.Orders;

public record CreateOrderVM(Guid ProductId, int ProductCount, string State)
{
    public CreateOrderCommand ToCommand()
    {
        return new CreateOrderCommand(ProductId, ProductCount, State);
    }
}