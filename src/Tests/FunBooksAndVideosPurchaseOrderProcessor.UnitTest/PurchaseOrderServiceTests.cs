using Application.Interfaces.Processors;
using Application.Models.Inputs;
using FunBooksAndVideos.PurchaseOrderProcessor.Application.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;

namespace FunBooksAndVideosPurchaseOrderProcessor.UnitTest;

[TestFixture]
public class PurchaseOrderServiceTests
{
    private Mock<IPublishEndpoint> _publishEndpointMock;
    private Mock<ILogger<IPurchaseOrderProcessor>> _loggerMock;
    private PurchaseOrderService _purchaseOrderService;

    [SetUp]
    public void Setup()
    {
        _publishEndpointMock = new Mock<IPublishEndpoint>();
        _loggerMock = new Mock<ILogger<IPurchaseOrderProcessor>>();
        _purchaseOrderService = new PurchaseOrderService(_publishEndpointMock.Object, _loggerMock.Object);
    }

    [TestCase("Book \"Clean Code\"")]
    [TestCase("Video \"Design Patterns\"")]
    [TestCase("Book Club Membership")]
    [TestCase("Video Club Membership")]
    [TestCase("Premium Membership")]
    public async Task SingleValidItem_ShouldReturnSuccess(string item)
    {
        // Arrange
        var dto = CreateDtoWithItems(item);

        // Act
        var result = await _purchaseOrderService.ProcessAsync(dto);

        // Assert
        Assert.IsTrue(result.IsSuccess, $"Processing failed for item: {item}");
    }

    [Test]
    public async Task MixedProducts_ShouldReturnSuccess()
    {
        // Arrange
        var items = new List<string>
        {
            "Book \"Clean Code\"",
            "Video \"Design Patterns\"",
            "Book Club Membership",
            "Video Club Membership",
            "Premium Membership"
        };

        var dto = CreateDtoWithItems(items.ToArray());

        // Act
        var result = await _purchaseOrderService.ProcessAsync(dto);

        // Assert
        Assert.IsTrue(result.IsSuccess, "Processing failed for mixed product types.");
    }

    private PurchaseOrderDto CreateDtoWithItems(params string[] items)
    {
        return new PurchaseOrderDto
        {
            PurchaseOrderId = 123,
            CustomerId = 456,
            Total = 50.00m * items.Length,
            ItemLines = items.ToList()
        };
    }
}

