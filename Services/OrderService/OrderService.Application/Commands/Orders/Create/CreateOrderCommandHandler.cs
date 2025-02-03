using Contract.Events;
using MapsterMapper;
using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;

namespace OrderService.Application.Commands.Orders.Create;

public class CreateOrderCommandHandler(IUnitOfWork unitOfWork,
    IMapper mapper,
    IMassTransitPublisher massTransitPublisher) : IRequestHandler<CreateOrderCommand,Guid>
{
    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = mapper.Map<Order>(request);
        unitOfWork.OrderRepository.Add(order);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var orderCreatedEvent = mapper.Map<OrderCreatedEvent>(order);
        await massTransitPublisher.PublishAsync(orderCreatedEvent,"order_created_queue",cancellationToken);
        return order.Id;
    }
}