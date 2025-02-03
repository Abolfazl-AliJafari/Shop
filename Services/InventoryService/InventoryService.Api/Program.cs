using FastEndpoints;
using FastEndpoints.Swagger;
using InventoryService.Api.HostedServices;
using InventoryService.Application;
using InventoryService.Infrastructure.MassTransit;
using InventoryService.Infrastructure.Persistence;
using InventoryService.Infrastructure.RabbitMQ;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterMassTransitConsumerServices(builder.Configuration)
    .RegisterApplicationServices(builder.Configuration)
    .RegisterPersistenceServices(builder.Configuration);
builder.Services.AddFastEndpoints().SwaggerDocument();

// builder.Services.AddHostedService<CreateOrderEventConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseFastEndpoints(c =>
{
    c.Versioning.Prefix = "v";
}).UseSwaggerGen();
app.Run();