using SharedContracts.Enums;

namespace Domain.Entities
{
    public class PurchaseOrderLine
    {
        public long PurchaseOrderId { get; set; }
        public string ProductName { get; set; }
        public ProductType ProductType { get; set; }
        public MembershipType? MembershipType { get; set; }

        public PurchaseOrderLine(long purchaseOrderId, string productName, ProductType productType, MembershipType? membershipType = null)
        {
            PurchaseOrderId = purchaseOrderId;
            ProductName = productName;
            ProductType = productType;
            MembershipType = membershipType;
        }
    }

}