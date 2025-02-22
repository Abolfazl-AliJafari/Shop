using Contract.Events;
using MapsterMapper;
using Moq;
using OrderService.Application.Commands.Orders.Create;
using OrderService.Application.Interfaces;
using OrderService.Application.Interfaces.IRepositories;
using OrderService.Domain.Entities;

namespace OrderService.UnitTests.CreateOrderTests;

public class CreateOrderCommandHandlerTests
{
    
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IMassTransitPublisher> _publisherMock = new();
    private readonly Mock<IOrderRepository> _orderRepoMock = new();
    private readonly CreateOrderCommandHandler _handler;

    public CreateOrderCommandHandlerTests()
    {
        _unitOfWorkMock.Setup(u => u.OrderRepository)
            .Returns(_orderRepoMock.Object);

        _handler = new CreateOrderCommandHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _publisherMock.Object
        );
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldSaveOrderAndPublishCorrectEvent()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var command = new CreateOrderCommand(
            ProductId: productId,
            ProductCount: 5,
            State: "Declined"
        );

        var expectedOrder = new Order
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            ProductCount = 5,
            CreatedAt = DateTime.UtcNow,
            State = "Declined"
        };

        var expectedEvent = new OrderCreatedEvent(
            orderId: expectedOrder.Id,
            productId: productId,
            productCount: 5,
            orderDate: expectedOrder.CreatedAt
        );

        // Setup AutoMapper Mocks
        _mapperMock.Setup(m => m.Map<Order>(command))
            .Returns(expectedOrder);

        _mapperMock.Setup(m => m.Map<OrderCreatedEvent>(expectedOrder))
            .Returns(expectedEvent);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _orderRepoMock.Verify(r => 
            r.Add(It.Is<Order>(o =>
                o.Id == expectedOrder.Id &&
                o.ProductId == command.ProductId &&
                o.ProductCount == command.ProductCount &&
                o.State == command.State
            )),Times.Once);

        _unitOfWorkMock.Verify(u => 
            u.SaveChangesAsync(CancellationToken.None), Times.Once);

        _publisherMock.Verify(p => 
            p.PublishAsync(
                It.Is<OrderCreatedEvent>(e =>
                    e.OrderId == expectedOrder.Id &&
                    e.ProductId == command.ProductId &&
                    e.ProductCount == command.ProductCount &&
                    e.OrderDate == expectedOrder.CreatedAt
                ),
                "order_created_queue",
                CancellationToken.None
            ), Times.Once);

        Assert.Equal(expectedOrder.Id, result);
    }
}