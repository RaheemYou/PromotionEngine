using PromotionEngine.Enum;
using PromotionEngine.Interfaces;
using System.Collections.Generic;

namespace PromotionEngine.Models
{
    /// <summary>
    /// The Promotion entity model.
    /// </summary>
    public class Promotion : IPromotion
    {
        /// <summary>
        /// Gets or sets the PromotionPrice.
        /// </summary>
        public decimal PromotionPrice { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Promotion is active or not.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the PromotionType.
        /// </summary>
        public PromotionType PromotionType { get; set; }

        /// <summary>
        /// Gets or sets the collection of Promotion items related to the promotion.
        /// </summary>
        public List<IPromotionItem> PromotionItems { get; set; }
    }
}
