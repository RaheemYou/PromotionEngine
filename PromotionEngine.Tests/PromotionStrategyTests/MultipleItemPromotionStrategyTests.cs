using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PromotionEngine.Interfaces;
using PromotionEngine.Models.BusinessModels;
using PromotionEngine.Models.EntityModels;
using PromotionEngine.PromotionStrategies;
using PromotionEngine.Tests.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngine.Tests.PromotionStrategyTests
{
    [TestClass]
    public class MultipleItemPromotionStrategyTests : TestBase
    {
        private Mock<ILogger<MultipleItemPromotionStrategy>> mockLogger { get; set; }
        private MultipleItemPromotionStrategy multipleItemPromotionStrategy { get; set; }

        public MultipleItemPromotionStrategyTests()
        {
            this.mockLogger = new Mock<ILogger<MultipleItemPromotionStrategy>>();
            this.multipleItemPromotionStrategy = new MultipleItemPromotionStrategy(this.mockLogger.Object);
        }

        #region Can Apply Promotion Tests

        [TestMethod]
        public void CanApplyPromotion_ThrowsArgumentNullException_WhenCartItemsNull()
        {
            try
            {
                this.multipleItemPromotionStrategy.CanApplyPromotion(null, new Mock<IPromotionModel>().Object);
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("cartItems", ex.ParamName);
            }
        }

        [TestMethod]
        public void CanApplyPromotion_ThrowsArgumentNullException_WhenPromotionNull()
        {
            try
            {
                this.multipleItemPromotionStrategy.CanApplyPromotion(new List<ICartItemModel>(), null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("promotion", ex.ParamName);
            }
        }

        [TestMethod]
        public void CanApplyPromotion_ReturnsFalse_WhenPromotionHasNoItems()
        {
            IPromotion promotion = new Promotion();
            promotion.Active = true;
            promotion.PromotionItems = new List<IPromotionItem>();

            List<ICartItemModel> cartItems = new List<ICartItemModel>()
            {
                new CartItemModel()
                {
                    SKU = "A",
                    Price = 100,
                    PromotionApplied = false,
                    Quantity = 3,
                    TotalPrice = 300,
                }
            };

            Assert.IsFalse(this.multipleItemPromotionStrategy.CanApplyPromotion(cartItems, new PromotionModel(promotion)));
        }

        [TestMethod]
        public void CanApplyPromotion_ReturnsFalse_WhenPromotionIsInactive()
        {
            IPromotion promotion = new Promotion();
            promotion.Active = false;
            promotion.PromotionItems = new List<IPromotionItem>();

            List<ICartItemModel> cartItems = new List<ICartItemModel>()
            {
                new CartItemModel()
                {
                    SKU = "A",
                    Price = 100,
                    PromotionApplied = false,
                    Quantity = 3,
                    TotalPrice = 300,
                }
            };

            Assert.IsFalse(this.multipleItemPromotionStrategy.CanApplyPromotion(cartItems, new PromotionModel(promotion)));
        }

        [TestMethod]
        public void CanApplyPromotion_ReturnsFalse_EmptyCartItems()
        {
            IPromotion promotion = new Promotion();
            promotion.Active = true;
            promotion.PromotionItems = new List<IPromotionItem>();

            Assert.IsFalse(this.multipleItemPromotionStrategy.CanApplyPromotion(new List<ICartItemModel>(), new PromotionModel(promotion)));
        }

        [TestMethod]
        public void CanApplyPromotion_False_SingleItemPromotion()
        {
            IPromotion promotion = new Promotion()
            {
                Active = true,
                PromotionItems = new List<IPromotionItem>()
                {
                    new PromotionItem()
                    {
                        SKU = "A",
                        Quantity = 1,
                    },
                },
                PromotionType = Enum.PromotionType.SingleItem,
            };

            List<ICartItemModel> cartItems = new List<ICartItemModel>()
            {
                new CartItemModel()
                {
                    SKU = "A",
                    Price = 100,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 100,
                },
                new CartItemModel()
                {
                    SKU = "B",
                    Price = 100,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 100,
                }
            };

            Assert.IsFalse(this.multipleItemPromotionStrategy.CanApplyPromotion(cartItems, new PromotionModel(promotion)));
        }

        [TestMethod]
        public void CanApplyPromotion_True()
        {
            IPromotion promotion = new Promotion()
            {
                Active = true,
                PromotionItems = new List<IPromotionItem>()
                {
                    new PromotionItem()
                    {
                        SKU = "A",
                        Quantity = 1,
                    },
                    new PromotionItem()
                    {
                        SKU = "B",
                        Quantity = 1,
                    },
                },
                PromotionType = Enum.PromotionType.MultipleItems,
            };

            List<ICartItemModel> cartItems = new List<ICartItemModel>()
            {
                new CartItemModel()
                {
                    SKU = "A",
                    Price = 100,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 100,
                },
                new CartItemModel()
                {
                    SKU = "B",
                    Price = 100,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 100,
                }
            };

            Assert.IsTrue(this.multipleItemPromotionStrategy.CanApplyPromotion(cartItems, new PromotionModel(promotion)));
        }

        [TestMethod]
        public void CanApplyPromotion_False_InvalidQuantity()
        {
            IPromotion promotion = new Promotion();
            promotion.Active = true;
            promotion.PromotionItems = new List<IPromotionItem>()
            {
                new PromotionItem()
                {
                    SKU = "A",
                    Quantity = 2,
                },
                new PromotionItem()
                {
                    SKU = "B",
                    Quantity = 1,
                },
            };

            promotion.Active = true;


            List<ICartItemModel> cartItems = new List<ICartItemModel>()
            {
                new CartItemModel()
                {
                    SKU = "A",
                    Price = 100,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 100,
                },
                new CartItemModel()
                {
                    SKU = "B",
                    Price = 100,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 100,
                }
            };

            Assert.IsFalse(this.multipleItemPromotionStrategy.CanApplyPromotion(cartItems, new PromotionModel(promotion)));
        }

        [TestMethod]
        public void CanApplyPromotion_False_SingleMatchingSKU()
        {
            IPromotion promotion = new Promotion();
            promotion.Active = true;
            promotion.PromotionItems = new List<IPromotionItem>()
            {
                new PromotionItem()
                {
                    SKU = "A",
                    Quantity = 1,
                },
                new PromotionItem()
                {
                    SKU = "B",
                    Quantity = 1,
                },
            };

            promotion.Active = true;


            List<ICartItemModel> cartItems = new List<ICartItemModel>()
            {
                new CartItemModel()
                {
                    SKU = "A",
                    Price = 100,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 100,
                },
                new CartItemModel()
                {
                    SKU = "C",
                    Price = 100,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 100,
                }
            };

            Assert.IsFalse(this.multipleItemPromotionStrategy.CanApplyPromotion(cartItems, new PromotionModel(promotion)));
        }

        [TestMethod]
        public void CanApplyPromotion_False_NoMatchingSKU()
        {
            IPromotion promotion = new Promotion();
            promotion.Active = true;
            promotion.PromotionItems = new List<IPromotionItem>()
            {
                new PromotionItem()
                {
                    SKU = "A",
                    Quantity = 1,
                },
                new PromotionItem()
                {
                    SKU = "B",
                    Quantity = 1,
                },
            };

            promotion.Active = true;


            List<ICartItemModel> cartItems = new List<ICartItemModel>()
            {
                new CartItemModel()
                {
                    SKU = "C",
                    Price = 100,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 100,
                },
                new CartItemModel()
                {
                    SKU = "D",
                    Price = 100,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 100,
                }
            };

            Assert.IsFalse(this.multipleItemPromotionStrategy.CanApplyPromotion(cartItems, new PromotionModel(promotion)));
        }

        #endregion Can Apply Promotion Tests

        #region Apply Promotion Tests

        [TestMethod]
        public void ApplyPromotion_ThrowsArgumentNullException_NullPromotions()
        {
            try
            {
                List<ICartItemModel> cartItems = new List<ICartItemModel>()
                {
                    new CartItemModel()
                    {
                        SKU = "A",
                        Price = 100,
                        PromotionApplied = false,
                        Quantity = 1,
                        TotalPrice = 100,
                    },
                    new CartItemModel()
                    {
                        SKU = "B",
                        Price = 100,
                        PromotionApplied = false,
                        Quantity = 1,
                        TotalPrice = 100,
                    }
                };

                List<IPromotionModel> promotions = null;

                List<ICartItemModel> processedItems = this.multipleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("promotions", ex.ParamName);
            }
        }

        [TestMethod]
        public void ApplyPromotion_ThrowsArgumentNullException_NullCartItems()
        {
            try
            {
                IPromotion promotion = new Promotion()
                {
                    Active = true,
                    PromotionItems = new List<IPromotionItem>()
                    {
                        new PromotionItem()
                        {
                            SKU = "A",
                            Quantity = 1,
                        },
                        new PromotionItem()
                        {
                            SKU = "B",
                            Quantity = 1,

                        },
                    },
                    PromotionPrice = 150,
                    PromotionType = Enum.PromotionType.MultipleItems
                };

                List<ICartItemModel> cartItems = null;

                List<IPromotionModel> promotions = new List<IPromotionModel>()
                {
                    new PromotionModel(promotion)
                };

                List<ICartItemModel> processedItems = this.multipleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("cartItems", ex.ParamName);
            }
        }

        [TestMethod]
        public void ApplyPromotion_Success_EmptyPromotions()
        {
            List<ICartItemModel> cartItems = new List<ICartItemModel>()
            {
                new CartItemModel()
                {
                    SKU = "A",
                    Price = 100,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 100,
                },
                new CartItemModel()
                {
                    SKU = "B",
                    Price = 100,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 100,
                }
            };

            List<IPromotionModel> promotions = new List<IPromotionModel>();

            List<ICartItemModel> processedItems = this.multipleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);

            Assert.AreEqual(cartItems, processedItems);
        }

        [TestMethod]
        public void ApplyPromotion_Sucess_EmptyCartItems()
        {
            IPromotion promotion = new Promotion()
            {
                Active = true,
                PromotionItems = new List<IPromotionItem>()
                {
                    new PromotionItem()
                    {
                        SKU = "A",
                        Quantity = 1,
                    },
                    new PromotionItem()
                    {
                        SKU = "B",
                        Quantity = 1,

                    },
                },
                PromotionPrice = 150,
                PromotionType = Enum.PromotionType.MultipleItems
            };

            List<ICartItemModel> cartItems = new List<ICartItemModel>();

            List<IPromotionModel> promotions = new List<IPromotionModel>()
            {
                new PromotionModel(promotion)
            };

            List<ICartItemModel> processedItems = this.multipleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);

            Assert.IsFalse(processedItems.Any());
        }

        [TestMethod]
        public void ApplyPromotion_Success_SinglePromotion()
        {
            IPromotion promotion = new Promotion()
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

                    },
                },
                PromotionPrice = 30,
                PromotionType = Enum.PromotionType.MultipleItems
            };

            List<ICartItemModel> cartItems = new List<ICartItemModel>()
            {
                new CartItemModel()
                {
                    SKU = "C",
                    Price = 20,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 20,
                },
                new CartItemModel()
                {
                    SKU = "D",
                    Price = 15,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 15,
                }
            };

            List<IPromotionModel> promotions = new List<IPromotionModel>()
            {
                new PromotionModel(promotion)
            };

            List<ICartItemModel> processedItems = this.multipleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);

            Assert.AreEqual(30, processedItems.First(x => x.SKU == "C").TotalPrice);
            Assert.AreEqual(0, processedItems.First(x => x.SKU == "D").TotalPrice);
        }

        [TestMethod]
        public void ApplyPromotion_Success_SinglePromotionWithMultipleCartItemQuantity()
        {
            IPromotion promotion = new Promotion()
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

                    },
                },
                PromotionPrice = 30,
                PromotionType = Enum.PromotionType.MultipleItems
            };

            List<ICartItemModel> cartItems = new List<ICartItemModel>()
            {
                new CartItemModel()
                {
                    SKU = "C",
                    Price = 20,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 20,
                },
                new CartItemModel()
                {
                    SKU = "D",
                    Price = 15,
                    PromotionApplied = false,
                    Quantity = 2,
                    TotalPrice = 15,
                }
            };

            List<IPromotionModel> promotions = new List<IPromotionModel>()
            {
                new PromotionModel(promotion)
            };

            List<ICartItemModel> processedItems = this.multipleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);

            Assert.AreEqual(30, processedItems.First(x => x.SKU == "C").TotalPrice);
            Assert.AreEqual(15, processedItems.First(x => x.SKU == "D").TotalPrice);
        }

        [TestMethod]
        public void ApplyPromotion_Success_SinglePromotionWithMultiplePromotionQuantity()
        {
            IPromotion promotion = new Promotion()
            {
                Active = true,
                PromotionItems = new List<IPromotionItem>()
                {
                    new PromotionItem()
                    {
                        SKU = "C",
                        Quantity = 2,
                    },
                    new PromotionItem()
                    {
                        SKU = "D",
                        Quantity = 1,

                    },
                },
                PromotionPrice = 30,
                PromotionType = Enum.PromotionType.MultipleItems
            };

            List<ICartItemModel> cartItems = new List<ICartItemModel>()
            {
                new CartItemModel()
                {
                    SKU = "C",
                    Price = 20,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 20,
                },
                new CartItemModel()
                {
                    SKU = "D",
                    Price = 15,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 15,
                }
            };

            List<IPromotionModel> promotions = new List<IPromotionModel>()
            {
                new PromotionModel(promotion)
            };

            List<ICartItemModel> processedItems = this.multipleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);

            Assert.AreEqual(20, processedItems.First(x => x.SKU == "C").TotalPrice);
            Assert.AreEqual(15, processedItems.First(x => x.SKU == "D").TotalPrice);
        }


        [TestMethod]
        public void ApplyPromotion_Success_PromotionAlreadyApplied()
        {
            IPromotion promotion = new Promotion()
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

                    },
                },
                PromotionPrice = 30,
                PromotionType = Enum.PromotionType.MultipleItems
            };

            List<ICartItemModel> cartItems = new List<ICartItemModel>()
            {
                new CartItemModel()
                {
                    SKU = "C",
                    Price = 20,
                    PromotionApplied = true,
                    Quantity = 1,
                    TotalPrice = 20,
                },
                new CartItemModel()
                {
                    SKU = "D",
                    Price = 15,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 15,
                }
            };

            List<IPromotionModel> promotions = new List<IPromotionModel>()
            {
                new PromotionModel(promotion)
            };

            List<ICartItemModel> processedItems = this.multipleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);

            Assert.AreEqual(20, processedItems.First(x => x.SKU == "C").TotalPrice);
            Assert.AreEqual(15, processedItems.First(x => x.SKU == "D").TotalPrice);
        }

        [TestMethod]
        public void ApplyPromotion_Success_ItemWithoutPromotion()
        {
            IPromotion promotion = new Promotion()
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

                    },
                },
                PromotionPrice = 150,
                PromotionType = Enum.PromotionType.MultipleItems
            };

            List<ICartItemModel> cartItems = new List<ICartItemModel>()
            {
                new CartItemModel()
                {
                    SKU = "A",
                    Price = 100,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 100,
                },
                new CartItemModel()
                {
                    SKU = "B",
                    Price = 120,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 120,
                }
            };

            List<IPromotionModel> promotions = new List<IPromotionModel>()
            {
                new PromotionModel(promotion)
            };

            List<ICartItemModel> processedItems = this.multipleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);

            Assert.AreEqual(100, processedItems.First(x => x.SKU == "A").TotalPrice);
            Assert.AreEqual(120, processedItems.First(x => x.SKU == "B").TotalPrice);
        }

        [TestMethod]
        public void ApplyPromotion_Success_SinglePromotionAndItemsWithoutPromotion()
        {
            IPromotion promotion = new Promotion()
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

                    },
                },
                PromotionPrice = 30,
                PromotionType = Enum.PromotionType.MultipleItems
            };

            List<ICartItemModel> cartItems = new List<ICartItemModel>()
            {
                new CartItemModel()
                {
                    SKU = "C",
                    Price = 20,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 20,
                },
                new CartItemModel()
                {
                    SKU = "D",
                    Price = 15,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 15,
                },
                new CartItemModel()
                {
                    SKU = "A",
                    Price = 100,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 100,
                },
                new CartItemModel()
                {
                    SKU = "B",
                    Price = 100,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 120,
                }
            };

            List<IPromotionModel> promotions = new List<IPromotionModel>()
            {
                new PromotionModel(promotion)
            };

            List<ICartItemModel> processedItems = this.multipleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);

            Assert.AreEqual(100, processedItems.First(x => x.SKU == "A").TotalPrice);
            Assert.AreEqual(120, processedItems.First(x => x.SKU == "B").TotalPrice);
            Assert.AreEqual(30, processedItems.First(x => x.SKU == "C").TotalPrice);
            Assert.AreEqual(0, processedItems.First(x => x.SKU == "D").TotalPrice);
        }

        #endregion Apply Promotion Tests
    }
}
