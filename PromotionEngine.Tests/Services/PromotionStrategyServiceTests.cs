using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PromotionEngine.Enum;
using PromotionEngine.Interfaces;
using PromotionEngine.Services;
using PromotionEngine.Services.PromotionStrategies;
using PromotionEngine.Tests.Abstracts;
using System;
using System.Collections.Generic;

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

            SingleItemPromotionStrategy singleItemPromotionStrategy = new SingleItemPromotionStrategy(new Mock<ILogger<SingleItemPromotionStrategy>>().Object);
            MultipleItemPromotionStrategy multipleItemPromotionStrategy = new MultipleItemPromotionStrategy(new Mock<ILogger<MultipleItemPromotionStrategy>>().Object);

            this.MockPromotionService.Setup(x => x.GetActivePromotions(It.IsAny<PromotionType>())).Returns(new List<IPromotionModel>());

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
    }
}
