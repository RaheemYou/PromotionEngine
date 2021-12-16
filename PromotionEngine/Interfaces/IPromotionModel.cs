using PromotionEngine.Enum;
using System.Collections.Generic;

namespace PromotionEngine.Interfaces
{
    public interface IPromotionModel
    {
        decimal PromotionPrice { get; set; }
        PromotionType PromotionType { get; set; }
        List<IPromotionItemModel> PromotionItems { get; set; }
        bool Active { get; set; }
    }
}
