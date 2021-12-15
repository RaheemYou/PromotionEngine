using PromotionEngine.Enum;
using PromotionEngine.Interfaces;
using PromotionEngine.Models.EntityModels;
using System.Collections.Generic;

namespace PromotionEngine.Models.BusinessModels
{
    /// <summary>
    /// The Promotion Business Model.
    /// </summary>
    public class PromotionModel : IPromotionModel
    {
        private IPromotion promotion { get; set; }

        /// <summary>
        /// We create this as internal so only the business service can create models, ideally we would have different projects per layer.
        /// If we used the repo/unit of work pattern we could have mocked the data layer, making it private without this makes it harder to test so I have made it public.
        /// </summary>
        /// <param name="promotion">The promotion.</param>
        public PromotionModel(IPromotion promotion)
        {
            this.promotion = promotion;
        }

        /// <summary>
        /// Gets or sets the PromotionPrice.
        /// </summary>
        public decimal PromotionPrice
        {
            get
            {
                return this.promotion.PromotionPrice;
            }

            set
            {
                // By setting this entity model, if we are using entity framework and the entity model has been assigned correctly, this will allow a commit to persist the data.
                this.promotion.PromotionPrice = value;
            }
        }

        /// <summary>
        /// Gets or sets the PromotionType.
        /// </summary>
        public PromotionType PromotionType
        {
            get
            {
                return this.promotion.PromotionType;
            }

            set
            {
                // By setting this entity model, if we are using entity framework and the entity model has been assigned correctly, this will allow a commit to persist the data.
                this.promotion.PromotionType = value;
            }
        }

        /// <summary>
        /// Gets or sets the collection of Promotion items related to the promotion.
        /// </summary>
        public List<IPromotionItem> PromotionItems
        {
            get
            {
                return this.promotion.PromotionItems;
            }

            set
            {
                // By setting this entity model, if we are using entity framework and the entity model has been assigned correctly, this will allow a commit to persist the data.
                this.promotion.PromotionItems = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the promotion is active.
        /// </summary>
        public bool Active
        {
            get
            {
                return this.promotion.Active;
            }

            set
            {
                // By setting this entity model, if we are using entity framework and the entity model has been assigned correctly, this will allow a commit to persist the data.
                this.promotion.Active = value;
            }
        }
    }
}
