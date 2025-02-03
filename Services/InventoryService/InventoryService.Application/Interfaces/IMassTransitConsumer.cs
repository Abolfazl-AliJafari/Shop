using MassTransit;

namespace InventoryService.Application.Interfaces;

public interface IMassTransitConsumer<T> : IConsumer<T> where T : class 
{
}