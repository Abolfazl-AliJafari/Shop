using OrderService.Application.Interfaces.IRepositories;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Persistence.Repositories;

public class OrderRepository(OrderServiceDbContext dbContext) : IOrderRepository
{
    public void Add(Order order)
    {
        dbContext.Add(order);
    }
}