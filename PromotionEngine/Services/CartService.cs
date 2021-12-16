using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using PromotionEngine.Abstracts;
using PromotionEngine.Interfaces;
using PromotionEngine.Models.BusinessModels;
using PromotionEngine.Validators;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngine.Services
{
    public class CartService : BusinessServiceBase<ICartService>, ICartService
    {
        private IPromotionStrategyService promotionStrategyService { get; set; }

        public CartService(IPromotionStrategyService promotionStrategyService, ILogger<ICartService> logger) : base(logger)
        {
            this.promotionStrategyService = promotionStrategyService;
        }

        /// <summary>
        /// Calculates the total cart price with promotions applied.
        /// </summary>
        /// <param name="cartModel">The cartModel.</param>
        /// <returns>The updated cartmodel with promotions applied.</returns>
        public CartModel CalculateTotalPromotionPrice(CartModel cartModel)
        {
            this.logger.LogInformation("Calculating the total promotion price.");

            CartValidator cartValidator = new CartValidator();
            ValidationResult validationResult = cartValidator.Validate(cartModel);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            cartModel.CartItems = this.promotionStrategyService.ApplyPromotionStrategies(cartModel.CartItems);

            cartModel.TotalPrice = cartModel.CartItems.Sum(x => x.TotalPrice);

            return cartModel;
        }
    }
}
