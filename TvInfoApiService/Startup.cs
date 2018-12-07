using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TvInfo.Persistence.Models;
using TvInfoApiService.Configuration;

namespace TvInfoApiService
{
    public class Startup
    {
        private const string TvInfoApiTitle = "TV Info API";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureMvc();
            services.ConfigureSwagger(TvInfoApiTitle);
            services.ConfigureShowContext(Configuration);
            services.ConfigureDependencyInjection();
            services.ConfigureLogging(Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSwagger();
            app.UseSwaggerUI(swaggerUiOptions =>
                swaggerUiOptions.SwaggerEndpoint("/swagger/v1/swagger.json", TvInfoApiTitle));

            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<ShowContext>().Database.EnsureCreated();
            }
            app.UseMvc();
        }
    }
}
