using Microsoft.Extensions.Logging;
using PromotionEngine.Enum;
using PromotionEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngine.PromotionStrategies
{
    /// <summary>
    /// The MultipleItemPromotionStrategy class.
    /// </summary>
    public class MultipleItemPromotionStrategy : IPromotionStrategy
    {
        private ILogger<MultipleItemPromotionStrategy> logger { get; set; }

        /// <summary>
        /// Initializes a new instance of the MultipleItemPromotionStrategy class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public MultipleItemPromotionStrategy(ILogger<MultipleItemPromotionStrategy> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Gets the promotion type.
        /// </summary>
        public PromotionType PromotionType
        {
            get
            {
                return PromotionType.MultipleItems;
            }
        }

        /// <summary>
        /// Apply the promotions to the cart items.
        /// </summary>
        /// <param name="cartItems">The cart Items.</param>
        /// <param name="promotions">The promotion.</param>
        /// <returns>The cart items with the promotions applied.</returns>
        public List<ICartItemModel> ApplyPromotions(List<ICartItemModel> cartItems, IList<IPromotionModel> promotions)
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
                List<ICartItemModel> itemsWithPromotionApplied = cartItems.Where(x=> x.PromotionApplied).ToList();

                foreach (IPromotionModel promotion in promotions)
                {
                    // Check whether the promotion can be applied to the cart items.
                    if (this.CanApplyPromotion(cartItems, promotion))
                    {
                        // We do not want to apply multiple promotions to an item i.e. C & D for 30 and C & B for 30
                        if (!itemsWithPromotionApplied.Any(x => promotion.PromotionItems.Any(y => y.SKU == x.SKU)))
                        {
                            // Get the cart items for the promotion.
                            List<ICartItemModel> promotedItems = cartItems.Where(x => promotion.PromotionItems.Any(y => y.SKU == x.SKU)).ToList();

                            if (promotedItems.Any())
                            {
                                logger.LogInformation($"Applying Promotion: {promotion.PromotionType}, {promotion.PromotionItems.Select(x=> x.SKU)}");

                                // Calculate the promotion that should be applied to the cart items.
                                for (int i = 0; i < promotedItems.Count; i++)
                                {
                                    ICartItemModel promotedItem = promotedItems.ElementAt(i);

                                    // The first element in the cart item is the ine that has the promotion price applied to it.
                                    this.ApplyToCartItem(promotedItem, promotion, i == 0);
                                    itemsWithPromotionApplied.Add(promotedItem);

                                }
                            }
                        }
                    }
                }

                // Get the items that did not have a promotion.
                List<ICartItemModel> notPromotionItems = cartItems.Where(x => !itemsWithPromotionApplied.Any(y => y.SKU == x.SKU)).ToList();
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
        public bool CanApplyPromotion(IList<ICartItemModel> cartItems, IPromotionModel promotion)
        {
            if (cartItems == null)
            {
                throw new ArgumentNullException("cartItems");
            }

            if (promotion == null)
            {
                throw new ArgumentNullException("promotion");
            }

            if (cartItems.Any() && promotion.Active && promotion.PromotionItems.Any())
            {
                List<string> promotionItems = promotion.PromotionItems.Select(x => x.SKU).ToList();

                if (promotionItems.Count == cartItems.Count(x => promotionItems.Contains(x.SKU)))
                {
                    // Ensure the quantity is correct.
                    foreach(IPromotionItem promotionItem in promotion.PromotionItems)
                    {
                        // If the cart item has less quantity than what is needed for the promotion then the promotion cannot be applied.
                        if (cartItems.First(x => x.SKU == promotionItem.SKU).Quantity < promotionItem.Quantity)
                        {
                            return false;
                        }
                    }

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
        /// <param name="applyPromotionPrice">Whether to apply the promotion price to the cart item price.</param>
        /// <returns>The updated cart item.</returns>
        private ICartItemModel ApplyToCartItem(ICartItemModel cartItem, IPromotionModel promotion, bool applyPromotionPrice)
        {
            int promotionItemQuantity = promotion.PromotionItems.First(x => x.SKU == cartItem.SKU).Quantity;

            cartItem.TotalPrice =  cartItem.Price * (cartItem.Quantity - promotionItemQuantity);

            if (applyPromotionPrice)
            {
                cartItem.TotalPrice += promotion.PromotionPrice;
            }

            cartItem.PromotionApplied = true;

            return cartItem;
        }
    }
}
