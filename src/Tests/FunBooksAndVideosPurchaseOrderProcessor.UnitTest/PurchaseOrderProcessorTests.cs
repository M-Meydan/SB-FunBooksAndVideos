using Application.Interfaces.Processors;
using Application.Interfaces.Rules;
using Domain.Entities;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using SharedContracts.Enums;
using SharedContracts.Result;


namespace FunBooksAndVideosPurchaseOrderProcessor.UnitTest
{
    [TestFixture]
    public class PurchaseOrderProcessorTests
    {
        private Mock<IRule> _membershipRuleMock;
        private Mock<IRule> _shippingRuleMock;
        private Mock<ILogger<IPurchaseOrderProcessor>> _loggerMock;
        private PurchaseOrderProcessor _processor;

        [SetUp]
        public void Setup()
        {
            _membershipRuleMock = new Mock<IRule>();
            _shippingRuleMock = new Mock<IRule>();
            _loggerMock = new Mock<ILogger<IPurchaseOrderProcessor>>();

            var rules = new List<IRule> { _membershipRuleMock.Object, _shippingRuleMock.Object };
            _processor = new PurchaseOrderProcessor(rules, _loggerMock.Object);
        }

        [TestCase(ProductType.Book, true, false, TestName = "Book Product - Applies Shipping Rule Only")]
        [TestCase(ProductType.Video, false, false, TestName = "Video Product - No Rules Applied")]
        public async Task ProcessOrderAsync_WithSingleProduct_ShouldApplyCorrectRule(ProductType productType, bool expectShipping, bool expectMembership)
        {
            // Arrange
            var order = CreateOrderWithLine(productType);

            SetupRuleMock(_shippingRuleMock, expectShipping, order);
            SetupRuleMock(_membershipRuleMock, expectMembership, order);

            // Act
            await _processor.ProcessOrderAsync(order);

            // Assert
            VerifyRule(_shippingRuleMock, expectShipping, order);
            VerifyRule(_membershipRuleMock, expectMembership, order);
        }

        [TestCase(MembershipType.BookClub)]
        [TestCase(MembershipType.VideoClub)]
        [TestCase(MembershipType.Premium)]
        public async Task ProcessOrderAsync_WithMembershipType_ShouldApplyMembershipRule(MembershipType type)
        {
            var order = CreateOrderWithLine(ProductType.Membership, type);

            SetupRuleMock(_membershipRuleMock, true, order);

            await _processor.ProcessOrderAsync(order);

            VerifyRule(_membershipRuleMock, true, order);
            VerifyRule(_shippingRuleMock, false, order);
        }

        [Test]
        public async Task ProcessOrderAsync_WithBookAndMembership_ShouldApplyBothRules()
        {
            var order = CreateOrderWithLines(
                (ProductType.Book, null),
                (ProductType.Membership, MembershipType.BookClub)
            );

            SetupRuleMock(_shippingRuleMock, true, order);
            SetupRuleMock(_membershipRuleMock, true, order);

            await _processor.ProcessOrderAsync(order);

            VerifyRule(_shippingRuleMock, true, order);
            VerifyRule(_membershipRuleMock, true, order);
        }

        [Test]
        public async Task ProcessOrderAsync_WithAllProductTypes_ShouldApplyCorrectRules()
        {
            var order = CreateOrderWithLines(
                (ProductType.Book, null),
                (ProductType.Video, null),
                (ProductType.Membership, MembershipType.BookClub)
            );

            SetupRuleMock(_shippingRuleMock, true, order);
            SetupRuleMock(_membershipRuleMock, true, order);

            await _processor.ProcessOrderAsync(order);

            VerifyRule(_shippingRuleMock, true, order);
            VerifyRule(_membershipRuleMock, true, order);
        }

        // ---------- Helpers ----------
        private PurchaseOrder CreateOrderWithLine(ProductType type, MembershipType? membershipType = null) =>
            new()
            {
                PurchaseOrderId = 1,
                CustomerId = 1,
                ItemLines = new List<PurchaseOrderLine>
                {
                new(1, $"{type} Product", type, membershipType)
                }
            };

        private PurchaseOrder CreateOrderWithLines(params (ProductType type, MembershipType? membershipType)[] items) =>
            new()
            {
                PurchaseOrderId = 1,
                CustomerId = 1,
                ItemLines = items.Select((item, i) => new PurchaseOrderLine(i + 1, $"{item.type} Product", item.type, item.membershipType)).ToList()
            };

        private void SetupRuleMock(Mock<IRule> ruleMock, bool isApplicable, PurchaseOrder order)
        {
            ruleMock.Setup(x => x.IsApplicable(order)).Returns(isApplicable);
            if (isApplicable)
            {
                ruleMock.Setup(x => x.ApplyAsync(order)).ReturnsAsync(Result.Ok());
            }
        }

        private void VerifyRule(Mock<IRule> ruleMock, bool shouldApply, PurchaseOrder order)
        {
            ruleMock.Verify(x => x.ApplyAsync(order), shouldApply ? Times.Once() : Times.Never());
        }
    }

}
