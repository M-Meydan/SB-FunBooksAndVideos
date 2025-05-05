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
    public class PremiumMembershipActivatorTest
    {
        Mock<IMembershipActivateRepository> _membershipActivateRepository;

        [TestInitialize]
        public void SetUp()
        {
            _membershipActivateRepository = new Mock<IMembershipActivateRepository>();
            _membershipActivateRepository.Setup(a => a.Activate(It.IsAny<Order>())).Returns(It.IsAny<bool>());
        }

        [TestMethod]
        public void GivenPremiumMembershipActivatorWhenPassPremiumMembershipThenActivateIt()
        {
            //Arrange
            
            Order order = BuildPremiumMemberShipTestData();

            //TODO:Update injecion with MOQ

            IMembershipActivator memberShipActivator = new PremiumMembershipActivator(_membershipActivateRepository.Object);

            //Act
            memberShipActivator.Activate(order);

            //Assert
            _membershipActivateRepository.Verify(a => a.Activate(It.IsAny<Order>()), Times.Exactly(1));
        }

        [TestMethod]
        public void GivenPremiumMembershipActivatorWhenPassBookMembershipThenDontActivateIt()
        {
            //Arrange

            Order order = BuildBookMemberShipTestData();

            //TODO:Update injecion with MOQ

            IMembershipActivator memberShipActivator = new PremiumMembershipActivator(_membershipActivateRepository.Object);

            //Act
           bool actual = memberShipActivator.Activate(order);

            //Assert
            _membershipActivateRepository.Verify(a => a.Activate(It.IsAny<Order>()), Times.Exactly(0));
            Assert.IsFalse(actual);
        }
        [TestMethod]
        public void GivenPremiumMembershipActivatorWhenPassVideoMembershipThenDontActivateIt()
        {
            //Arrange

            Order order = BuildVideoMemberShipTestData();

            //TODO:Update injecion with MOQ

            IMembershipActivator memberShipActivator = new PremiumMembershipActivator(_membershipActivateRepository.Object);

            //Act
            bool actual = memberShipActivator.Activate(order);

            //Assert
            _membershipActivateRepository.Verify(a => a.Activate(It.IsAny<Order>()), Times.Exactly(0));
            Assert.IsFalse(actual);
        }

        private Order BuildPremiumMemberShipTestData()
        {
            return new Order
            {
                ItemLines = new List<IProduct>
                {
                new PremiumMembership{ProductId = 1, ProductName = "PremiumMembership"},
                new Book{ProductId = 2, ProductName = "Girl on the train"}
                }
            };
        }

        private Order BuildBookMemberShipTestData()
        {
            return new Order
            {
                ItemLines = new List<IProduct>
                {
                new BookMembership{ProductId = 1, ProductName = "PremiumMembership"},
                new Book{ProductId = 2, ProductName = "Girl on the train"}
                }
            };
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
