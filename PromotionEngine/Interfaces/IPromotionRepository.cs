using PromotionEngine.Enum;
using PromotionEngine.Models;
using System.Collections.Generic;

namespace PromotionEngine.Interfaces
{
    public interface IPromotionRepository
    {
        IEnumerable<Promotion> GetActiveByPromotionType(PromotionType promotionType);
    }
}
