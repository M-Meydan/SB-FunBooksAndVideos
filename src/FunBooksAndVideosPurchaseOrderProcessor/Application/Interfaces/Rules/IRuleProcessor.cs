using Domain.Entities;
using SharedContracts.Result;

namespace Application.Interfaces.Rules
{
    /// <summary>
    /// Interface for processing business rules.
    /// </summary>
    public interface IRuleProcessor
    {
        Task<Result> ExecuteRulesAsync(PurchaseOrder order);
    }

}
