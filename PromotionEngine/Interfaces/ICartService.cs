using PromotionEngine.Models.BusinessModels;

namespace PromotionEngine.Interfaces
{
    public interface ICartService
    {
        CartModel CalculateTotalPromotionPrice(CartModel cartModel);
    }
}
