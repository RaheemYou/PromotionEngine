using Microsoft.Extensions.Logging;
using PromotionEngine.Abstracts;
using PromotionEngine.Enum;
using PromotionEngine.Interfaces;
using PromotionEngine.Models.BusinessModels;
using PromotionEngine.Services.PromotionStrategies;
using System.Collections.Generic;

namespace PromotionEngine.Services
{
    public class PromotionStrategyService : BusinessServiceBase<IPromotionStrategyService>, IPromotionStrategyService
    {
        private IPromotionService promotionService { get; set; }
        private IAppSettings appSettings { get; set; }
        private SingleItemPromotionStrategy singleItemPromotionStrategy { get; set; }
        private MultipleItemPromotionStrategy multipleItemPromotionStrategy { get; set; }

        public PromotionStrategyService(ILogger<IPromotionStrategyService> logger, 
            IPromotionService promotionService,
            IAppSettings appSettings,
            SingleItemPromotionStrategy singleItemPromotionStrategy, 
            MultipleItemPromotionStrategy multipleItemPromotionStrategy) : base(logger)
        {
            this.promotionService = promotionService;
            this.appSettings = appSettings;
            this.singleItemPromotionStrategy = singleItemPromotionStrategy;
            this.multipleItemPromotionStrategy = multipleItemPromotionStrategy;
        }

        /// <summary>
        /// Applies promotions to cart items.
        /// </summary>
        /// <param name="cartItems">The cartItems.</param>
        /// <returns>The updated cart items with applied promotions.</returns>
        public List<CartItemModel> ApplyPromotionStrategies(List<CartItemModel> cartItems)
        {
            string allowedPromotionTypes = this.appSettings.AllowedPromotionType;

            if (!string.IsNullOrEmpty(allowedPromotionTypes))
            {
                if (this.appSettings.AllowedPromotionType.Contains(PromotionType.SingleItem.ToString()))
                {
                    this.logger.LogInformation("Applying SingleItem promotion Strategy.");

                    List<IPromotionModel> promotions = this.promotionService.GetActivePromotions(PromotionType.SingleItem);
                    cartItems = this.singleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);
                }

                if (this.appSettings.AllowedPromotionType.Contains(PromotionType.MultipleItems.ToString()))
                {
                    this.logger.LogInformation("Applying MultipleItems promotion Strategy.");

                    List<IPromotionModel> promotions = this.promotionService.GetActivePromotions(PromotionType.MultipleItems);
                    cartItems = this.multipleItemPromotionStrategy.ApplyPromotions(cartItems, promotions);
                }
            }
            else
            {
                this.logger.LogInformation("AppSettings contains no active promotions.");
            }

            return cartItems;
        }
    }
}
