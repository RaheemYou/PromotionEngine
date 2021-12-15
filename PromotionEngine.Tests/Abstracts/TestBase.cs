using Moq;
using PromotionEngine.Interfaces;

namespace PromotionEngine.Tests.Abstracts
{
    public abstract class TestBase
    {
        public IPromotionRepository PromotionRepository { get; set; }
        public Mock<IPromotionRepository> MockPromotionRepository { get; set; }
    }
}
