using Application.Interfaces.Processors;
using Application.Interfaces.Services;
using Application.Models.Inputs;
using Domain.Entities;
using Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedContracts.Enums;
using SharedContracts.Result;

namespace FunBooksAndVideos.PurchaseOrderProcessor.Application.Services;

/// <summary>
/// Service for handling purchase orders.
/// </summary>
public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<IPurchaseOrderProcessor> _logger;

    public PurchaseOrderService(IPublishEndpoint publishEndpoint,ILogger<IPurchaseOrderProcessor> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<Result> ProcessAsync(PurchaseOrderDto dto)
    {
        try
        {
            //instead of using database, we create the purchase order entity here for simplicity
            var creationResult = TryCreatePurchaseOrder(dto);
            if (!creationResult.IsSuccess)
                return creationResult;

            await _publishEndpoint.Publish(new ProcessPurchaseOrderMessage(creationResult.Value));
            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to process order {dto.PurchaseOrderId}");
            return Result.Fail("Order processing failed");
        }
    }

    private Result<PurchaseOrder> TryCreatePurchaseOrder(PurchaseOrderDto dto)
    {
        try
        {
            var itemLines = dto.ItemLines.Select(CreatePurchaseOrderLine).ToList();
            var purchaseOrder = new PurchaseOrder
            {
                PurchaseOrderId = dto.PurchaseOrderId,
                CustomerId = dto.CustomerId,
                Total = dto.Total,
                ItemLines = itemLines
            };
            return Result<PurchaseOrder>.Ok(purchaseOrder);
        }
        catch (ArgumentException ex)
        {
            return Result<PurchaseOrder>.Fail(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating purchase order from dto: {ex.Message}");
            return Result<PurchaseOrder>.Fail("Failed to create purchase order.");
        }
    }

    private PurchaseOrderLine CreatePurchaseOrderLine(string productString)
    {
        long purchaseOrderId = 0; //We don't have access to the purchaseOrderId here, so we use a default.
        if (productString.StartsWith("Video \""))
            return new PurchaseOrderLine(purchaseOrderId, productString[7..^1], ProductType.Video);
        else if (productString.StartsWith("Book \""))
            return new PurchaseOrderLine(purchaseOrderId, productString[6..^1], ProductType.Book);
        else if (productString == "Book Club Membership")
            return new PurchaseOrderLine(purchaseOrderId, "Book Club", ProductType.Membership, MembershipType.BookClub);
        else if (productString == "Video Club Membership")
            return new PurchaseOrderLine(purchaseOrderId, "Video Club", ProductType.Membership, MembershipType.VideoClub);
        else if (productString == "Premium Membership")
            return new PurchaseOrderLine(purchaseOrderId, "Premium", ProductType.Membership, MembershipType.Premium);
        else
            throw new ArgumentException($"Invalid product string format: {productString}");
    }
}
