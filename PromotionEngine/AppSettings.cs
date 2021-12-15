using Microsoft.Extensions.Configuration;

namespace PromotionEngine
{
    /// <summary>
    /// The AppSettings class.
    /// </summary>
    public class AppSettings
    {
        public AppSettings(IConfiguration configuration)
        {
            this.PromotionType = configuration.GetSection("PromotionType").Value;
        }

        /// <summary>
        /// Gets the PromotionType for the application.
        /// </summary>
        public string PromotionType { get; private set; }
    }
}
