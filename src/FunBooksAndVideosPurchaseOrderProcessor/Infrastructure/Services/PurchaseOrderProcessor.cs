using Application.Interfaces.Processors;
using Application.Interfaces.Rules;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services;


/// <summary>
/// Service for processing purchase orders.
/// </summary>
public class PurchaseOrderProcessor : IPurchaseOrderProcessor
{
    private readonly IEnumerable<IRule> _rules;
    private readonly ILogger<IPurchaseOrderProcessor> _logger;

    public PurchaseOrderProcessor(
        IEnumerable<IRule> rules,
        ILogger<IPurchaseOrderProcessor> logger)
    {
        _rules = rules;
        _logger = logger;
    }

    /// <summary>
    /// Processes a purchase order by applying all applicable business rules.
    /// </summary>
    public async Task ProcessOrderAsync(PurchaseOrder order)
    {
        _logger.LogInformation($"Processing order {order.PurchaseOrderId} for customer {order.CustomerId}");

        // Apply each applicable business rule synchronously
        foreach (var rule in _rules.Where(r => r.IsApplicable(order)))
        {
            _logger.LogInformation($"Applying business rule {rule.GetType().Name} to order {order.PurchaseOrderId}");

           var ruleResult = await rule.ApplyAsync(order);
        }

        _logger.LogInformation($"Order {order.PurchaseOrderId} processed successfully");
    }
}
