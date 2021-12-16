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
    public class SingleItemPromotionStrategyTests : TestBase
    {
        private Mock<ILogger<SingleItemPromotionStrategy>> mockLogger { get; set; }
        private SingleItemPromotionStrategy SingleItemPromotionStrategy { get; set; }

        public SingleItemPromotionStrategyTests()
        {
            this.mockLogger = new Mock<ILogger<SingleItemPromotionStrategy>>();
            this.SingleItemPromotionStrategy = new SingleItemPromotionStrategy(this.mockLogger.Object);
        }

        #region Can Apply Promotion Tests

        [TestMethod]
        public void CanApplyPromotion_ThrowsArgumentNullException_WhenCartItemsNull()
        {
            try
            {
                this.SingleItemPromotionStrategy.CanApplyPromotion(null, new Mock<IPromotionModel>().Object);
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
                this.SingleItemPromotionStrategy.CanApplyPromotion(new List<ICartItemModel>(), null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("promotion", ex.ParamName);
            }
        }

        [TestMethod]
        public void CanApplyPromotion_ReturnsFalse_WhenPromotionHasNoItems()
        {
            IPromotionModel promotion = this.ValidSingleItemPromotion();
            promotion.PromotionItems = new List<IPromotionItemModel>();


            List<ICartItemModel> cartItems = this.ValidCartItems();

            Assert.IsFalse(this.SingleItemPromotionStrategy.CanApplyPromotion(cartItems, promotion));
        }

        [TestMethod]
        public void CanApplyPromotion_ReturnsFalse_WhenPromotionIsInactive()
        {
            IPromotionModel promotion = this.ValidSingleItemPromotion();
            promotion.Active = false;

            List<ICartItemModel> cartItems = this.ValidCartItems();

            Assert.IsFalse(this.SingleItemPromotionStrategy.CanApplyPromotion(cartItems, promotion));
        }

        [TestMethod]
        public void CanApplyPromotion_ReturnsFalse_EmptyCartItems()
        {
            IPromotionModel promotion = this.ValidSingleItemPromotion();
            Assert.IsFalse(this.SingleItemPromotionStrategy.CanApplyPromotion(new List<ICartItemModel>(), promotion));
        }

        [TestMethod]
        public void CanApplyPromotion_False_MultiItemPromotion()
        {
            IPromotionModel promotion = this.ValidSingleItemPromotion();
            promotion.PromotionType = Enum.PromotionType.MultipleItems;

            List<ICartItemModel> cartItems = this.ValidCartItems();

            Assert.IsFalse(this.SingleItemPromotionStrategy.CanApplyPromotion(cartItems, promotion));
        }

        [TestMethod]
        public void CanApplyPromotion_True()
        {
            IPromotionModel promotion = this.ValidSingleItemPromotion();

            List<ICartItemModel> cartItems = this.ValidCartItems();

            Assert.IsTrue(this.SingleItemPromotionStrategy.CanApplyPromotion(cartItems, promotion));
        }

        [TestMethod]
        public void CanApplyPromotion_False_InvalidQuantity()
        {
            IPromotionModel promotion = this.ValidSingleItemPromotion();
            promotion.PromotionItems = promotion.PromotionItems.Select(x=> {
                x.Quantity = 4;
                return x;
            }).ToList();

            List<ICartItemModel> cartItems = this.ValidCartItems();

            Assert.IsFalse(this.SingleItemPromotionStrategy.CanApplyPromotion(cartItems, promotion));
        }

        [TestMethod]
        public void CanApplyPromotion_False_NoMatchingSKU()
        {
            IPromotionModel promotion = this.ValidSingleItemPromotion();

            List<ICartItemModel> cartItems = this.ValidCartItems().Select(x =>
            {
                if (x.SKU == "A")
                {
                    x.SKU = "C";
                }

                return x;
            }).ToList();

            Assert.IsFalse(this.SingleItemPromotionStrategy.CanApplyPromotion(cartItems, promotion));
        }

        #endregion Can Apply Promotion Tests

        #region Apply Promotion Tests

        [TestMethod]
        public void ApplyPromotion_ThrowsArgumentNullException_NullPromotions()
        {
            try
            {
                List<ICartItemModel> cartItems = this.ValidCartItems();

                List<IPromotionModel> promotions = null;

                List<ICartItemModel> processedItems = this.SingleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);
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
                IPromotionModel promotion = this.ValidSingleItemPromotion();

                List<ICartItemModel> cartItems = null;

                List<IPromotionModel> promotions = new List<IPromotionModel>()
                {
                    promotion
                };

                List<ICartItemModel> processedItems = this.SingleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("cartItems", ex.ParamName);
            }
        }

        [TestMethod]
        public void ApplyPromotion_Success_EmptyPromotions()
        {
            List<ICartItemModel> cartItems = this.ValidCartItems();

            List<IPromotionModel> promotions = new List<IPromotionModel>();

            List<ICartItemModel> processedItems = this.SingleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);

            Assert.AreEqual(cartItems, processedItems);
        }

        [TestMethod]
        public void ApplyPromotion_Sucess_EmptyCartItems()
        {
            IPromotionModel promotion = this.ValidSingleItemPromotion();

            List<ICartItemModel> cartItems = new List<ICartItemModel>();

            List<IPromotionModel> promotions = new List<IPromotionModel>()
            {
                promotion
            };

            List<ICartItemModel> processedItems = this.SingleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);

            Assert.IsFalse(processedItems.Any());
        }

        [TestMethod]
        public void ApplyPromotion_Success_SinglePromotion()
        {
            IPromotionModel promotion = this.ValidSingleItemPromotion();

            List<ICartItemModel> cartItems = this.ValidCartItems();

            List<IPromotionModel> promotions = new List<IPromotionModel>()
            {
                promotion
            };

            List<ICartItemModel> processedItems = this.SingleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);

            Assert.AreEqual(130, processedItems.First(x => x.SKU == "A").TotalPrice);
            Assert.AreEqual(60, processedItems.First(x => x.SKU == "B").TotalPrice);
        }

        [TestMethod]
        public void ApplyPromotion_Success_MultiplePromotion()
        {
            IPromotionModel promotion = this.ValidSingleItemPromotion();

            List<ICartItemModel> cartItems = this.ValidCartItems();

            List<IPromotionModel> promotions = new List<IPromotionModel>()
            {
                promotion,
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
                    PromotionType = Enum.PromotionType.SingleItem,
                })
            };

            List<ICartItemModel> processedItems = this.SingleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);

            Assert.AreEqual(130, processedItems.First(x => x.SKU == "A").TotalPrice);
            Assert.AreEqual(45, processedItems.First(x => x.SKU == "B").TotalPrice);
        }

        [TestMethod]
        public void ApplyPromotion_Success_PromotionQuantityNotReached()
        {
            IPromotionModel promotion = this.ValidSingleItemPromotion();
            promotion.PromotionItems = promotion.PromotionItems.Select(x =>
            {
                x.Quantity = 50;
                return x;
            }).ToList();

            List<ICartItemModel> cartItems = this.ValidCartItems();

            List<IPromotionModel> promotions = new List<IPromotionModel>()
            {
                promotion
            };

            List<ICartItemModel> processedItems = this.SingleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);

            Assert.AreEqual(150, processedItems.First(x => x.SKU == "A").TotalPrice);
            Assert.AreEqual(60, processedItems.First(x => x.SKU == "B").TotalPrice);
        }


        [TestMethod]
        public void ApplyPromotion_Success_CartItemQuantityGreaterThanPromotionQuantity()
        {
            IPromotionModel promotion = this.ValidSingleItemPromotion();

            List<ICartItemModel> cartItems = this.ValidCartItems().Select(x=> 
            {
                if (x.SKU == "A")
                {
                    x.Quantity = 4;
                }

                return x;
            }).ToList();

            List<IPromotionModel> promotions = new List<IPromotionModel>()
            {
                promotion
            };

            List<ICartItemModel> processedItems = this.SingleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);

            Assert.AreEqual(180, processedItems.First(x => x.SKU == "A").TotalPrice);
            Assert.AreEqual(60, processedItems.First(x => x.SKU == "B").TotalPrice);
        }

        [TestMethod]
        public void ApplyPromotion_Success_PromotionAlreadyApplied()
        {
            IPromotionModel promotion = this.ValidSingleItemPromotion();

            List<ICartItemModel> cartItems = this.ValidCartItems().Select(x =>
            {
                x.PromotionApplied = true;
                return x;
            }).ToList();

            List<IPromotionModel> promotions = new List<IPromotionModel>()
            {
                promotion
            };

            List<ICartItemModel> processedItems = this.SingleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);

            Assert.AreEqual(150, processedItems.First(x => x.SKU == "A").TotalPrice);
            Assert.AreEqual(60, processedItems.First(x => x.SKU == "B").TotalPrice);

            Assert.AreEqual(cartItems.Count, processedItems.Count);
        }

        [TestMethod]
        public void ApplyPromotion_Success_ItemWithoutPromotion()
        {
            IPromotionModel promotion = this.ValidSingleItemPromotion();

            List<ICartItemModel> cartItems = this.ValidCartItems().Select(x =>
            {
                if (x.SKU == "A")
                {
                    x.SKU = "C";
                }

                return x;
            }).ToList();

            List<IPromotionModel> promotions = new List<IPromotionModel>()
            {
                promotion
            };

            List<ICartItemModel> processedItems = this.SingleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);

            Assert.AreEqual(150, processedItems.First(x => x.SKU == "C").TotalPrice);
            Assert.AreEqual(60, processedItems.First(x => x.SKU == "B").TotalPrice);
        }

        [TestMethod]
        public void ApplyPromotion_Success_ItemWithAndWithoutPromotion()
        {
            IPromotionModel promotion = this.ValidSingleItemPromotion();

            List<ICartItemModel> cartItems = this.ValidCartItems();

            cartItems.Add(new CartItemModel()
            {
                SKU = "C",
                Price = 20,
                PromotionApplied = false,
                Quantity = 1,
                TotalPrice = 20,
            });

            cartItems.Add(new CartItemModel()
            {
                SKU = "D",
                Price = 15,
                PromotionApplied = false,
                Quantity = 1,
                TotalPrice = 15,
            });

            List<IPromotionModel> promotions = new List<IPromotionModel>()
            {
                promotion,
                new PromotionModel(new Promotion()
                {
                    Active = true,
                    PromotionPrice = 45,
                    PromotionType = Enum.PromotionType.SingleItem,
                    PromotionItems = new List<IPromotionItem>()
                    {
                        new PromotionItem()
                        {
                            SKU = "B",
                            Quantity = 2,
                        }
                    }
                })
            };

            List<ICartItemModel> processedItems = this.SingleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);

            Assert.AreEqual(130, processedItems.First(x => x.SKU == "A").TotalPrice);
            Assert.AreEqual(45, processedItems.First(x => x.SKU == "B").TotalPrice);
            Assert.AreEqual(20, processedItems.First(x => x.SKU == "C").TotalPrice);
            Assert.AreEqual(15, processedItems.First(x => x.SKU == "D").TotalPrice);
        }

        #endregion Apply Promotion Tests

        #region Private Methods

        /// <summary>
        /// A Valid single item promotion for tests.
        /// </summary>
        /// <returns>A valid single item promotion.</returns>
        private IPromotionModel ValidSingleItemPromotion()
        {
            return new PromotionModel(
                new Promotion()
                {
                    Active = true,
                    PromotionItems = new List<IPromotionItem>()
                    {
                        new PromotionItem()
                        {
                            SKU = "A",
                            Quantity = 3,
                        },
                    },
                    PromotionPrice = 130,
                    PromotionType = Enum.PromotionType.SingleItem,
                });
        }

        /// <summary>
        /// A Valid collection of cart items.
        /// </summary>
        /// <returns>A valid collection of cart items.</returns>
        private List<ICartItemModel> ValidCartItems()
        {
            return new List<ICartItemModel>()
            {
                new CartItemModel()
                {
                    SKU = "A",
                    Price = 50,
                    PromotionApplied = false,
                    Quantity = 3,
                    TotalPrice = 150,
                },
                new CartItemModel()
                {
                    SKU = "B",
                    Price = 30,
                    PromotionApplied = false,
                    Quantity = 2,
                    TotalPrice = 60,
                },
            };
        }

        #endregion Private Methods
    }
}
