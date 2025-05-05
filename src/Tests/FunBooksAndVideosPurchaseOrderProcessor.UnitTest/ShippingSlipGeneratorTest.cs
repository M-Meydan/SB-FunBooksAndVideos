using FunBooksAndVideos.BusinessLogic.Classes;
using FunBooksAndVideos.Model.Entities;
using FunBooksAndVideos.Model.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunBooksAndVideos.UnitTest
{
    [TestClass]
    public class ShippingSlipGeneratorTest
    {
        [TestMethod]
        public void GivenOrderWhenIHavePhysicalProductThenGenerateShippingSlip()
        {
            //Arrange
            ShippingSlipGenerator shippingSlipGenerator = new ShippingSlipGenerator();

            //Act
            bool actual = shippingSlipGenerator.Generate(BuildOrderWithPhysicalProduct());

            //Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void GivenOrderWhenIHaveNonPhysicalProductThenDontGenerateShippingSlip()
        {
            //Arrange
            ShippingSlipGenerator shippingSlipGenerator = new ShippingSlipGenerator();

            //Act
            bool actual = shippingSlipGenerator.Generate(BuildOrderWithNonPhysicalProduct());

            //Assert
            Assert.IsFalse(actual);
        }

        private Order BuildOrderWithPhysicalProduct()
        {
            return new Order
            {
                ItemLines = new List<IProduct>
                {
                new Book{ProductId = 2, ProductName = "Girl on the train"}
                }
            };
        }

        private Order BuildOrderWithNonPhysicalProduct()
        {
            return new Order
            {
                ItemLines = new List<IProduct>
                {
                new VideoMembership{ProductId = 2, ProductName = "Girl on the train"}
                }
            };
        }
    }
}
