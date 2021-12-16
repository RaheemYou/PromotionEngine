using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Logging;
using Moq;
using PromotionEngine.Enum;
using PromotionEngine.Models.EntityModels;
using PromotionEngine.Repository;
using PromotionEngine.Tests.Abstracts;
using System.Collections.Generic;
using System.Linq;
using PromotionEngine.Interfaces;
using System;

namespace PromotionEngine.Tests.Repository
{
    [TestClass]
    public class PromotionRepositoryTests : TestBase
    {
        private Mock<ILogger<IPromotionRepository>> mockLogger {get; set;}
        public IPromotionRepository PromotionRepository { get; set; }

        public PromotionRepositoryTests()
        {
            this.mockLogger = new Mock<ILogger<IPromotionRepository>>();

            this.PromotionRepository = new PromotionRepository(mockLogger.Object);
        }

        [TestMethod]
        public void GetActiveByPromotionType_SingleItem()
        {
            List<IPromotion> promotions = this.PromotionRepository.GetActiveByPromotionType(PromotionType.SingleItem).ToList();

            // Verify the log message.
            this.mockLogger.Verify(x => x.Log(
            It.Is<LogLevel>(l => l == LogLevel.Information),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString() == "Getting Active Promotions by Type in the repo."),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));

            Assert.AreEqual(promotions.Count, 2);
        }

        [TestMethod]
        public void GetActiveByPromotionType_MultipleItems()
        {
            List<IPromotion> promotions = this.PromotionRepository.GetActiveByPromotionType(PromotionType.MultipleItems).ToList();

            // Verify the log message.
            this.mockLogger.Verify(x => x.Log(
            It.Is<LogLevel>(l => l == LogLevel.Information),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString() == "Getting Active Promotions by Type in the repo."),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));

            Assert.AreEqual(promotions.Count, 1);
        }
    }
}
