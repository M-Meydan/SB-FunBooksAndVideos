using Application.Interfaces.External;
using Application.Interfaces.Rules;
using Domain.Entities;
using SharedContracts.Enums;
using SharedContracts.Result;

namespace Application.Rules;


/// <summary>
/// Business rule to implement for shipping slip generation.
/// </summary>
public class ShippingSlipRuleHandler : IRule
{
    private readonly IShippingSlipService _shippingSlipService;

    public ShippingSlipRuleHandler(IShippingSlipService shippingSlipService)
    {
        _shippingSlipService = shippingSlipService;
    }

    /// <summary>
    /// Checks if the rule is applicable to the purchase order.
    /// // According to the spec, we're assuming books are physical products that need shipping
    /// </summary>
    public bool IsApplicable(PurchaseOrder order)
    {
        return order.ItemLines.Any(i => i.ProductType == ProductType.Book);
    }

    /// <summary>
    /// Applies the shipping slip generation rule to the purchase order.
    /// </summary>
    public async Task<Result> ApplyAsync(PurchaseOrder order)
    {
        var physicalProducts = order.ItemLines.Where(i => i.ProductType == ProductType.Book).ToList();

        if (physicalProducts.Any())
            return await _shippingSlipService.GenerateShippingSlipAsync(order.PurchaseOrderId, order.CustomerId, physicalProducts);

        return Result.Ok();
    }
}
