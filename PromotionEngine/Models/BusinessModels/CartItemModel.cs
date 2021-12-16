using PromotionEngine.Interfaces;
using System.Text.Json.Serialization;

namespace PromotionEngine.Models.BusinessModels
{
    /// <summary>
    /// The CartItem Model class.
    /// </summary>
    public class CartItemModel
    {
        /// <summary>
        /// Gets or sets the item string.
        /// </summary>
        public string SKU { get; set; }

        /// <summary>
        /// Gets or sets the Price.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the Quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the TotalPrice.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Gets or sets the PromotionApplied.
        /// </summary>
        [JsonIgnore]
        public bool PromotionApplied { get; set; }
    }
}
