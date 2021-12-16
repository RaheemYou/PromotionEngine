using Microsoft.Extensions.Configuration;
using PromotionEngine.Interfaces;

namespace PromotionEngine
{
    /// <summary>
    /// The AppSettings class.
    /// </summary>
    public class AppSettings : IAppSettings
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
