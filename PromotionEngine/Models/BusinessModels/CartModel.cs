using PromotionEngine.Interfaces;
using System.Collections.Generic;

namespace PromotionEngine.Models.BusinessModels
{
    /// <summary>
    /// The CartModel class.
    /// </summary>
    public class CartModel
    {
        /// <summary>
        /// Gets or sets a collection of CartItems.
        /// </summary>
        public List<CartItemModel> CartItems { get; set; }

        /// <summary>
        /// Gets or sets the TotalPrice. 
        /// </summary>
        public decimal TotalPrice { get; set; }
    }
}
