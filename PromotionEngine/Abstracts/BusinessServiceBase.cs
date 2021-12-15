using Microsoft.Extensions.Logging;

namespace PromotionEngine.Abstracts
{
    /// <summary>
    /// The Business service base class.
    /// </summary>
    /// <typeparam name="S">The service class.</typeparam>
    public abstract class BusinessServiceBase<S>
        where S : class
    {
        internal ILogger<S> logger { get; set; }

        public BusinessServiceBase(ILogger<S> logger)
        {
            this.logger = logger;
        }
    }
}
