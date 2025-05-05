using Application.Interfaces.External;
using Application.Interfaces.Rules;
using Domain.Entities;
using SharedContracts.Enums;
using SharedContracts.Result;

namespace Application.Rules;

/// <summary>
/// Business rule to implement memberships activation.
/// </summary>
public class MembershipActivationRuleHandler : IRule
{
    private readonly IMembershipService _membershipActivationService;

    public MembershipActivationRuleHandler(IMembershipService membershipActivationService)
    {
        _membershipActivationService = membershipActivationService;
    }

    /// <summary>
    /// Checks if the rule is applicable to the purchase order by checking if any item line has a Membership product.
    /// </summary>
    public bool IsApplicable(PurchaseOrder order)
    {
        return order.ItemLines.Any(i => i.ProductType == ProductType.Membership);
    }

    /// <summary>
    /// Applies the membership activation rule to the purchase order.
    /// </summary>
    public async Task<Result> ApplyAsync(PurchaseOrder order)
    {
        // Get all Membership products
        var memberships = order.ItemLines
            .Where(i => i.ProductType == ProductType.Membership && i.MembershipType.HasValue)
            .Select(i => new Membership { MembershipType = i.MembershipType.Value })
            .ToList();

        if (memberships.Any())
        {
            // Extract MembershipTypes
            var membershipTypes = memberships.Select(m => m.MembershipType).ToList();
            return await _membershipActivationService.ActivateMemberships(order.PurchaseOrderId, order.CustomerId, membershipTypes);
        }

        return Result.Ok();
    }
}
        
