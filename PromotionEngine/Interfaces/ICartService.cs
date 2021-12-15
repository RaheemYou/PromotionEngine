namespace PromotionEngine.Interfaces
{
    public interface ICartService
    {
        ICartModel CalculateTotalPromotionPrice(ICartModel cartModel);
    }
}
