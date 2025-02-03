namespace InventoryService.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public int Stock { get; set; }
}