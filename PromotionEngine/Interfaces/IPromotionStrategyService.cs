using PromotionEngine.Models.BusinessModels;
using System.Collections.Generic;

namespace PromotionEngine.Interfaces
{
    public interface IPromotionStrategyService
    {
        List<CartItemModel> ApplyPromotionStrategies(List<CartItemModel> cartItems);
    }
}
