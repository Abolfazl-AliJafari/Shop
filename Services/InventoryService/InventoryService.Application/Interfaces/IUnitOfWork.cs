namespace InventoryService.Application.Interfaces;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    public IProductRepository ProductRepository { get; set; }
    /// <summary>
    /// Asynchronously saves all changes made within the unit of work to the data store.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation (optional).</param>
    /// <returns>
    /// A Task representing the asynchronous operation. The task result contains a Result indicating
    /// the success or failure of the save operation.
    /// </returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}