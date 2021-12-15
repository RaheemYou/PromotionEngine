using PromotionEngine.Interfaces;
using PromotionEngine.Models.EntityModels;

namespace PromotionEngine.Models.BusinessModels
{
    /// <summary>
    /// The PromotionItem business model.
    /// </summary>
    public class PromotionItemModel : IPromotionItemModel
    {
        private IPromotionItem promotionItem { get; set; }

        /// <summary>
        /// We create this as internal so only the business service can create models, ideally we would have different projects per layer.
        /// If we used the repo/unit of work pattern we could have mocked the data layer, making it private without this makes it harder to test so I have made it public.
        /// </summary>
        /// <param name="promotionItem">The promotion item.</param>
        public PromotionItemModel(IPromotionItem promotionItem)
        {
            this.promotionItem = promotionItem;
        }

        /// <summary>
        /// Gets or sets the SKU for the Promotion.
        /// </summary>
        public string SKU
        {
            get
            {
                return this.promotionItem.SKU;
            }

            set
            {
                // By setting this entity model, if we are using entity framework and the entity model has been assigned correctly, this will allow a commit to persist the data.
                this.promotionItem.SKU = value;
            }
        }

        /// <summary>
        /// Gets or sets the Item Quantity for the Promotion. 
        /// </summary>
        public int Quantity
        {
            get
            {
                return this.promotionItem.Quantity;
            }

            set
            {
                // By setting this entity model, if we are using entity framework and the entity model has been assigned correctly, this will allow a commit to persist the data.
                this.promotionItem.Quantity = value;
            }
        }
    }
}
