using PromotionEngine.Enum;
using System.Collections.Generic;


namespace PromotionEngine.Interfaces
{
    public interface IPromotionStrategy
    {
        PromotionType PromotionType { get; }
        List<ICartItemModel> ApplyPromotions(List<ICartItemModel> cartItems, IList<IPromotionModel> promotions);
    }
}
