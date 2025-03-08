using System.Net;
using System.Net.Http.Json;
using Contract.Events;
using MassTransit;
using MassTransit.Internals;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using OrderService.Application.Commands.Orders.Create;
using OrderService.Infrastructure.Persistence;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace OrderService.IntegrationTests;

public class OrderServiceIntegrationTests: IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer;
    private readonly RabbitMqContainer _rabbitMqContainer;
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;
    private OrderServiceDbContext _dbContext;

    public OrderServiceIntegrationTests()
    {
        _postgresContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15-alpine")
            .WithDatabase("test_db")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        _rabbitMqContainer = new RabbitMqBuilder()
            .WithImage("rabbitmq:3-management")
            .WithPortBinding(5672, true)
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();
        await _rabbitMqContainer.StartAsync();

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<OrderServiceDbContext>));

                    if (descriptor != null)
                        services.Remove(descriptor);

                    services.AddDbContext<OrderServiceDbContext>(options =>
                        options.UseNpgsql(_postgresContainer.GetConnectionString()));
                });

                builder.ConfigureServices(services =>
                {
                    services.AddMassTransitTestHarness(cfg =>
                    {
                        cfg.UsingRabbitMq((ctx, rmqCfg) =>
                        {
                            rmqCfg.Host(new Uri(_rabbitMqContainer.GetConnectionString()));
                        });
                    });
                });
            });

        _client = _factory.CreateClient();
        _dbContext = _factory.Services.GetRequiredService<OrderServiceDbContext>();
        await _dbContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgresContainer.DisposeAsync();
        await _rabbitMqContainer.DisposeAsync();
        await _dbContext.Database.EnsureDeletedAsync();
        await _factory.DisposeAsync();
    }

    [Fact]
    public async Task CreateOrder_ValidRequest_ShouldSaveToDbAndPublishEvent()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var request = new CreateOrderCommand(
            ProductId: productId,
            ProductCount: 5,
            State: "Pending"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/orders", request);

        // Assert 1
        response.EnsureSuccessStatusCode();
        var orderId = await response.Content.ReadFromJsonAsync<Guid>();
        Assert.NotEqual(Guid.Empty, orderId);

        // Assert 2
        var dbOrder = await _dbContext.Orders.FindAsync(orderId);
        Assert.NotNull(dbOrder);
        Assert.Equal(productId, dbOrder.ProductId);
        Assert.Equal(5, dbOrder.ProductCount);
        Assert.Equal("Pending", dbOrder.State);

        // Assert 3
        var testHarness = _factory.Services.GetRequiredService<ITestHarness>();
        Assert.True(await testHarness.Published.Any<OrderCreatedEvent>());

        var events = await testHarness.Published.SelectAsync<OrderCreatedEvent>().ToListAsync();
        var @event = events.First().MessageObject as OrderCreatedEvent;
        Assert.Equal(orderId, @event?.OrderId);
        Assert.Equal(productId, @event?.ProductId);
        Assert.Equal(5, @event?.ProductCount);
        Assert.Equal(dbOrder.CreatedAt, @event?.OrderDate);
    }

    [Fact]
    public async Task CreateOrder_InvalidProductCount_ShouldReturnBadRequest()
    {
        // Arrange
        var invalidRequest = new CreateOrderCommand(
            ProductId: Guid.NewGuid(),
            ProductCount: -1, 
            State: "Pending"
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/orders", invalidRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Empty(_dbContext.Orders);
    }
}