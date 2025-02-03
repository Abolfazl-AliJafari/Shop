using Microsoft.OpenApi.Models;
using OrderService.Application;
using OrderService.Infrastructure.MassTransit;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Order API", Version = "v1" });

});
builder.Services.RegisterMassTransitServices(builder.Configuration)
    .RegisterApplicationServices(builder.Configuration)
    .RegisterPersistenceServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API V1");
    });
}
app.MapControllers();

app.UseHttpsRedirection();

app.Run();
