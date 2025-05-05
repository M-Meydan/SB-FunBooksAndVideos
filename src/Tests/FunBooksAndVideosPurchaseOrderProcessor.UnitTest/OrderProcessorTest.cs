using FunBooksAndVideos.BusinessLogic.Classes;
using FunBooksAndVideos.BusinessLogic.Interfaces;
using FunBooksAndVideos.Model.Entities;
using FunBooksAndVideos.Model.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunBooksAndVideos.UnitTest
{

    [TestClass]
    public class OrderProcessorTest
    {
        Mock<IPurchaseOrderGenerator> _mockPurchaseOrderGenerator;
        Mock<IShippingSlipGenerator> _mockShippingSlipGenerator;
        Mock<IMembershipActivator> _mockMemberShipActivator;

        [TestInitialize]
        public void SetUp()
        {
            _mockPurchaseOrderGenerator = new Mock<IPurchaseOrderGenerator>();
            _mockShippingSlipGenerator = new Mock<IShippingSlipGenerator>();
            _mockMemberShipActivator = new Mock<IMembershipActivator>();


            _mockPurchaseOrderGenerator.Setup(a => a.Generate(It.IsAny<Order>()))
                .Returns(It.IsAny<bool>());

            _mockShippingSlipGenerator.Setup(a => a.Generate(It.IsAny<Order>()))
               .Returns(It.IsAny<bool>());

            _mockMemberShipActivator.Setup(a => a.Activate(It.IsAny<Order>()))
               .Returns(It.IsAny<bool>());
        }

        [TestMethod]
        public void GivenOrderWhenProcessOrderThenInvokeAllActions()
        {
            //Arrange
            Order testData = BuildOrder();

            OrderProcessor orderProcessor = new OrderProcessor(_mockPurchaseOrderGenerator.Object, _mockShippingSlipGenerator.Object, _mockMemberShipActivator.Object);

            //Act
            orderProcessor.ProcessOrder(testData);

            //Assert
            _mockPurchaseOrderGenerator.Verify(a => a.Generate(It.IsAny<Order>()), Times.Exactly(1));
            _mockShippingSlipGenerator.Verify(a => a.Generate(It.IsAny<Order>()), Times.Exactly(1));
            _mockMemberShipActivator.Verify(a => a.Activate(It.IsAny<Order>()), Times.Exactly(1));
        }


        private Order BuildOrder()
        {
            return new Order
            {
                ItemLines = new List<IProduct>
                {
                new VideoMembership{ProductId = 1, ProductName = "VideoMembership"},
                new Book{ProductId = 2, ProductName = "Girl on the train"}
                }
            };
        }
    }

    //This is how manual mocking looks like. If I want to mock IPurchaseOrderGenerator-
    //I can create an instance of MockPurchaseOrderGenerator then execution will call-
    //Generate method of MockPurchaseOrderGenerator class only
    public class MockPurchaseOrderGenerator : IPurchaseOrderGenerator
    {
        public bool Generate(Order itemlines)
        {
            return true;
        }
    }
}
