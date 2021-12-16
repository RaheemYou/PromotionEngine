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
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngine.Tests.Services
{
    [TestClass]
    public class CartServiceTests : TestBase
    {
        private Mock<ILogger<ICartService>> mockLogger { get; set; }
        private Mock<IAppSettings> mockAppsettings { get; set; }
        private IPromotionStrategyService promotionStrategyService { get; set; }
        private ICartService cartService { get; set; }

        public CartServiceTests()
        {
            this.mockLogger = new Mock<ILogger<ICartService>>();
            this.MockPromotionService = new Mock<IPromotionService>();
            this.mockAppsettings = new Mock<IAppSettings>();

            this.MockPromotionService.Setup(x => x.GetActivePromotions(PromotionType.SingleItem)).Returns(this.MockPromotions().Where(x => x.PromotionType == PromotionType.SingleItem).ToList());
            this.MockPromotionService.Setup(x => x.GetActivePromotions(PromotionType.MultipleItems)).Returns(this.MockPromotions().Where(x => x.PromotionType == PromotionType.MultipleItems).ToList());

            SingleItemPromotionStrategy singleItemPromotionStrategy = new SingleItemPromotionStrategy(new Mock<ILogger<SingleItemPromotionStrategy>>().Object);
            MultipleItemPromotionStrategy multipleItemPromotionStrategy = new MultipleItemPromotionStrategy(new Mock<ILogger<MultipleItemPromotionStrategy>>().Object);

            this.promotionStrategyService = new PromotionStrategyService(new Mock<ILogger<IPromotionStrategyService>>().Object, this.MockPromotionService.Object, this.mockAppsettings.Object, singleItemPromotionStrategy, multipleItemPromotionStrategy);

            this.cartService = new CartService(this.promotionStrategyService, this.mockLogger.Object);
        }

        [TestMethod]
        public void ApplyPromotion_ScenarioA()
        {
            this.mockAppsettings.Setup(x => x.AllowedPromotionType).Returns($"{PromotionType.SingleItem.ToString()},{PromotionType.MultipleItems.ToString()}");

            List<CartItemModel> cartItems = new List<CartItemModel>();
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

            CartModel cart = new CartModel()
            {
                CartItems = cartItems
            };

            CartModel updatedCart = this.cartService.CalculateTotalPromotionPrice(cart);

            Assert.AreEqual(100, updatedCart.TotalPrice);
        }

        [TestMethod]
        public void ApplyPromotion_ScenarioB()
        {
            this.mockAppsettings.Setup(x => x.AllowedPromotionType).Returns($"{PromotionType.SingleItem.ToString()},{PromotionType.MultipleItems.ToString()}");

            List<CartItemModel> cartItems = new List<CartItemModel>();
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

            CartModel cart = new CartModel()
            {
                CartItems = cartItems
            };

            CartModel updatedCart = this.cartService.CalculateTotalPromotionPrice(cart);

            Assert.AreEqual(370, updatedCart.TotalPrice);
        }

        [TestMethod]
        public void ApplyPromotion_ScenarioC()
        {
            this.mockAppsettings.Setup(x => x.AllowedPromotionType).Returns($"{PromotionType.SingleItem.ToString()},{PromotionType.MultipleItems.ToString()}");

            List<CartItemModel> cartItems = new List<CartItemModel>();
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

            CartModel cart = new CartModel()
            {
                CartItems = cartItems
            };

            CartModel updatedCart = this.cartService.CalculateTotalPromotionPrice(cart);

            Assert.AreEqual(280, updatedCart.TotalPrice);
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
