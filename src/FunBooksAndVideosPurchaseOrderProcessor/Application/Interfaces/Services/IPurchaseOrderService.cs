using Application.Models.Inputs;
using SharedContracts.Result;

namespace Application.Interfaces.Services;

/// <summary>
/// Service interface for processing purchase orders.
/// </summary>
public interface IPurchaseOrderService
{
    Task<Result> ProcessAsync(PurchaseOrderDto dto);
}
