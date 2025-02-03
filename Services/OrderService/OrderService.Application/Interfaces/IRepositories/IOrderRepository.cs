using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces.IRepositories;

public interface IOrderRepository
{
    void Add(Order order);
}