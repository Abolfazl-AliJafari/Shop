using InventoryService.Application.Commands.Products.Create;
using InventoryService.Domain.Entities;
using Mapster;

namespace InventoryService.Application.Mapping;

public class ProductMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<Product, CreateProductCommand>()
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Stock, src => src.Stock);
        config.ForType<CreateProductCommand, Product>()
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Stock, src => src.Stock);
    }
}