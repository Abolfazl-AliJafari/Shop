using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Api.VMs.Orders;

namespace OrderService.Api.Controllers.v1;
[ApiController]
[Route("api/v1/[controller]")]
public class OrdersController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody]CreateOrderVM order,CancellationToken cancellationToken)
    {
        var command = order.ToCommand();
        var result = await sender.Send(command, cancellationToken);
        return Ok(result);
    }
}