using PromotionEngine.Enum;
using System.Collections.Generic;

namespace PromotionEngine.Interfaces
{
    public interface IPromotionService
    {
        List<IPromotionModel> GetActivePromotions(PromotionType promotionType);
    }
}
