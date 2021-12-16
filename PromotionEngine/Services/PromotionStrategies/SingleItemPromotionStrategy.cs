using Microsoft.Extensions.Logging;
using PromotionEngine.Enum;
using PromotionEngine.Interfaces;
using PromotionEngine.Models.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngine.Services.PromotionStrategies
{
    public class SingleItemPromotionStrategy : IPromotionStrategy
    {
        /// <summary>
        /// Gets the promotion type.
        /// </summary>
        public PromotionType PromotionType
        {
            get
            {
                return PromotionType.SingleItem;
            }
        }

        private ILogger<SingleItemPromotionStrategy> logger { get; set; }

        /// <summary>
        /// Initializes a new instance of the SingleItemPromotionStrategy class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public SingleItemPromotionStrategy(ILogger<SingleItemPromotionStrategy> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Applies the promotions to the cart items.
        /// </summary>
        /// <param name="cartItems">The cart items.</param>
        /// <param name="promotions">The promotion.</param>
        /// <returns>An updated list of cart items.</returns>
        public List<CartItemModel> ApplyPromotions(List<CartItemModel> cartItems, IList<IPromotionModel> promotions)
        {
            if (cartItems == null)
            {
                throw new ArgumentNullException("cartItems");
            }

            if (promotions == null)
            {
                throw new ArgumentNullException("promotions");
            }

            if (cartItems.Any() && promotions.Any())
            {
                // Add the promotions that have been applied potentially by another strategy.
                List<CartItemModel> itemsWithPromotionApplied = cartItems.Where(x => x.PromotionApplied).ToList();

                foreach (IPromotionModel promotion in promotions)
                {
                    // Check whether the promotion can be applied to the cart items.
                    if (this.CanApplyPromotion(cartItems, promotion))
                    {
                        // Get the promotion item.
                        IPromotionItemModel promotionItem = promotion.PromotionItems.First();

                        // Get the cart item related to the promotion item.
                        CartItemModel cartItem = cartItems.First(x => x.SKU == promotionItem.SKU);

                        // Only apply the promotion if a promotion has not already been applied to it.
                        if (!itemsWithPromotionApplied.Any(x => x.SKU == promotionItem.SKU))
                        {
                            this.ApplyToCartItem(cartItem, promotion, promotionItem);
                            itemsWithPromotionApplied.Add(cartItem);
                        }
                    }
                }

                // Get the items that did not have a promotion.
                List<CartItemModel> notPromotionItems = cartItems.Where(x => !itemsWithPromotionApplied.Any(y => y.SKU == x.SKU)).ToList();
                itemsWithPromotionApplied.AddRange(notPromotionItems);

                return itemsWithPromotionApplied;
            }

            return cartItems;
        }

        /// <summary>
        /// Determine whether we can apply the promotion to the cart items, only public for tests.
        /// </summary>
        /// <param name="cartItems">The cartItems.</param>
        /// <param name="promotion">The promotion.</param>
        /// <returns>True if the promotion can be applied to the items.</returns>
        public bool CanApplyPromotion(IList<CartItemModel> cartItems, IPromotionModel promotion)
        {
            if (cartItems == null)
            {
                throw new ArgumentNullException("cartItems");
            }

            if (promotion == null)
            {
                throw new ArgumentNullException("promotion");
            }

            if (cartItems.Any() && promotion.Active && promotion.PromotionItems.Any() && promotion.PromotionType == this.PromotionType)
            {
                IPromotionItemModel promotionItem = promotion.PromotionItems.FirstOrDefault(x => cartItems.Any(y=> y.SKU == x.SKU));

                // If we have a promotionItem for any cart item, make sure the cart item has the neccessary quantity for the promotion to be applied.
                if (promotionItem != null && promotionItem.Quantity <= cartItems.First(x => x.SKU == promotionItem.SKU).Quantity)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Applies the promotion item to the cart item.
        /// </summary>
        /// <param name="cartItem">The cartItem.</param>
        /// <param name="promotion">The promotion.</param>
        /// <param name="promotionItem">The promotionItem.</param>
        /// <returns>The updated cart item.</returns>
        private CartItemModel ApplyToCartItem(CartItemModel cartItem, IPromotionModel promotion, IPromotionItemModel promotionItem)
        {
            int remainder =  cartItem.Quantity % promotionItem.Quantity;
            int lotsOfPromotions = cartItem.Quantity / promotionItem.Quantity;

            cartItem.TotalPrice = (promotion.PromotionPrice * lotsOfPromotions) + (cartItem.UnitPrice * remainder);
            cartItem.PromotionApplied = true;

            return cartItem;
        }
    }
}
