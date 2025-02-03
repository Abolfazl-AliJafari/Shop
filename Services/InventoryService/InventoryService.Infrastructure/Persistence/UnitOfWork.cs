
using InventoryService.Application.Interfaces;

namespace InventoryService.Infrastructure.Persistence;

public class UnitOfWork(InventoryServiceDbContext dbContext,
    IProductRepository productRepository) : IUnitOfWork
{
    public IProductRepository ProductRepository { get; set; } = productRepository;
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