using RestEase;
using Shop_ConsoleClient.Models;

namespace Shop_ConsoleClient;

public interface IOrderApi
{
    [Post("api/v1/orders")]
    Task<Guid> CreateOrder([Body] Order order);
}