namespace PromotionEngine.Interfaces
{
    public interface ICartItemModel
    {
        string SKU { get; set; }
        decimal Price { get; set; }
        int Quantity { get; set; }
        decimal TotalPrice { get; set; }
        bool PromotionApplied { get; set; }
    }
}
