namespace Domain.Entities
{
    public class PurchaseOrder
    {
        public long PurchaseOrderId { get; set; }

        public decimal Total { get; set; }

        public long CustomerId { get; set; }

        public List<PurchaseOrderLine> ItemLines { get; set; } = new();
    }
}