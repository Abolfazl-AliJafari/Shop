using OrderService.Application.Interfaces.IRepositories;

namespace OrderService.Application.Interfaces;

/// <summary>
/// Defines the contract for a unit of work that manages transaction-like operations
/// and provides methods to persist changes to the data store.
/// </summary>
public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    public IOrderRepository OrderRepository { get; set; }
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