using Contract.Events;
using Mapster;
using OrderService.Application.Commands.Orders.Create;
using OrderService.Domain.Entities;

namespace OrderService.Application.Mapping;

public class OrderMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<Order, OrderCreatedEvent>()
            .Map(dest => dest.OrderId, src => src.Id)
            .Map(dest => dest.ProductId, src => src.ProductId)
            .Map(dest => dest.ProductCount, src => src.ProductCount)
            .Map(dest => dest.OrderDate, src => src.CreatedAt);
        config.ForType<Order, CreateOrderCommand>()
            .Map(dest => dest.ProductId, src => src.ProductId)
            .Map(dest => dest.ProductCount, src => src.ProductCount)
            .Map(dest => dest.State, src => src.State);
    }
}