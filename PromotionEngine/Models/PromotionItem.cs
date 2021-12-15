﻿using PromotionEngine.Interfaces;

namespace PromotionEngine.Models
{
    /// <summary>
    /// The PromotionItem entity model.
    /// </summary>
    public class PromotionItem : IPromotionItem
    {
        /// <summary>
        /// Gets or sets the Item for the Promotion.
        /// </summary>
        public string Item { get; set; }

        /// <summary>
        /// Gets or sets the Item Quantity for the Promotion. 
        /// </summary>
        public int Quantity { get; set; }
    }
}
