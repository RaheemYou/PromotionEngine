using PromotionEngine.Enum;
using System.Collections.Generic;

namespace PromotionEngine.Interfaces
{
    public interface IPromotionModel
    {
        decimal PromotionPrice { get; set; }
        PromotionType PromotionType { get; set; }
        List<IPromotionItem> PromotionItems { get; set; }
        bool Active { get; set; }
    }
}
