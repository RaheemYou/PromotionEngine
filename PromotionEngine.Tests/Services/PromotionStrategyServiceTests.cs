using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PromotionEngine.Enum;
using PromotionEngine.Interfaces;
using PromotionEngine.Models.BusinessModels;
using PromotionEngine.Models.EntityModels;
using PromotionEngine.Services;
using PromotionEngine.Services.PromotionStrategies;
using PromotionEngine.Tests.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngine.Tests.Services
{
    [TestClass]
    public class PromotionStrategyServiceTests : TestBase
    {
        private Mock<ILogger<IPromotionStrategyService>> mockLogger { get; set; }
        private Mock<IAppSettings> mockAppsettings {get; set;}
        private IPromotionStrategyService promotionStrategyService { get; set; }

        public PromotionStrategyServiceTests()
        {
            this.mockLogger = new Mock<ILogger<IPromotionStrategyService>>();
            this.MockPromotionService = new Mock<IPromotionService>();
            this.mockAppsettings = new Mock<IAppSettings>();

            this.MockPromotionService.Setup(x => x.GetActivePromotions(PromotionType.SingleItem)).Returns(this.MockPromotions().Where(x=> x.PromotionType == PromotionType.SingleItem).ToList());
            this.MockPromotionService.Setup(x => x.GetActivePromotions(PromotionType.MultipleItems)).Returns(this.MockPromotions().Where(x => x.PromotionType == PromotionType.MultipleItems).ToList());

            SingleItemPromotionStrategy singleItemPromotionStrategy = new SingleItemPromotionStrategy(new Mock<ILogger<SingleItemPromotionStrategy>>().Object);
            MultipleItemPromotionStrategy multipleItemPromotionStrategy = new MultipleItemPromotionStrategy(new Mock<ILogger<MultipleItemPromotionStrategy>>().Object);

            this.promotionStrategyService = new PromotionStrategyService(this.mockLogger.Object, this.MockPromotionService.Object, this.mockAppsettings.Object, singleItemPromotionStrategy, multipleItemPromotionStrategy);
        }

        [TestMethod]
        public void ApplyPromotion_SingleItem()
        {
            this.mockAppsettings.Setup(x => x.AllowedPromotionType).Returns(PromotionType.SingleItem.ToString());

            this.promotionStrategyService.ApplyPromotionStrategies(new System.Collections.Generic.List<ICartItemModel>());

            // Verify the log message.
            this.mockLogger.Verify(x => x.Log(
            It.Is<LogLevel>(l => l == LogLevel.Information),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString() == "Applying SingleItem promotion Strategy."),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }

        [TestMethod]
        public void ApplyPromotion_MultipleItems()
        {
            this.mockAppsettings.Setup(x => x.AllowedPromotionType).Returns(PromotionType.MultipleItems.ToString());

            this.promotionStrategyService.ApplyPromotionStrategies(new System.Collections.Generic.List<ICartItemModel>());

            // Verify the log message.
            this.mockLogger.Verify(x => x.Log(
            It.Is<LogLevel>(l => l == LogLevel.Information),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString() == "Applying MultipleItems promotion Strategy."),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }

        [TestMethod]
        public void ApplyPromotion_NoStrategy()
        {
            this.mockAppsettings.Setup(x => x.AllowedPromotionType).Returns("");

            this.promotionStrategyService.ApplyPromotionStrategies(new System.Collections.Generic.List<ICartItemModel>());

            // Verify the log message.
            this.mockLogger.Verify(x => x.Log(
            It.Is<LogLevel>(l => l == LogLevel.Information),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString() == "AppSettings contains no active promotions."),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }

        [TestMethod]
        public void ApplyPromotion_ScenarioA()
        {
            this.mockAppsettings.Setup(x => x.AllowedPromotionType).Returns($"{PromotionType.SingleItem.ToString()},{PromotionType.MultipleItems.ToString()}");

            List<ICartItemModel> cartItems = new List<ICartItemModel>();
            cartItems.Add(new CartItemModel()
            {
                SKU = "A",
                UnitPrice = 50,
                Quantity = 1,
                TotalPrice = 50,
            });
            cartItems.Add(new CartItemModel()
            {
                SKU = "B",
                UnitPrice = 30,
                Quantity = 1,
                TotalPrice = 30,
            });
            cartItems.Add(new CartItemModel()
            {
                SKU = "C",
                UnitPrice = 20,
                Quantity = 1,
                TotalPrice = 20,
            });

            List<ICartItemModel> appliedPromotions = this.promotionStrategyService.ApplyPromotionStrategies(cartItems);

            Assert.AreEqual(50, appliedPromotions.First(x => x.SKU == "A").TotalPrice);
            Assert.AreEqual(30, appliedPromotions.First(x => x.SKU == "B").TotalPrice);
            Assert.AreEqual(20, appliedPromotions.First(x => x.SKU == "C").TotalPrice);
        }

        [TestMethod]
        public void ApplyPromotion_ScenarioB()
        {
            this.mockAppsettings.Setup(x => x.AllowedPromotionType).Returns($"{PromotionType.SingleItem.ToString()},{PromotionType.MultipleItems.ToString()}");

            List<ICartItemModel> cartItems = new List<ICartItemModel>();
            cartItems.Add(new CartItemModel()
            {
                SKU = "A",
                UnitPrice = 50,
                Quantity = 5,
                TotalPrice = 250,
            });
            cartItems.Add(new CartItemModel()
            {
                SKU = "B",
                UnitPrice = 30,
                Quantity = 5,
                TotalPrice = 150,
            });
            cartItems.Add(new CartItemModel()
            {
                SKU = "C",
                UnitPrice = 20,
                Quantity = 1,
                TotalPrice = 20,
            });

            List<ICartItemModel> appliedPromotions = this.promotionStrategyService.ApplyPromotionStrategies(cartItems);

            Assert.AreEqual(230, appliedPromotions.First(x => x.SKU == "A").TotalPrice);
            Assert.AreEqual(120, appliedPromotions.First(x => x.SKU == "B").TotalPrice);
            Assert.AreEqual(20, appliedPromotions.First(x => x.SKU == "C").TotalPrice);
        }

        [TestMethod]
        public void ApplyPromotion_ScenarioC()
        {
            this.mockAppsettings.Setup(x => x.AllowedPromotionType).Returns($"{PromotionType.SingleItem.ToString()},{PromotionType.MultipleItems.ToString()}");

            List<ICartItemModel> cartItems = new List<ICartItemModel>();
            cartItems.Add(new CartItemModel()
            {
                SKU = "A",
                UnitPrice = 50,
                Quantity = 3,
                TotalPrice = 250,
            });
            cartItems.Add(new CartItemModel()
            {
                SKU = "B",
                UnitPrice = 30,
                Quantity = 5,
                TotalPrice = 150,
            });
            cartItems.Add(new CartItemModel()
            {
                SKU = "C",
                UnitPrice = 20,
                Quantity = 1,
                TotalPrice = 20,
            });
            cartItems.Add(new CartItemModel()
            {
                SKU = "D",
                UnitPrice = 15,
                Quantity = 1,
                TotalPrice = 15,
            });

            List<ICartItemModel> appliedPromotions = this.promotionStrategyService.ApplyPromotionStrategies(cartItems);

            Assert.AreEqual(130, appliedPromotions.First(x => x.SKU == "A").TotalPrice);
            Assert.AreEqual(120, appliedPromotions.First(x => x.SKU == "B").TotalPrice);
            Assert.AreEqual(30, appliedPromotions.First(x => x.SKU == "C").TotalPrice);
            Assert.AreEqual(0, appliedPromotions.First(x => x.SKU == "D").TotalPrice);
        }

        private List<IPromotionModel> MockPromotions()
        {
            return new List<IPromotionModel>()
            {
                new PromotionModel(new Promotion()
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
                }),
                new PromotionModel(new Promotion()
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
                }),
                new PromotionModel(new Promotion()
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
                })
            };
        }
    }
}
