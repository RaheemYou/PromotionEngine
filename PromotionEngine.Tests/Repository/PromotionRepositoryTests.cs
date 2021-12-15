using Microsoft.VisualStudio.TestTools.UnitTesting;
using PromotionEngine.Enum;
using PromotionEngine.Models;
using PromotionEngine.Repository;
using PromotionEngine.Tests.Abstracts;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngine.Tests.Repository
{
    [TestClass]
    public class PromotionRepositoryTests : TestBase
    {
        public PromotionRepositoryTests()
        {
            this.PromotionRepository = new PromotionRepository();
        }

        [TestMethod]
        public void GetActiveByPromotionType_SingleItem()
        {
            List<Promotion> promotions = this.PromotionRepository.GetActiveByPromotionType(PromotionType.SingleItem).ToList();

            Assert.AreEqual(promotions.Count, 2);
        }

        [TestMethod]
        public void GetActiveByPromotionType_MultipleItems()
        {
            List<Promotion> promotions = this.PromotionRepository.GetActiveByPromotionType(PromotionType.MultipleItems).ToList();

            Assert.AreEqual(promotions.Count, 1);
        }
    }
}
