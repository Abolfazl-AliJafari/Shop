namespace Contract.Events;

public class OrderCreatedEvent
{
    public OrderCreatedEvent(Guid orderId, Guid productId,int productCount, DateTime orderDate)
    {
        OrderId = orderId;
        ProductId = productId;
        ProductCount = productCount;
        OrderDate = orderDate;
    }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int ProductCount { get; set; }
    public DateTime OrderDate { get; set; }
}