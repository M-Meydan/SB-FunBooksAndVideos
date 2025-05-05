using Application.Interfaces.External;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using SharedContracts.Result;

namespace Infrastructure.Services.External;

/// <summary>
/// Service for generating shipping slips.This would typically call an external shipping service or publish an event for it.
/// </summary>
public class ShippingSlipService : IShippingSlipService
{
    private readonly ILogger<ShippingSlipService> _logger;

    public ShippingSlipService(ILogger<ShippingSlipService> logger)
    {
        _logger = logger;
    }

    public Task<Result> GenerateShippingSlipAsync(long purchaseOrderId, long customerId, List<PurchaseOrderLine> physicalProducts)
    {
        // In a real implementation, this would call shipping slip service or send an event to generate a shipping slip in a shipping system

        _logger.LogInformation($"Generating shipping slip for order {purchaseOrderId}, customer {customerId}");

        return Task.FromResult(Result.Ok());
    }
}
