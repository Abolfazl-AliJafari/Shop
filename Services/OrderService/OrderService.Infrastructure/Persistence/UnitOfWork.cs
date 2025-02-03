using OrderService.Application.Interfaces;
using OrderService.Application.Interfaces.IRepositories;

namespace OrderService.Infrastructure.Persistence;

public class UnitOfWork(OrderServiceDbContext dbContext,
    IOrderRepository orderRepository) : IUnitOfWork
{
    public IOrderRepository OrderRepository { get; set; } = orderRepository;
    public void Dispose()
    {
        dbContext.Dispose();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await dbContext.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}