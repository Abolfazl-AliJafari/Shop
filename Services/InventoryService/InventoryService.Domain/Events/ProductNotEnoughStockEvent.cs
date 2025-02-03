namespace InventoryService.Domain.Events;

public record ProductNotEnoughStockEvent(Guid Id, int Count);