using Microsoft.Extensions.Logging;
using PromotionEngine.Abstracts;
using PromotionEngine.Enum;
using PromotionEngine.Interfaces;
using PromotionEngine.Models.BusinessModels;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngine.Services
{
    /// <summary>
    /// The PromotionService class.
    /// </summary>
    public class PromotionService : BusinessServiceBase<IPromotionService>, IPromotionService
    {
        private IPromotionRepository promotionRepository { get; set; }

        public PromotionService(IPromotionRepository promotionRepository, ILogger<IPromotionService> logger) : base(logger)
        {
            this.promotionRepository = promotionRepository;
        }

        /// <summary>
        /// Gets a collection of active promotions for the promotiontype.
        /// </summary>
        /// <param name="promotionType">The promotionType.</param>
        /// <returns>A collection of promotions for the promotion type.</returns>
        public List<IPromotionModel> GetActivePromotions(PromotionType promotionType)
        {
            this.logger.LogInformation("Getting active Promotions by type in the service.");

            return this.promotionRepository.GetActiveByPromotionType(promotionType).Select(x => (IPromotionModel)new PromotionModel(x)).ToList();
        }
    }
}
