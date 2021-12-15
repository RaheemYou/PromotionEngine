using PromotionEngine.Enum;
using PromotionEngine.Interfaces;
using PromotionEngine.Models;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngine.Repository
{
    /// <summary>
    /// The PromotionRepository class.
    /// </summary>
    public class PromotionRepository : IPromotionRepository
    {
        /// <summary>
        /// Gets a collection of active promotions filtered by type.
        /// </summary>
        /// <param name="promotionType">The promotionType.</param>
        /// <returns>A collection of promotions.</returns>
        public IEnumerable<Promotion> GetActiveByPromotionType(PromotionType promotionType)
        {
            // This will not resolve until we do a ToList on it.
            return this.Promotions().Where(x => x.Active && x.PromotionType == promotionType);
        }

        /// <summary>
        /// This provides a collection of promotions which would essentially come from the database.
        /// </summary>
        /// <returns>An IQueryable collection of promotions.</returns>
        private IQueryable<Promotion> Promotions()
        {
            return new List<Promotion>
            {
                new Promotion()
                {
                    PromotionType = PromotionType.SingleItem,
                    PromotionItems = new List<IPromotionItem>()
                    {
                        new PromotionItem()
                        {
                            Item = "A",
                            Quantity = 3,
                        }
                    },
                    PromotionPrice = 130,
                    Active =  true,
                },
                new Promotion()
                {
                    PromotionType = PromotionType.SingleItem,
                    PromotionItems = new List<IPromotionItem>()
                    {
                        new PromotionItem()
                        {
                            Item = "B",
                            Quantity = 2,
                        }
                    },
                    PromotionPrice = 45,
                    Active =  true,
                },
                new Promotion()
                {
                    PromotionType = PromotionType.MultipleItems,
                    PromotionItems = new List<IPromotionItem>()
                    {
                        new PromotionItem()
                        {
                            Item = "C",
                            Quantity = 1,
                        },
                         new PromotionItem()
                        {
                            Item = "D",
                            Quantity = 1,
                        }
                    },
                    PromotionPrice = 30,
                    Active =  true,
                },
            }.AsQueryable();
        }
    }
}
