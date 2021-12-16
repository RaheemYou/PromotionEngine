using PromotionEngine.Enum;
using System.Collections.Generic;

namespace PromotionEngine.Interfaces
{
    public interface IPromotionRepository
    {
        IEnumerable<IPromotion> GetActiveByPromotionType(PromotionType promotionType);
    }
}
