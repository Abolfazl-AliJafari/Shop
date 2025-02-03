namespace InventoryService.Domain.Events;

public record ProductDecreasedStockEvent(Guid Id , int Count);