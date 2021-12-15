using PromotionEngine.Enum;
using PromotionEngine.Interfaces;
using System.Collections.Generic;

namespace PromotionEngine.PromotionStrategies
{
    public class SingleItemPromotionStrategy : IPromotionStrategy
    {
        /// <summary>
        /// Gets the promotion type.
        /// </summary>
        public PromotionType PromotionType
        {
            get
            {
                return PromotionType.SingleItem;
            }
        }


        public List<ICartItemModel> ApplyPromotions(List<ICartItemModel> cartItems, IList<IPromotionModel> promotions)
        {
            throw new System.NotImplementedException();
        }
    }
}
