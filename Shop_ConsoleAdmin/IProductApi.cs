using RestEase;
using Shop_ConsoleAdmin.Models;

namespace Shop_ConsoleAdmin;

public interface IProductApi
{
 [Post("api/v1/products")]
 Task<CreateProductResponse> CreateProduct([Body] Product product);
}