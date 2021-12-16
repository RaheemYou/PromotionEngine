using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PromotionEngine.Repository;
using PromotionEngine.Services;
using PromotionEngine.Services.PromotionStrategies;

namespace PromotionEngine
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton<AppSettings>();

            // Services
            services.AddScoped<CartService>();
            services.AddScoped<PromotionService>();
            services.AddScoped<PromotionStrategyService>();

            // Promotion Strategies
            services.AddScoped<SingleItemPromotionStrategy>();
            services.AddScoped<MultipleItemPromotionStrategy>();

            // Repositories
            services.AddScoped<PromotionRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
