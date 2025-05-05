using Domain.Entities;
using SharedContracts.Result;

namespace Application.Interfaces.External
{
    public interface IShippingSlipService
    {
        Task<Result> GenerateShippingSlipAsync(long purchaseOrderId, long customerId, List<PurchaseOrderLine> physicalProducts);
    }
}