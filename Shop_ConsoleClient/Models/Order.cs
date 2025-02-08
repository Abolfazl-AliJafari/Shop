namespace Shop_ConsoleClient.Models;

public class Order
{
    public Guid ProductId { get; set; }
    public int ProductCount { get; set; }
    public string State { get; set; }
}