using Domain.Entities;
using SharedContracts.Result;

namespace Application.Interfaces.Rules
{
    /// <summary>
    /// Interface for business rules. Each rule should implement this interface.
    /// </summary>
    public interface IRule
    {
        bool IsApplicable(PurchaseOrder order);
        Task<Result> ApplyAsync(PurchaseOrder order);
    }
}
