using Domain.Entities;

namespace Domain.Events;

public record ProcessPurchaseOrderMessage(PurchaseOrder order);