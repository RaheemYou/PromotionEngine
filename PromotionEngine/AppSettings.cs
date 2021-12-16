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
            this.AllowedPromotionType = configuration.GetSection("AllowedPromotionType").Value;
        }

        /// <summary>
        /// Gets the PromotionType for the application.
        /// </summary>
        public string AllowedPromotionType { get; private set; }
    }
}
