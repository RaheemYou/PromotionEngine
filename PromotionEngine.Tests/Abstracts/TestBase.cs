using Moq;
using PromotionEngine.Interfaces;

namespace PromotionEngine.Tests.Abstracts
{
    public abstract class TestBase
    {
        internal Mock<IPromotionRepository> MockPromotionRepository { get; set; }
        internal Mock<IPromotionService> MockPromotionService { get; set; }
    }
}
