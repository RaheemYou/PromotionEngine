using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PromotionEngine.Enum;
using PromotionEngine.Interfaces;
using PromotionEngine.Models.EntityModels;
using PromotionEngine.Services;
using PromotionEngine.Tests.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngine.Tests.Services
{
    [TestClass]
    public class PromotionServiceTests : TestBase
    {
        private Mock<ILogger<IPromotionService>> mockLogger { get; set; }
        private IPromotionService promotionService { get; set; }

        public PromotionServiceTests()
        {
            this.mockLogger = new Mock<ILogger<IPromotionService>>();
            this.MockPromotionRepository = new Mock<Interfaces.IPromotionRepository>();           
            this.promotionService = new PromotionService(this.MockPromotionRepository.Object, this.mockLogger.Object);
        }

        [TestMethod]
        public void GetActiveByPromotionType_SingleItem()
        {
            // Mock the layer below the service.
            this.MockPromotionRepository.Setup(x => x.GetActiveByPromotionType(PromotionType.SingleItem)).Returns(this.MockPromotions().Where(x => x.PromotionType == PromotionType.SingleItem).AsEnumerable());

            List<IPromotionModel> promotions = this.promotionService.GetActivePromotions(PromotionType.SingleItem).ToList();

            // Verify the log message.
            this.mockLogger.Verify(x => x.Log(
            It.Is<LogLevel>(l => l == LogLevel.Information),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString() == "Getting active Promotions by type in the service."),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));

            Assert.AreEqual(promotions.Count, 2);
        }

        [TestMethod]
        public void GetActiveByPromotionType_MultipleItem()
        {
            this.MockPromotionRepository.Setup(x => x.GetActiveByPromotionType(PromotionType.MultipleItems)).Returns(this.MockPromotions().Where(x => x.PromotionType == PromotionType.MultipleItems).AsEnumerable());

            List<IPromotionModel> promotions = this.promotionService.GetActivePromotions(PromotionType.MultipleItems).ToList();

            // Verify the log message.
            this.mockLogger.Verify(x => x.Log(
            It.Is<LogLevel>(l => l == LogLevel.Information),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString() == "Getting active Promotions by type in the service."),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));

            Assert.AreEqual(promotions.Count, 1);
        }

        private List<IPromotion> MockPromotions()
        {
            return new List<IPromotion>()
            {
                new Promotion()
                {
                    Active = true,
                    PromotionItems = new List<IPromotionItem>()
                    {
                        new PromotionItem()
                        {
                            SKU = "A",
                            Quantity = 3,
                        }
                    },
                    PromotionPrice = 130,
                    PromotionType = PromotionType.SingleItem
                },
                new Promotion()
                {
                    Active = true,
                    PromotionItems = new List<IPromotionItem>()
                    {
                        new PromotionItem()
                        {
                            SKU = "B",
                            Quantity = 2,
                        }
                    },
                    PromotionPrice = 45,
                    PromotionType = PromotionType.SingleItem
                },
                new Promotion()
                {
                    Active = true,
                    PromotionItems = new List<IPromotionItem>()
                    {
                        new PromotionItem()
                        {
                            SKU = "C",
                            Quantity = 1,
                        },
                        new PromotionItem()
                        {
                            SKU = "D",
                            Quantity = 1,
                        }
                    },
                    PromotionPrice = 30,
                    PromotionType = PromotionType.MultipleItems
                }
            };
        }
    }
}
