namespace OrderService.Domain.Entities;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; } 
    public int ProductCount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string State { get; set; }
}