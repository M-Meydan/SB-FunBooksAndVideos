using Application.Interfaces.Rules;
using Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Consumers;


/// <summary>
/// Message Consumer for processing purchase orders.
/// </summary>
public class ProcessPurchaseOrderConsumer : IConsumer<ProcessPurchaseOrderMessage>
{
    private readonly IRuleProcessor _ruleProcessor;
    private readonly ILogger<ProcessPurchaseOrderConsumer> _logger;

    public ProcessPurchaseOrderConsumer(
        IRuleProcessor ruleProcessor,
        ILogger<ProcessPurchaseOrderConsumer> logger)
    {
        _ruleProcessor = ruleProcessor;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProcessPurchaseOrderMessage> context)
    {
        var orderId = context.Message.order.PurchaseOrderId;

        try
        {
            _logger.LogInformation("Processing purchase order {OrderId}", orderId);

            //Process business rules
            var result = await _ruleProcessor.ExecuteRulesAsync(context.Message.order);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Order {OrderId} failed rules: {Error}",
                    orderId, result.Error);
            }
            else
            {
                //Publish order processed event so we can mark it complete

                _logger.LogInformation("Completed processing order {OrderId}", orderId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing order {OrderId}", orderId);
            throw; // Let MassTransit handle retries
        }
    }
}
