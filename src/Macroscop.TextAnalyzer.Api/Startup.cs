using Macroscop.TextAnalyzer.Api.Bll.Services;
using Macroscop.TextAnalyzer.Api.Bll.Services.Interfaces;
using Macroscop.TextAnalyzer.Api.Formatters;
using Macroscop.TextAnalyzer.Api.Middlewares;
using Macroscop.TextAnalyzer.Api.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;



namespace Macroscop.TextAnalyzer.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers(options => options.InputFormatters.Add(new PlainTextFormatter()))
                .Services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Macroscop.TextAnalyzer.Api",
                        Version = "v1"
                    });
                })
                .Configure<ConcurrencyLimitOptions>(Configuration.GetSection("ConcurrencyLimitOptions"))
                .AddScoped<ITextAnalyzeService, TextAnalyzeService>();



        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => 
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Macroscop.TextAnalyzer.Api v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseMiddleware<ConcurrencyLimiterMiddleware>();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}