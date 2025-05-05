using Domain.Entities;

namespace Application.Interfaces.Processors
{
    public interface IPurchaseOrderProcessor
    {
        Task ProcessOrderAsync(PurchaseOrder order);
    }
}