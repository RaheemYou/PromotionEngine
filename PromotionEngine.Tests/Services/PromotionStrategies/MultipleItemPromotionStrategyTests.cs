using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PromotionEngine.Interfaces;
using PromotionEngine.Models.BusinessModels;
using PromotionEngine.Models.EntityModels;
using PromotionEngine.Services.PromotionStrategies;
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
                this.multipleItemPromotionStrategy.CanApplyPromotion(new List<CartItemModel>(), null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("promotion", ex.ParamName);
            }
        }

        [TestMethod]
        public void CanApplyPromotion_ReturnsFalse_WhenPromotionHasNoItems()
        {

            IPromotionModel promotion = this.ValidMultipleItemPromotion();
            promotion.PromotionItems = new List<IPromotionItemModel>();

            List<CartItemModel> cartItems = this.ValidCartItems();

            Assert.IsFalse(this.multipleItemPromotionStrategy.CanApplyPromotion(cartItems, promotion));
        }

        [TestMethod]
        public void CanApplyPromotion_ReturnsFalse_WhenPromotionIsInactive()
        {
            IPromotionModel promotion = this.ValidMultipleItemPromotion();
            promotion.Active = false;

            List<CartItemModel> cartItems = this.ValidCartItems();

            Assert.IsFalse(this.multipleItemPromotionStrategy.CanApplyPromotion(cartItems, promotion));
        }

        [TestMethod]
        public void CanApplyPromotion_ReturnsFalse_EmptyCartItems()
        {
            IPromotionModel promotion = this.ValidMultipleItemPromotion();

            Assert.IsFalse(this.multipleItemPromotionStrategy.CanApplyPromotion(new List<CartItemModel>(), promotion));
        }

        [TestMethod]
        public void CanApplyPromotion_False_SingleItemPromotion()
        {
            IPromotionModel promotion = this.ValidMultipleItemPromotion();
            promotion.PromotionType = Enum.PromotionType.SingleItem;

            List<CartItemModel> cartItems = this.ValidCartItems();

            Assert.IsFalse(this.multipleItemPromotionStrategy.CanApplyPromotion(cartItems, promotion));
        }

        [TestMethod]
        public void CanApplyPromotion_True()
        {
            IPromotionModel promotion = this.ValidMultipleItemPromotion();

            List<CartItemModel> cartItems = this.ValidCartItems();

            Assert.IsTrue(this.multipleItemPromotionStrategy.CanApplyPromotion(cartItems, promotion));
        }

        [TestMethod]
        public void CanApplyPromotion_False_InvalidQuantity()
        {
            IPromotionModel promotion = this.ValidMultipleItemPromotion();
            promotion.PromotionItems = promotion.PromotionItems.Select(x =>
            {
                x.Quantity = 2;
                return x;
            }).ToList();

            List<CartItemModel> cartItems = this.ValidCartItems();

            Assert.IsFalse(this.multipleItemPromotionStrategy.CanApplyPromotion(cartItems, promotion));
        }

        [TestMethod]
        public void CanApplyPromotion_False_SingleMatchingSKU()
        {
            IPromotionModel promotion = this.ValidMultipleItemPromotion();

            List<CartItemModel> cartItems = this.ValidCartItems().Select(x=> 
            { 
                if (x.SKU == "C")
                {
                    x.SKU = "A";
                }

                return x;
            }).ToList();

            Assert.IsFalse(this.multipleItemPromotionStrategy.CanApplyPromotion(cartItems, promotion));
        }

        [TestMethod]
        public void CanApplyPromotion_False_NoMatchingSKU()
        {
            IPromotionModel promotion = this.ValidMultipleItemPromotion();

            List<CartItemModel> cartItems = this.ValidCartItems().Select(x =>
            {
                if (x.SKU == "C")
                {
                    x.SKU = "A";
                }

                if (x.SKU == "D")
                {
                    x.SKU = "B";
                }

                return x;
            }).ToList();

            Assert.IsFalse(this.multipleItemPromotionStrategy.CanApplyPromotion(cartItems, promotion));
        }

        #endregion Can Apply Promotion Tests

        #region Apply Promotion Tests

        [TestMethod]
        public void ApplyPromotion_ThrowsArgumentNullException_NullPromotions()
        {
            try
            {
                List<CartItemModel> cartItems = this.ValidCartItems();

                List<IPromotionModel> promotions = null;

                List<CartItemModel> processedItems = this.multipleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);
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
                IPromotionModel promotion = this.ValidMultipleItemPromotion();

                List<CartItemModel> cartItems = null;

                List<IPromotionModel> promotions = new List<IPromotionModel>()
                {
                    promotion
                };

                List<CartItemModel> processedItems = this.multipleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("cartItems", ex.ParamName);
            }
        }

        [TestMethod]
        public void ApplyPromotion_Success_EmptyPromotions()
        {
            List<CartItemModel> cartItems = this.ValidCartItems();

            List<IPromotionModel> promotions = new List<IPromotionModel>();

            List<CartItemModel> processedItems = this.multipleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);

            Assert.AreEqual(cartItems, processedItems);
        }

        [TestMethod]
        public void ApplyPromotion_Sucess_EmptyCartItems()
        {
            IPromotionModel promotion = this.ValidMultipleItemPromotion();

            List<CartItemModel> cartItems = new List<CartItemModel>();

            List<IPromotionModel> promotions = new List<IPromotionModel>()
            {
                promotion
            };

            List<CartItemModel> processedItems = this.multipleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);

            Assert.IsFalse(processedItems.Any());
        }

        [TestMethod]
        public void ApplyPromotion_Success_SinglePromotion()
        {
            IPromotionModel promotion = this.ValidMultipleItemPromotion();

            List<CartItemModel> cartItems = this.ValidCartItems();

            List<IPromotionModel> promotions = new List<IPromotionModel>()
            {
               promotion
            };

            List<CartItemModel> processedItems = this.multipleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);

            Assert.AreEqual(30, processedItems.First(x => x.SKU == "C").TotalPrice);
            Assert.AreEqual(0, processedItems.First(x => x.SKU == "D").TotalPrice);
        }

        [TestMethod]
        public void ApplyPromotion_Success_SinglePromotionWithMultipleCartItemQuantity()
        {
            IPromotionModel promotion = this.ValidMultipleItemPromotion();

            List<CartItemModel> cartItems = this.ValidCartItems().Select(x =>
            {
                if (x.SKU == "D")
                {
                    x.Quantity = 2;
                }

                return x;
            }).ToList();

            List<IPromotionModel> promotions = new List<IPromotionModel>()
            {
                promotion
            };

            List<CartItemModel> processedItems = this.multipleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);

            Assert.AreEqual(30, processedItems.First(x => x.SKU == "C").TotalPrice);
            Assert.AreEqual(15, processedItems.First(x => x.SKU == "D").TotalPrice);
        }

        [TestMethod]
        public void ApplyPromotion_Success_SinglePromotionWithMultiplePromotionQuantity()
        {
            IPromotionModel promotion = this.ValidMultipleItemPromotion();
            promotion.PromotionItems = promotion.PromotionItems.Select(x =>
            {
                if (x.SKU == "C")
                {
                    x.Quantity = 2;
                }

                return x;
            }).ToList();

            List<CartItemModel> cartItems = this.ValidCartItems();

            List<IPromotionModel> promotions = new List<IPromotionModel>()
            {
                promotion
            };

            List<CartItemModel> processedItems = this.multipleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);

            Assert.AreEqual(20, processedItems.First(x => x.SKU == "C").TotalPrice);
            Assert.AreEqual(15, processedItems.First(x => x.SKU == "D").TotalPrice);
        }


        [TestMethod]
        public void ApplyPromotion_Success_PromotionAlreadyApplied()
        {
            IPromotionModel promotion = this.ValidMultipleItemPromotion();

            List<CartItemModel> cartItems = this.ValidCartItems().Select(x =>
            {
                x.PromotionApplied = true;
                return x;
            }).ToList();

            List<IPromotionModel> promotions = new List<IPromotionModel>()
            {
                promotion
            };

            List<CartItemModel> processedItems = this.multipleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);

            Assert.AreEqual(20, processedItems.First(x => x.SKU == "C").TotalPrice);
            Assert.AreEqual(15, processedItems.First(x => x.SKU == "D").TotalPrice);

            Assert.AreEqual(cartItems.Count, processedItems.Count);
        }

        [TestMethod]
        public void ApplyPromotion_Success_ItemsWithoutPromotion()
        {
            IPromotionModel promotion = this.ValidMultipleItemPromotion();

            List<CartItemModel> cartItems = this.ValidCartItems().Select(x =>
            {
                if (x.SKU == "C")
                {
                    x.SKU = "A";
                }

                if (x.SKU == "D")
                {
                    x.SKU = "B";
                }

                return x;
            }).ToList();

            List<IPromotionModel> promotions = new List<IPromotionModel>()
            {
                promotion
            };

            List<CartItemModel> processedItems = this.multipleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);

            Assert.AreEqual(20, processedItems.First(x => x.SKU == "A").TotalPrice);
            Assert.AreEqual(15, processedItems.First(x => x.SKU == "B").TotalPrice);
        }

        [TestMethod]
        public void ApplyPromotion_Success_ItemsWithAndWithoutPromotion()
        {
            IPromotionModel promotion = this.ValidMultipleItemPromotion();

            List<CartItemModel> cartItems = this.ValidCartItems();

            cartItems.Add(new CartItemModel()
            {
                SKU = "A",
                UnitPrice = 100,
                PromotionApplied = false,
                Quantity = 1,
                TotalPrice = 100,
            });

            cartItems.Add(new CartItemModel()
            {
                SKU = "B",
                UnitPrice = 100,
                PromotionApplied = false,
                Quantity = 1,
                TotalPrice = 120,
            });

            List<IPromotionModel> promotions = new List<IPromotionModel>()
            {
                promotion
            };

            List<CartItemModel> processedItems = this.multipleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);

            Assert.AreEqual(100, processedItems.First(x => x.SKU == "A").TotalPrice);
            Assert.AreEqual(120, processedItems.First(x => x.SKU == "B").TotalPrice);
            Assert.AreEqual(30, processedItems.First(x => x.SKU == "C").TotalPrice);
            Assert.AreEqual(0, processedItems.First(x => x.SKU == "D").TotalPrice);
        }

        #endregion Apply Promotion Tests

        #region Private Methods

        /// <summary>
        /// A Valid promotion for tests.
        /// </summary>
        /// <returns>A valid promotion.</returns>
        private IPromotionModel ValidMultipleItemPromotion()
        {
            return new PromotionModel(
                new Promotion()
                {
                    Active = true,
                    PromotionPrice = 30,
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
                    PromotionType = Enum.PromotionType.MultipleItems,
                });
        }

        /// <summary>
        /// A Valid collection of cart items.
        /// </summary>
        /// <returns>A valid collection of cart items.</returns>
        private List<CartItemModel> ValidCartItems()
        {
            return new List<CartItemModel>()
            {
                new CartItemModel()
                {
                    SKU = "C",
                    UnitPrice = 20,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 20,
                },
                new CartItemModel()
                {
                    SKU = "D",
                    UnitPrice = 15,
                    PromotionApplied = false,
                    Quantity = 1,
                    TotalPrice = 15,
                }
            };
        }

        #endregion Private Methods
    }
}
