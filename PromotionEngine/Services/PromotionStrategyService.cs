using Microsoft.Extensions.Logging;
using PromotionEngine.Abstracts;
using PromotionEngine.Enum;
using PromotionEngine.Interfaces;
using PromotionEngine.Services.PromotionStrategies;
using System.Collections.Generic;

namespace PromotionEngine.Services
{
    public class PromotionStrategyService : BusinessServiceBase<IPromotionStrategyService>, IPromotionStrategyService
    {
        private IPromotionService promotionService { get; set; }
        private AppSettings appSettings { get; set; }
        private SingleItemPromotionStrategy singleItemPromotionStrategy { get; set; }
        private MultipleItemPromotionStrategy multipleItemPromotionStrategy { get; set; }

        public PromotionStrategyService(ILogger<IPromotionStrategyService> logger, 
            IPromotionService promotionService, 
            AppSettings appSettings,
            SingleItemPromotionStrategy singleItemPromotionStrategy, 
            MultipleItemPromotionStrategy multipleItemPromotionStrategy) : base(logger)
        {

        }

        /// <summary>
        /// Applies promotions to cart items.
        /// </summary>
        /// <param name="cartItems">The cartItems.</param>
        /// <returns>The updated cart items with applied promotions.</returns>
        public List<ICartItemModel> ApplyPromotionStrategies(List<ICartItemModel> cartItems)
        {
            string allowedPromotionTypes = this.appSettings.AllowedPromotionType;

            if (this.appSettings.AllowedPromotionType.Contains(PromotionType.SingleItem.ToString()))
            {
                List<IPromotionModel> promotions = this.promotionService.GetActivePromotions(PromotionType.SingleItem);
                cartItems = this.singleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);
            }

            if (this.appSettings.AllowedPromotionType.Contains(PromotionType.MultipleItems.ToString()))
            {
                List<IPromotionModel> promotions = this.promotionService.GetActivePromotions(PromotionType.MultipleItems);
                cartItems = this.multipleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);
            }

            return cartItems;
        }
    }
}
