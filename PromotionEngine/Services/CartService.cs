using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using PromotionEngine.Abstracts;
using PromotionEngine.Interfaces;
using PromotionEngine.Validators;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngine.Services
{
    public class CartService : BusinessServiceBase<ICartService>, ICartService
    {
        private IPromotionService promotionService { get; set; }
        private IPromotionStrategy promotionStrategy { get; set; }

        public CartService(IPromotionService promotionService, IPromotionStrategy promotionStrategy, ILogger<ICartService> logger) : base(logger)
        {
            this.promotionService = promotionService;
            this.promotionStrategy = promotionStrategy;
        }

        /// <summary>
        /// Calculates the total cart price with promotions applied.
        /// </summary>
        /// <param name="cartModel">The cartModel.</param>
        /// <returns>The updated cartmodel with promotions applied.</returns>
        public ICartModel CalculateTotalPromotionPrice(ICartModel cartModel)
        {
            this.logger.LogInformation("Calculating the total promotion price.");

            CartValidator cartValidator = new CartValidator();
            ValidationResult validationResult = cartValidator.Validate(cartModel);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            List<IPromotionModel> promotions = this.promotionService.GetActivePromotions(this.promotionStrategy.PromotionType);

            cartModel.CartItems = this.promotionStrategy.ApplyPromotions(cartModel.CartItems, promotions);
            cartModel.TotalPrice = cartModel.CartItems.Sum(x => x.TotalPrice);

            return cartModel;
        }
    }
}
