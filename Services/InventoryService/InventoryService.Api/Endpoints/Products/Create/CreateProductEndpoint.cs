using FastEndpoints;
using MediatR;

namespace InventoryService.Api.Endpoints.Products.Create;

public class CreateProductEndpoint(ISender sender) : Endpoint<CreateProductRequest,CreateProductResponse>
{
    public override void Configure()
    {
        Post("api/v1/products");
        Description(p => p.WithTags("Products"));
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateProductRequest req, CancellationToken ct)
    {
        var command = req.ToCommand();
        var result = await sender.Send(command, ct);
        Response = new CreateProductResponse(result);
    }
}