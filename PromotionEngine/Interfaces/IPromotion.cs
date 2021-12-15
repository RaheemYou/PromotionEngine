using PromotionEngine.Enum;
using System.Collections.Generic;

namespace PromotionEngine.Interfaces
{
    public interface IPromotion
    {
        decimal PromotionPrice { get; set; }
        bool Active { get; set; }
        PromotionType PromotionType { get; set; }
        List<IPromotionItem> PromotionItems { get; set; }
    }
}
