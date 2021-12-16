using System.Collections.Generic;

namespace PromotionEngine.Interfaces
{
    public interface IPromotionStrategyService
    {
        List<ICartItemModel> ApplyPromotionStrategies(List<ICartItemModel> cartItems);
    }
}
