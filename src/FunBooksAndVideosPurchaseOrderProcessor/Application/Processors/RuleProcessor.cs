using Application.Interfaces.Rules;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using SharedContracts.Result;

namespace Application.Processors;

/// <summary>
/// Processes purchase orders by applying business rules.
/// </summary>
public class RuleProcessor : IRuleProcessor
{
    private readonly IEnumerable<IRule> _rules;
    private readonly ILogger<RuleProcessor> _logger;

    public RuleProcessor(
        IEnumerable<IRule> rules,
        ILogger<RuleProcessor> logger)
    {
        _rules = rules;
        _logger = logger;
    }

    public async Task<Result> ExecuteRulesAsync(PurchaseOrder order)
    {
        var failedRules = new List<string>();

        try
        {
            foreach (var rule in _rules.Where(r => r.IsApplicable(order)))
            {
                var result = await rule.ApplyAsync(order);

                if (!result.IsSuccess)
                {
                    failedRules.Add(rule.GetType().Name);
                    _logger.LogWarning("Rule {RuleName} failed for order {OrderId}: {Error}",
                        rule.GetType().Name, order.PurchaseOrderId, result.Error);
                }
                else
                {
                    _logger.LogInformation("Rule {RuleName} applied successfully for order {OrderId}",
                        rule.GetType().Name, order.PurchaseOrderId);
                }
            }

            if (failedRules.Any())
                return Result.Fail($"Failed rules: {string.Join(", ", failedRules)}");

            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Error processing rules: {ex.Message}");
        }
    }
}
