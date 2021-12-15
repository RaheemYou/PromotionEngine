using System.Collections.Generic;

namespace PromotionEngine.Interfaces
{
    public interface ICartModel
    {
        List<ICartItemModel> CartItems { get; set; }

        decimal TotalPrice { get; set; }
    }
}
