using SharedContracts.Enums;
using SharedContracts.Result;

namespace Application.Interfaces.External
{
    public interface IMembershipService
    {
        Task<Result> ActivateMemberships(long purchaseOrderId, long customerId, List<MembershipType> membershipTypes);
    }
}