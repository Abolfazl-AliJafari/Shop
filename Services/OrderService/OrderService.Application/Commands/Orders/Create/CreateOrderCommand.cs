using MediatR;

namespace OrderService.Application.Commands.Orders.Create;

public record CreateOrderCommand(Guid ProductId,int ProductCount,string State) : IRequest<Guid>;