using PromotionEngine.Enum;
using PromotionEngine.Models.BusinessModels;
using System.Collections.Generic;


namespace PromotionEngine.Interfaces
{
    public interface IPromotionStrategy
    {
        PromotionType PromotionType { get; }
        List<CartItemModel> ApplyPromotions(List<CartItemModel> cartItems, IList<IPromotionModel> promotions);
    }
}
