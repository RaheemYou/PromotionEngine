using Microsoft.VisualStudio.TestTools.UnitTesting;
using PromotionEngine.Enum;
using PromotionEngine.Interfaces;
using PromotionEngine.Models.BusinessModels;
using PromotionEngine.Models.EntityModels;
using PromotionEngine.Services;
using PromotionEngine.Tests.Abstracts;
using System.Collections.Generic;

namespace PromotionEngine.Tests.Services
{
    [TestClass]
    public class CartServiceTests : TestBase
    {
        private ICartService cartService { get; set; }

        public CartServiceTests()
        {
            //this.cartService = new CartService()
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
