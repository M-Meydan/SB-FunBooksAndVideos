using FunBooksAndVideos.BusinessLogic.Classes;
using FunBooksAndVideos.BusinessLogic.Interfaces;
using FunBooksAndVideos.Model.Entities;
using FunBooksAndVideos.Model.Interfaces;
using FunBooksAndVideos.Repository.Interfaces;
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
    public class PurchaseOrderGeneratorTest
    {
        Mock<IPurchaseOrderGeneratorRepository> _mockPurchaseOrderGeneratorRepository;

        [TestInitialize]
        public void SetUp()
        {
            _mockPurchaseOrderGeneratorRepository = new Mock<IPurchaseOrderGeneratorRepository>();
            _mockPurchaseOrderGeneratorRepository.Setup(a => a.SavePurchaseOrder(It.IsAny<Order>())).Returns(It.IsAny<bool>());
        }

        [TestMethod]
        public void GivenPurchaseOrderWhenIPassOrderThenItShouldSaveTheOrder()
        {
            //Arrange
            PurchaseOrderGenerator purchaseOrderGenerator = new PurchaseOrderGenerator(_mockPurchaseOrderGeneratorRepository.Object);
            Order order = BuildVideoMemberShipTestData();

            //Act
            purchaseOrderGenerator.Generate(order);

            //Assert
            _mockPurchaseOrderGeneratorRepository.Verify(a => a.SavePurchaseOrder(It.IsAny<Order>()), Times.Exactly(1));
        }

        private Order BuildVideoMemberShipTestData()
        {
            return new Order
            {
                ItemLines = new List<IProduct>
                {
                new VideoMembership{ProductId = 1, ProductName = "PremiumMembership"},
                new Book{ProductId = 2, ProductName = "Girl on the train"}
                }
            };
        }
    }
}
