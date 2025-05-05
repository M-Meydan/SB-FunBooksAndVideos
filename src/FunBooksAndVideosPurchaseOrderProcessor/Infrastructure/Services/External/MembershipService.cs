using Application.Interfaces.External;
using Microsoft.Extensions.Logging;
using SharedContracts.Enums;
using SharedContracts.Result;

namespace Infrastructure.Services.External;

/// <summary>
/// Service for activating memberships. This would make a call to customer service to activate memberships or publish an event for it.
/// </summary>
public class MembershipService : IMembershipService
{
    private readonly ILogger<MembershipService> _logger;

    public MembershipService(ILogger<MembershipService> logger)
    {
        _logger = logger;
    }

    public Task<Result> ActivateMemberships(long purchaseOrderId, long customerId, List<MembershipType> membershipTypes)
    {
        //call customer service to activate memberships or publish an event for it

        //Log information
        _logger.LogInformation($"Activating memberships for order {purchaseOrderId}, customer {customerId}");

        return Task.FromResult(Result.Ok());
    }
}
