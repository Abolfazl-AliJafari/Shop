using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderService.Api.Controllers.v1;
using OrderService.Api.VMs.Orders;
using OrderService.Application.Commands.Orders.Create;

namespace OrderService.UnitTests.CreateOrderTests;

public class CreateOrderControllerTests
{
    private readonly Mock<ISender> _mediatorMock;
    private readonly OrdersController _controller;

    public CreateOrderControllerTests()
    {
        _mediatorMock = new Mock<ISender>();
        _controller = new OrdersController(_mediatorMock.Object);
    }

    [Fact]
    public async Task CreateOrder_ReturnsOkResult()
    {
        // Arrange
        var productId = Guid.NewGuid();
        const int productCount = 10;
        const string state = "Declined";
        var command = new CreateOrderCommand(productId,productCount,state);
        var expectedOrderId = Guid.NewGuid();
        
        _mediatorMock
            .Setup(m => m.Send(command, default))
            .ReturnsAsync(expectedOrderId);

        // Act
        var result = await _controller.CreateOrder(new CreateOrderVM(productId,productCount,state),CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedOrderId, okResult.Value);
    }
}